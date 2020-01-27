using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Models.Queries;
using VirtualRoulette.Service.Iterfaces;

namespace VirtualRoulette.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BetController : ControllerBase
    {
        private IUserService _userService;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BetController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }/// <summary>
        /// Methot to Make new bet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("MakeBet")]
        public async Task<ActionResult<BetRow>> MakeBet(BetRequest request)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress currentIpAddress = hostEntry.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);
            int user =Convert.ToInt32(User.Identity.Name);
            var result = await _userService.MakeBet(request,user, currentIpAddress.ToString());
            return Ok(result);
        }
    }
}
