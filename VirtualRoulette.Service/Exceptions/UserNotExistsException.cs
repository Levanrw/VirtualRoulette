using System.Net;

namespace VirtualRoulette.Service.Exceptions
{
    public class UserNotExistsException : BaseApiException
    {
        public override string Message { get; }
        /// <summary>
        /// Exception will activate when User not found
        /// </summary>
        public UserNotExistsException() : base(HttpStatusCode.NotFound)
        {
            Message = "User Not Found";
        }
    }
}
