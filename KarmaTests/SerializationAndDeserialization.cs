using Karma;
using KarmaTests.TestTargets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace KarmaTests
{
    [TestClass]
    public class SerializationAndDeserialization
    {
        [TestMethod]
        public void WithNullObjects()
        {
            var obj = new TargetClass();
            obj.FillValues(0);

            RunTestIteration(obj, new KarmaOptions() { SerializeDefaultValues = true });
        }

        [TestMethod]
        public void WithoutNullObjects()
        {
            var obj = new TargetClass();
            obj.FillValues(0);

            RunTestIteration(obj, new KarmaOptions() { SerializeDefaultValues = false });
        }

        [TestMethod]
        public void OmmitingConstructor()
        {
            var obj = new TargetClassWithoutEmptyConstructor(Int32.MaxValue);

            RunTestIteration(obj, new KarmaOptions());
        }

        private void RunTestIteration<T>(IAssertable<T> obj, KarmaOptions options)
        {
            byte[] bytes;
            using (var mStream = new MemoryStream())
            using (var serializer = new KarmaSerializer(mStream, options))
            {
                serializer.Write(obj);
                bytes = mStream.ToArray();
            }

            using (var mStream = new MemoryStream(bytes))
            using (var deserializer = new KarmaDeserializer(mStream))
            {
                var resultObj = deserializer.Read<T>();
                obj.AssertEqualsTo(resultObj);
            }
        }
    }
}
