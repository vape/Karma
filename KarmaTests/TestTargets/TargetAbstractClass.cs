using Karma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaTests.TestTargets
{
    public abstract class TargetAbstractClass
    {
        [KarmaMember]
        public Int32 Int32AbstractClassField;

        public TargetAbstractClass()
        { }

        public TargetAbstractClass(int value)
        {
            Int32AbstractClassField = value;
        }

        public override bool Equals(object obj)
        {
            var o = obj as TargetAbstractClass;
            if (o != null)
            {
                return Int32AbstractClassField == o.Int32AbstractClassField;
            }

            return false;
        }
    }

    public class TargetClassInheritedFromAbstract : TargetAbstractClass
    {
        [KarmaMember]
        public Int32 Int32Field;

        public TargetClassInheritedFromAbstract()
        { }

        public TargetClassInheritedFromAbstract(int value)
            : base(value)
        {
            Int32Field = value;
        }

        public override bool Equals(object obj)
        {
            var o = obj as TargetClassInheritedFromAbstract;
            if (o != null)
            {
                return Int32AbstractClassField == o.Int32AbstractClassField && Int32Field == o.Int32Field;
            }

            return false;
        }
    }
}
