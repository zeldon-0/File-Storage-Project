using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner)
            : base(message, inner) { }
        public override string ToString()
        {
            return Message;
        }
    }
}
