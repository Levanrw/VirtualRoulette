using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Service.Iterfaces;
namespace VirtualRoulette.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JackpotController : ControllerBase
    {
        private IUserService _userService;
        public JackpotController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// metod to return current jackpot amount
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("JackpotAmount")]
        public async Task<ActionResult<decimal>> JackpotAmount()
        {
            var result = await _userService.GetJackpot();
            return Ok(result);
        }

    }
}