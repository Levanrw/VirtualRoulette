using System.Threading.Tasks;
using VirtualRoulette.Models.Queries;

namespace VirtualRoulette.DAL.Irepositories
{
    public interface ICommandRepositor
    {/// <summary>
     /// Generate new spin
     /// </summary>
     /// <returns></returns>
        Task<int> GenerateSpin();
        /// <summary>
        /// Make bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> MakeBet(BetCommand command);
        /// <summary>
        /// update bet to set won amount
        /// </summary>
        /// <param name="BetId">current bet id</param>
        /// <param name="WonAmount">won amount</param>
        void UpdateBet(int BetId, int WonAmount);
        /// <summary>
        /// update user balance after spin(if it was win)
        /// </summary>
        /// <param name="UserId"> current user Id</param>
        /// <param name="Balance">New balance</param>
        void UpdateUserBalance(int UserId, int Balance);
        /// <summary>
        /// set win random number to spin
        /// </summary>
        /// <param name="SpinId"> current spin id</param>
        /// <param name="WinNumber"> random number</param>
        void SetWinNumber(int SpinId, int WinNumber);
        /// <summary>
        /// update Jackpot after bet
        /// </summary>
        /// <param name="JackpotAmount">new jackpot amount (0.1% per bet)</param>
        void UpdateJackpot(decimal JackpotAmount);
    }
}
