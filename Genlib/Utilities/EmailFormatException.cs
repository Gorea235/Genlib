using System;
using System.Collections.Generic;
using System.Text;

namespace Genlib.Utilities
{
    public class EmailFormatException : Exception
    {
        public EmailFormatException() { }
        public EmailFormatException(string message) : base(message) { }
        public EmailFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
