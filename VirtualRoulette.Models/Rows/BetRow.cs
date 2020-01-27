using System;

namespace VirtualRoulette.Models.Queries
{
    /// <summary>
    /// Object which returns to user
    /// </summary>
    public class BetRow
    {/// <summary>
     /// Bet's status (accepted or no)
     /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// SpinId on which made the bet
        /// </summary>
        public int SpinID { get; set; }
        /// <summary>
        /// Spin's win numer
        /// </summary>
        public int WInNumber { get; set; }
        /// <summary>
        /// Amount which won client after last bet
        /// </summary>
        public decimal WonAmount { get; set; }
        /// <summary>
        /// Bet's create date
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
