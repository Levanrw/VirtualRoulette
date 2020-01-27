using System;

namespace VirtualRoulette.Models.Queries
{
    public class GameHistoryRow
    {
        public int SpinID { get; set; }
        public int BetAmount { get; set; }
        public int WonAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
