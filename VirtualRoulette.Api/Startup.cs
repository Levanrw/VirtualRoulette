using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VirtualRoulette.Service;
using VirtualRoulette.DAL.DBProvider;
using VirtualRoulette.DAL.Repositories;
using VirtualRoulette.DAL.Irepositories;
using VirtualRoulette.Service.SignManager;
using VirtualRoulette.Models;
using VirtualRoulette.Service.Iterfaces;
using Microsoft.AspNetCore.Http;
using VirtualRoulette.Api.Filters;
using System.IO;
using System;

namespace VirtualRoulette.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            VirtualRoulette.DAL.DBProvider.DataAccessLayerBase.Configuration = Configuration;
           
            services.AddCors();
            ConfigurationSwagger(services);
            services.AddTransient<IDBProvider, DBProvider>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IQueryrRepositor , QueryRepositor> ();
            services.AddTransient<ISignInManager, SignInManager>();
            services.AddTransient<ICommandRepositor, CommandRepositor>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<TokenSettings>(Configuration.GetSection("AppSettings"));

            var token = Configuration.GetSection("AppSettings").Get<TokenSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                   ValidIssuer = token.Issuer,
                   ValidAudience = token.Audience,
                   ValidateAudience = false,
                   ValidateIssuer = false
               };
           });
            services.AddControllers();
            services.AddMvc(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.IgnoreNullValues = true;            
                
            })
           .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        private  void ConfigurationSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
                foreach (var xmlCommentFile in currentDir.EnumerateFiles("VirtualRoulette.*.xml"))
                    c.IncludeXmlComments(xmlCommentFile.FullName);

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });           

        }

        // This method gets called by the runtime.. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());           
        }
    }
}
