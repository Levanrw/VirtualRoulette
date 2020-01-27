using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualRoulette.Models;
using VirtualRoulette.Models.Queries;

namespace VirtualRoulette.DAL.Irepositories
{
    public interface IQueryrRepositor
    {/// <summary>
    /// metod for autorization
    /// </summary>
    /// <param name="userName">client username</param>
    /// <returns></returns>
        Task<UserRow> GetUser(string userName);
        /// <summary>
        /// Get current user's balance
        /// </summary>
        /// <param name="UserId"> current userId</param>
        /// <returns></returns>
        Task<int> GetUserBalance(int UserId);
        /// <summary>
        /// Get current spin Id
        /// </summary>
        /// <returns></returns>
        Task<int> GetActiveSpin();
        /// <summary>
        /// Chek for Users spin (if user's has bet on current spin,he cannt bet again on current spin)
        /// </summary>
        /// <param name="SpinId"> active spin Id</param>
        /// <param name="UserId"> active user Id</param>
        /// <returns></returns>
        Task<int> ChekUserBetPermission(int SpinId, int UserId);
        /// <summary>
        /// Get current jackpot amount
        /// </summary>
        /// <returns></returns>
        Task<decimal> CurrentJackpot();
        /// <summary>
        /// Get User's Bets count (for paging)
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns></returns>
        Task<int> GetNumberOfBets(int userId);
        /// <summary>
        /// Get current Users games history with Paging
        /// </summary>
        /// <param name="userId"> current User's Id</param>
        /// <param name="pageNumber">Page number </param>
        /// <param name="itemsPerPage">count of data on page</param>
        /// <returns></returns>
        Task<IEnumerable<GameHistoryRow>> GetUserGameHistory(int userId, int? pageNumber, int? itemsPerPage);
    }
}