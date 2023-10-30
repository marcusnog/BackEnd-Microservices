using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.Exceptions
{
    public class CodeException : Exception
    {
        public string ErrorCode { get; set; }

        public CodeException(string errorCode, string? message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public CodeException(string errorCode, string? message, Exception? innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
