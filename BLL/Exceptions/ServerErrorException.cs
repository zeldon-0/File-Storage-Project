using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class ServerErrorException : Exception
    { 
        public ServerErrorException() : base() { }
        public ServerErrorException(string message) : base(message) { }
        public ServerErrorException(string message, Exception inner)
            : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
}
