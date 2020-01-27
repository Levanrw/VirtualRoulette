using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GameHistoryController : ControllerBase
    {
        private IUserService _userService;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GameHistoryController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Method which returns list of bets history with paging
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GameHistory")]
        public async Task<ActionResult<PagedData<GameHistoryRow>>> MakeBet(GameHistoryRequest query)
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            var Result =await _userService.GameHistory(query, userId);
            return Ok(Result);
        }
    }
}
