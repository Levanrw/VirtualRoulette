using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualRoulette.DAL.DBProvider;
using VirtualRoulette.DAL.Irepositories;
using VirtualRoulette.Models;
using VirtualRoulette.Models.Queries;

namespace VirtualRoulette.DAL.Repositories
{
    public class QueryRepositor : IQueryrRepositor
    {
        private readonly IDBProvider _dbProvider;
        public QueryRepositor(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        /// <summary>
        /// metod for autorization
        /// </summary>
        /// <param name="userName">client username</param>
        /// <returns></returns>
        public async Task<UserRow> GetUser(string userName)
        {
            string sql = "SELECT * FROM Users WHERE UserName = @userName";

            UserRow user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                //db.Close();
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserRow>(sql, new { userName });

                db.Close();
            }

            return user;
        }
        /// <summary>
        /// Get current user's balance
        /// </summary>
        /// <param name="UserId"> current userId</param>
        /// <returns></returns>
        public async Task<int> GetUserBalance(int UserId)
        {
            string sql = "SELECT Id,Balance FROM Users WHERE Id = @UserId";

            UserRow user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                //db.Close();
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserRow>(sql, new { UserId });

                db.Close();
            }

            return user.Balance;
        }
        /// <summary>
        /// Get current spin Id
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetActiveSpin()
        {
            string sql = "SELECT Id FROM Spin WHERE Status = 1";

            int SpinId;

            using (var db = _dbProvider.GetDBInstance())
            {
                //db.Close();
                db.Open();

                SpinId = await db.QueryFirstOrDefaultAsync<int>(sql);

                db.Close();
            }

            return SpinId;
        }
        /// <summary>
        /// Chek for Users spin (if user's has bet on current spin,he cannt bet again on current spin)
        /// </summary>
        /// <param name="SpinId"> active spin Id</param>
        /// <param name="UserId"> active user Id</param>
        /// <returns></returns>
        public async Task<int> ChekUserBetPermission(int SpinId,int UserId)
        {
            string sql = "SELECT Id FROM Bet WHERE SpinId = @SpinId and UserId=@UserId";

            int BetId;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                BetId = await db.QueryFirstOrDefaultAsync<int>(sql,new { SpinId= SpinId ,UserId= UserId });
                db.Close();
            }

            return BetId;
        }
        /// <summary>
        /// Get current jackpot amount
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> CurrentJackpot()
        {
            string sql = "SELECT Amount FROM jackpot";

            decimal Jackpot;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                Jackpot = await db.QueryFirstOrDefaultAsync<decimal>(sql);
                db.Close();
            }

            return Jackpot;
        }
        /// <summary>
        /// Get current Users games history with Paging
        /// </summary>
        /// <param name="userId"> current User's Id</param>
        /// <param name="pageNumber">Page number </param>
        /// <param name="itemsPerPage">count of data on page</param>
        /// <returns></returns>
        public async Task<IEnumerable<GameHistoryRow>> GetUserGameHistory(int userId, int? pageNumber, int? itemsPerPage)
        {
            var skip = pageNumber == null ? 0 : (pageNumber - 1) * itemsPerPage;
            var take = itemsPerPage == null ? 20 : itemsPerPage;

            string userQuery = "SELECT Id FROM Users WHERE Id = @userId";

            string betQuery = "SELECT SpinId, BetAmount, CreateDate, WonAmount FROM Bet WHERE UserId = @userId ORDER BY CreateDate DESC LIMIT @skip, @take";

            int? user = null;
            IEnumerable<GameHistoryRow> historyRows = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<int>(userQuery, new { userId });

                if (user != null)
                    historyRows = await db.QueryAsync<GameHistoryRow>(betQuery, new { userId, skip, take });

                db.Close();
            }

            return historyRows;
        }
        /// <summary>
        /// Get User's Bets count (for paging)
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns></returns>
        public async Task<int> GetNumberOfBets(int userId)
        {
            string sql = "SELECT COUNT(Id) FROM Bet WHERE UserId = @userId";

            int numberOfBets = default(int);

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                numberOfBets = await db.ExecuteScalarAsync(sql, new { userId });

                db.Close();
            }
            return numberOfBets;
        }

    }
}
