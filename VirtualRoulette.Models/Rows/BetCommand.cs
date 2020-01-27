using System;
using System.Collections.Generic;
using System.Text;
using VirtualRoulette.Models.Enum;

namespace VirtualRoulette.Models.Queries
{/// <summary>
/// Object to record in database after Bet
/// </summary>
  public class BetCommand
    {/// <summary>
    /// Active Users Id
    /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Bet string which send client
        /// </summary>
        public string BetString { get; set; }
        /// <summary>
        /// Won amount after bet
        /// </summary>
        public int WonAmount { get; set; }
        /// <summary>
        /// Clients public IP address
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Amount which beted client
        /// </summary>
        public long BetAmount { get; set; }
        /// <summary>
        /// Bet status (Correctly or no)
        /// </summary>
        public BetStatus Status { get; set; }
        /// <summary>
        /// Bet create date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Row will be active or no
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Current Spin Id
        /// </summary>
        public int SpinId { get; set; }
      

    }
}
