using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaTests.TestTargets
{
    public interface IAssertable<T>
    {
        void AssertEqualsTo(T obj);
    }
}
