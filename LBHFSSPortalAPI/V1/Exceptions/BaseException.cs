using System;

namespace LBHFSSPortalAPI.V1.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Detailed information on the cause of the API error
        /// </summary>
        public string ApiErrorMessage { get; set; }

        /// <summary>
        /// Low level error detail including call stack if necessary (for API diagnostics)
        /// </summary>
        public string DeveloperErrorMessage { get; set; }        
    }
}
