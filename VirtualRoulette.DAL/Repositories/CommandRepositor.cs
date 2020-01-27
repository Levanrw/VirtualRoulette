using System;
using System.Threading.Tasks;
using VirtualRoulette.DAL.DBProvider;
using VirtualRoulette.DAL.Irepositories;
using VirtualRoulette.Models.Enum;
using VirtualRoulette.Models.Queries;

namespace VirtualRoulette.DAL.Repositories
{
    public class CommandRepositor : ICommandRepositor
    {
        private readonly IDBProvider _dbProvider;

        public CommandRepositor(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        /// <summary>
        /// Generate new spin
        /// </summary>
        /// <returns></returns>
        public async Task<int> GenerateSpin()
        {
            SpinRow Spin = new SpinRow();
            DateTime CurrentDate = DateTime.Now;
            string changespinstatus = "Update spin set Status=@Status,CloseDate=@CloseDate where Status=1";
            string sql = "insert into spin(IsActive,CreateDate,Status) values(@IsActive,@CreateDate,@Status)";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                await db.ExecuteAsync(changespinstatus, new { Status = SpinStatus.Completed, CloseDate = CurrentDate });
                Spin.SpinId = await db.ExecuteAsync(sql, new { IsActive = 1, CreateDate = CurrentDate, Status = SpinStatus.InProgress });
                db.Close();
            }
            return Spin.SpinId;
        }

        public async Task<int> SetSpinWiningNumber(int SpinId)
        {
            DateTime CurrentDate = DateTime.Now;
            string SetWinNumber = "Update spin set WinNumber=@WinNumber where SpinId=@SpinId";
            using (var db = _dbProvider.GetDBInstance())
            {
                Random rnd = new Random();
                int WinNumber = rnd.Next(0, 36);
                db.Open();
                SpinId = await db.ExecuteAsync(SetWinNumber, new { WinNumber = WinNumber, SpinId = SpinId });
                db.Close();
            }
            return SpinId;
        }
        /// <summary>
        /// Make bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> MakeBet(BetCommand command)
        {
            DateTime CurrentDate = DateTime.Now;
            int BetId;
            string sql = "insert into Bet(UserId,BetString,WonAmount,IpAddress,BetAmount,Status,CreateDate,IsActive,SpinId) values(@UserId,@BetString,@WonAmount,@IpAddress,@BetAmount,@Status,@CreateDate,@IsActive,@SpinId)";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                BetId = await db.ExecuteAsync(sql, new { UserId = command.UserId, BetString = command.BetString, WonAmount = command.WonAmount, IpAddress = command.IpAddress, BetAmount = command.BetAmount, Status = command.Status, CreateDate = command.CreateDate, IsActive = command.IsActive, SpinId = command.SpinId });
                db.Close();
            }
            return BetId;

        }
        /// <summary>
        /// set win random number to spin
        /// </summary>
        /// <param name="SpinId"> current spin id</param>
        /// <param name="WinNumber"> random number</param>
        public async void SetWinNumber(int SpinId, int WinNumber)
        {
            string sql = "update Spin set WinNumber=@WinNumber where Id=@SpinId";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                await db.ExecuteAsync(sql, new { WinNumber = WinNumber, SpinId = SpinId });
                db.Close();
            }

        }
        /// <summary>
        /// update bet to set won amount
        /// </summary>
        /// <param name="BetId">current bet id</param>
        /// <param name="WonAmount">won amount</param>
        public async void UpdateBet(int BetId, int WonAmount)
        {
            string sql = "update Bet set WonAmount=@WonAmount where Id=@BetId";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                await db.ExecuteAsync(sql, new { WonAmount = WonAmount, BetId = BetId });
                db.Close();
            }

        }
        /// <summary>
        /// update user balance after spin(if it was win)
        /// </summary>
        /// <param name="UserId"> current user Id</param>
        /// <param name="Balance">New balance</param>
        public async void UpdateUserBalance(int UserId, int Balance)
        {
            string sql = "update Users set Balance=@Balance where Id=@UserId";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                await db.ExecuteAsync(sql, new { Balance = Balance, UserId = UserId });
                db.Close();
            }

        }
        /// <summary>
        /// update Jackpot after bet
        /// </summary>
        /// <param name="JackpotAmount">new jackpot amount (0.1% per bet)</param>
        public async void UpdateJackpot(decimal JackpotAmount)
        {
            DateTime CurrentDate = DateTime.Now;
            string sql = "Update Jackpot set Amount=@Amount, CreateDate=@CreateDate ";
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();
                await db.ExecuteAsync(sql, new { Amount = JackpotAmount, CreateDate = CurrentDate });
                db.Close();
            }

        }

    }
}
