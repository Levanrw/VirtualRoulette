using System.Net;

namespace VirtualRoulette.Service.Exceptions
{
    public class IncorrectBetException : BaseApiException
    {
        public override string Message { get; }
        /// <summary>
        /// exception will be activate when bet request will not be in correct format
        /// </summary>
        public IncorrectBetException() : base(HttpStatusCode.BadRequest)
        {
            Message = "Bet Not Correct";
        }

        public IncorrectBetException(string message) : base(HttpStatusCode.BadRequest)
        {
            Message = message;
        }
    }
}
