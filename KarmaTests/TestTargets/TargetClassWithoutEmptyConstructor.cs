using Karma;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KarmaTests.TestTargets
{
    public class TargetClassWithoutEmptyConstructor : IAssertable<TargetClassWithoutEmptyConstructor>
    {
        [KarmaMember]
        public int Int32Field;

        public TargetClassWithoutEmptyConstructor(int someParameter)
        {
            Int32Field = someParameter;
        }

        public void AssertEqualsTo(TargetClassWithoutEmptyConstructor obj)
        {
            Assert.AreEqual(Int32Field, obj.Int32Field);
        }
    }
}
