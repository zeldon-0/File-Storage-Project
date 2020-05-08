using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Exception_Filtering
{
    class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;
        public object Value { get; set; }
    }
}
