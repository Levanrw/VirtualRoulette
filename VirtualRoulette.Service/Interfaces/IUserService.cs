using System;
using System.Threading.Tasks;
using VirtualRoulette.Models.Queries;

namespace VirtualRoulette.Service.Iterfaces
{
    public interface IUserService
    {/// <summary>
     /// Method to autorization
     /// </summary>
     /// <param name="query"></param>
     /// <returns></returns>
        Task<string> GetUser(LoginRequest query);
        /// <summary>
        /// Method which returns user's balance
        /// </summary>
        /// <param name="UserId"> Current User's Id</param>
        /// <returns></returns>
        Task<int> GetUserBalance(int UserId);
        /// <summary>
        /// Generate new spin
        /// </summary>
        void GenerateSpin();
        /// <summary>
        /// Make bet
        /// </summary>
        /// <param name="request"> Request from User</param>
        /// <param name="UserId"> current user's Id</param>
        /// <param name="IpAddress">User's IP address</param>
        /// <returns></returns>
        Task<BetRow> MakeBet(BetRequest request, int UserId, string IpAddress);
        /// <summary>
        /// Current jackpots method
        /// </summary>
        /// <returns></returns>
        Task<Decimal> GetJackpot();
        /// <summary>
        ///Client's game hostory method 
        /// </summary>
        /// <param name="query">Request from user </param>
        /// <param name="UserId">Current user's Id</param>
        /// <returns></returns>
        Task<PagedData<GameHistoryRow>> GameHistory(GameHistoryRequest query, int UserId);
    }
}
