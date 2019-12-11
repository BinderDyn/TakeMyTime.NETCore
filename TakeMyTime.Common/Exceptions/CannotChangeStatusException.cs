using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.Common.Exceptions
{
    public sealed class CannotChangeStatusException : Exception
    {
        public CannotChangeStatusException(string message) : base(message)
        {
        }
    }
}
