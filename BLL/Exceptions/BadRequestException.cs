using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BLL.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base() {}
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception inner) 
            : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
}
