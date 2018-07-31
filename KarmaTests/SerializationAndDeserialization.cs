using Karma;
using KarmaTests.TestTargets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace KarmaTests
{
    [TestClass]
    public class SerializationAndDeserialization
    {
        [TestMethod]
        public void WithNullObjects()
        {
            RunTestIteration(new KarmaOptions() { SerializeDefaultValues = true });
        }

        [TestMethod]
        public void WithoutNullObjects()
        {
            RunTestIteration(new KarmaOptions() { SerializeDefaultValues = false });
        }

        private void RunTestIteration(KarmaOptions options)
        {
            var obj = new TargetClass();
            obj.FillValues(0);

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
                var resultObj = deserializer.Read<TargetClass>();
                obj.AssertEqualsTo(resultObj);
            }
        }
    }
}
