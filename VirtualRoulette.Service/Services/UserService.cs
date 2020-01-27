using ge.singular.roulette;
using System;
using System.Threading.Tasks;
using VirtualRoulette.DAL.Irepositories;
using VirtualRoulette.Models.Enum;
using VirtualRoulette.Models.Queries;
using VirtualRoulette.Service.Exceptions;
using VirtualRoulette.Service.Iterfaces;
using VirtualRoulette.Service.SignManager;

namespace VirtualRoulette.Service
{
    public class UserService : IUserService
    {
        private readonly IQueryrRepositor _queryrepository;
        private readonly ICommandRepositor _commandrepository;
        private readonly ISignInManager _signInManager;
        public UserService(IQueryrRepositor queryrepository, ISignInManager signInManager, ICommandRepositor commandrepository)
        {
            _commandrepository = commandrepository;
            _queryrepository = queryrepository;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Method to autorization
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> GetUser(LoginRequest query)
        {
            var user = await _queryrepository.GetUser(query.UserName)
                ?? throw new UserNotExistsException();
            var password = _signInManager.GetPassword(query.Password, user.Salt);
            if (user.Password != password)
                throw new UserNotExistsException();
            return _signInManager.GenerateToken(query.UserName, user.Id.ToString());
        }
        /// <summary>
        /// Method which returns user's balance
        /// </summary>
        /// <param name="UserId"> Current User's Id</param>
        /// <returns></returns>
        public async Task<int> GetUserBalance(int UserId)
        {
            var UserBalance = await _queryrepository.GetUserBalance(UserId);
            return UserBalance;
        }
        /// <summary>
        /// Generate new spin
        /// </summary>
        public async void GenerateSpin()
        {
            await _commandrepository.GenerateSpin();

        }
        /// <summary>
        /// Make bet
        /// </summary>
        /// <param name="request"> Request from User</param>
        /// <param name="UserId"> current user's Id</param>
        /// <param name="IpAddress">User's IP address</param>
        /// <returns></returns>
        public async Task<BetRow> MakeBet(BetRequest request, int UserId, string IpAddress)
        {
            GenerateSpin();
            int SpinId = await _queryrepository.GetActiveSpin();
            int UserBetPermisson = await _queryrepository.ChekUserBetPermission(SpinId, UserId);
            if (UserBetPermisson > 0)
            {
                throw new ArgumentException("You already have the bet on the current spin, please wait or generate new spin");
            }
            IsBetValidResponse ibvr = CheckBets.IsValid(Convert.ToString(request.BetString));
            if (!ibvr.getIsValid() || ibvr.getBetAmount() <= 0)
            {
                throw new IncorrectBetException("Bet is not in correct format");
            }
            else
            {
                BetRow betRow = new BetRow();
                BetCommand command = new BetCommand();
                command.SpinId = SpinId;
                command.IpAddress = IpAddress;
                command.UserId = UserId;
                command.BetAmount = ibvr.getBetAmount();
                command.WonAmount = 0;
                command.Status = BetStatus.Lose;
                command.IsActive = true;
                command.CreateDate = DateTime.Now;
                command.BetString = Convert.ToString(request.BetString);

                var UserBalance = await _queryrepository.GetUserBalance(UserId);
                if (UserBalance < command.BetAmount)
                {
                    throw new ArgumentException("you have not enough balance for bet");
                }
                else
                {
                    decimal JakpotAmount = await _queryrepository.CurrentJackpot();
                    int BetId = await _commandrepository.MakeBet(command);
                    JakpotAmount += (ibvr.getBetAmount() * 0.01M);
                    UserBalance -= (int)command.BetAmount;
                    _commandrepository.UpdateJackpot(JakpotAmount);
                    _commandrepository.UpdateUserBalance(UserId, UserBalance);
                    Random rnd = new Random();
                    int WinNumber = rnd.Next(0, 36);
                    // _commandrepository.SetSpinWiningNumber(WinNumber);
                    _commandrepository.SetWinNumber(SpinId, WinNumber);
                    int WinMoney = CheckBets.EstimateWin(Convert.ToString(request.BetString), WinNumber);
                    if (WinMoney > 0)
                    {
                        _commandrepository.UpdateBet(BetId, WinMoney);
                        UserBalance += WinMoney;
                        _commandrepository.UpdateUserBalance(UserId, UserBalance);
                    }
                    betRow.SpinID = SpinId;
                    betRow.Status = "Bet accepted";
                    betRow.WInNumber = WinNumber;
                    betRow.WonAmount = WinMoney;
                    betRow.CreateDate = DateTime.Now;
                }
                return betRow;
            }
        }
        /// <summary>
        /// Current jackpots method
        /// </summary>
        /// <returns></returns>
        public async Task<Decimal> GetJackpot()
        {
            decimal Jackpot = await _queryrepository.CurrentJackpot();
            return Jackpot;
        }
        /// <summary>
        ///Client's game hostory method 
        /// </summary>
        /// <param name="query">Request from user </param>
        /// <param name="UserId">Current user's Id</param>
        /// <returns></returns>
        public async Task<PagedData<GameHistoryRow>> GameHistory(GameHistoryRequest query, int UserId)
        {
            var data = await _queryrepository.GetUserGameHistory(UserId, query.PageNumber, query.Take)
                 ?? throw new UserNotExistsException();

            var numberOfRows = await _queryrepository.GetNumberOfBets(UserId);

            return new PagedData<GameHistoryRow>(data, numberOfRows);
        }
    }
}
