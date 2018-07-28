using System;

namespace Karma
{
    public class KarmaException : Exception
    {
        public KarmaException(string message)
            : base(message)
        { }
    }
}
