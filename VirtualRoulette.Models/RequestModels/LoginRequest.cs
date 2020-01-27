namespace VirtualRoulette.Models.Queries
{/// <summary>
/// Request for user autorization
/// </summary>
    public class LoginRequest
    {      /// <summary>
           /// Users's username
           /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// users password
        /// </summary>
        public string Password { get; set; }
    }
}
