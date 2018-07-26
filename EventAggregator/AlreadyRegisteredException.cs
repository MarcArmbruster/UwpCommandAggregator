using System;

namespace UwpEventAggregator
{
    /// <summary>
    /// Already registered exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class AlreadyRegisteredException : Exception
    {
        public AlreadyRegisteredException(string message) : base(message)
        {
        }
    }
}
