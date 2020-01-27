namespace VirtualRoulette.Models
{/// <summary>
/// Object to get current uses's data
/// </summary>
    public class UserRow
    {/// <summary>
    /// Current user's Id
    /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User's firstname
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// users lastname
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// users username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// users passwors
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// users salt
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// users current balance
        /// </summary>
        public int Balance { get; set; }
    }
}
