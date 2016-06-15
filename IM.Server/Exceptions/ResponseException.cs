using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Server
{
    public class ResponseException : Exception
    {
        public ResponseException(string message)
            : base(message)
        { }
    }
}
