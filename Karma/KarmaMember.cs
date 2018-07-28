using System;

namespace Karma
{
    public class KarmaMemberAttribute : Attribute
    {
        public readonly string CustomName;

        public KarmaMemberAttribute(string customName = null)
        {
            CustomName = customName;
        }
    }
}
