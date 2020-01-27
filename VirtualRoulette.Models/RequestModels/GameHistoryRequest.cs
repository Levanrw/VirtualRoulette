namespace VirtualRoulette.Models.Queries
{/// <summary>
 /// Game history request
 /// </summary>
    public class GameHistoryRequest
    {
        /// <summary>
        /// Optional parameter for paging
        /// </summary>
        public int? PageNumber { get; set; }
        /// <summary>
        /// Optional parameter for paging
        /// </summary>
        public int? Take { get; set; }
    }
}
