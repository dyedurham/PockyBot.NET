using System;

namespace PockyBot.NET.Models.Exceptions
{
    public class PegValidationException : Exception
    {
        public PegValidationException() { }

        public PegValidationException(string message) : base(message) { }

        public PegValidationException(string message, Exception inner) : base(message, inner) { }
    }
}
