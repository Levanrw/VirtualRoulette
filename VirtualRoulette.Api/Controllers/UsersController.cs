using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VirtualRoulette.Models.Queries;
using VirtualRoulette.Service.Iterfaces;

namespace VirtualRoulette.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// User authenticate
        /// UserName: Gamer
        /// Password: 12345
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginRequest query)
        {
           var result= await _userService.GetUser(query);
            return  Ok(result);
        }
        /// <summary>.
        /// Method to get current user balance
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("UserBalance")]
        public async Task<IActionResult> GetBalance()
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            var result = await _userService.GetUserBalance(userId);
            return Ok(result);
        }

    }
}
