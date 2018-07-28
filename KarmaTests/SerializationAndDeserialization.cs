using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karma;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KarmaTests
{
    public class SerializationSubTarget
    {
        [KarmaMember]
        public string SomeString;
        [KarmaMember]
        public double SomeNumber;

        [KarmaMember]
        SerializationSubTarget SubTargetOfSubTarget;

        public void SetValues()
        {
            SomeString = "SomeStringValue";
            SomeNumber = 123456.123d;

            SubTargetOfSubTarget = new SerializationSubTarget()
            {
                SomeNumber = 654321.009d,
                SomeString = "SomeStringValueDepth"
            };
        }

        public void Check(SerializationSubTarget target)
        {
            Assert.AreEqual(SomeString, target.SomeString);
            Assert.AreEqual(SomeNumber, target.SomeNumber);
            Assert.AreEqual(SubTargetOfSubTarget.SomeString, target.SubTargetOfSubTarget.SomeString);
            Assert.AreEqual(SubTargetOfSubTarget.SomeString, target.SubTargetOfSubTarget.SomeString);
        }
    }

    public class SerializationTarget
    {
        public string TestString;

        [KarmaMember]
        public int[] Int32ArrayField;
        [KarmaMember]
        private Int16 Int16Field;
        [KarmaMember]
        private Int32 Int32Field;
        [KarmaMember]
        private Int64 Int64Field;
        [KarmaMember]
        protected UInt16 UInt16Field;
        [KarmaMember]
        protected UInt32 UInt32Field;
        [KarmaMember]
        protected UInt64 UInt64Field;
        [KarmaMember]
        public Double DoubleField;
        [KarmaMember]
        public Single SingleField;
        [KarmaMember]
        public String StringField;
        [KarmaMember]
        public Byte ByteField;
        [KarmaMember]
        public SByte SByteField;
        [KarmaMember]
        public Guid GuidField;

        [KarmaMember]
        public int[] Int32ArrayProperty { get; private set; }
        [KarmaMember]
        public string[] StringArrayProperty { get; private set; }
        [KarmaMember]
        public List<string> StringListProperty { get; private set; }
        [KarmaMember]
        public Dictionary<string, List<int>> DictionaryProperty { get; private set; }
        [KarmaMember]
        public Int16 Int16Property { get; private set; }
        [KarmaMember]
        public Int32 Int32Property { get; private set; }
        [KarmaMember]
        public Int64 Int64Property { get; private set; }
        [KarmaMember]
        public UInt16 UInt16Property { get; set; }
        [KarmaMember]
        public UInt32 UInt32Property { get; set; }
        [KarmaMember]
        public UInt64 UInt64Property { get; set; }
        [KarmaMember]
        public Double DoubleProperty { get; set; }
        [KarmaMember]
        public Single SingleProperty { get; set; }
        [KarmaMember]
        public Byte ByteProperty { get; set; }
        [KarmaMember]
        public SByte SByteProperty { get; set; }
        [KarmaMember]
        public String StringProperty
        {
            get { return stringPropertyValue; }
            set
            {
                stringPropertyValue = value;
                TestString = "Holla";
            }
        }
        [KarmaMember]
        public Guid GuidProperty { get; set; }

        [KarmaMember]
        public SerializationSubTarget SubTargetField;
        [KarmaMember]
        public SerializationSubTarget SubTargetProperty { get; set; }

        private string stringPropertyValue;

        public void SetValues()
        {
            Int32ArrayField = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Int16Field = -1234;
            Int32Field = -123456789;
            Int64Field = -1234567891011121314;
            UInt16Field = 1234;
            UInt32Field = 123456789;
            UInt64Field = 1234567891011121314;
            DoubleField = 1234.5678d;
            SingleField = 1234.56f;
            StringField = "HollaString";
            ByteField = 196;
            SByteField = -64;
            GuidField = Guid.NewGuid();

            Int32ArrayProperty = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1 };
            StringArrayProperty = new string[3] { "String1", "Стринг2", "行3" };
            StringListProperty = new List<string>() { "String1", "Стринг2", "行3" };
            DictionaryProperty = new Dictionary<string, List<int>>()
            {
                { "String", new List<int>() { 1, 2, 3, 4 } },
                { "Строка", new List<int>() { 5, 6, 7, 8 } },
                { "行", new List<int>() { 9, 10, 11, 12 } }
            };
            Int16Property = -9234;
            Int32Property = -923456789;
            Int64Property = -934567891011121314;
            UInt16Property = 9234;
            UInt32Property = 923456789;
            UInt64Property = 934567891011121314;
            DoubleProperty = 91234.5678d;
            SingleProperty = 91234.56f;
            StringProperty = "HollaStringProp";
            ByteProperty = 164;
            SByteProperty = -32;
            GuidProperty = Guid.NewGuid();

            SubTargetField = new SerializationSubTarget();
            SubTargetProperty = new SerializationSubTarget();
            SubTargetField.SetValues();
            SubTargetProperty.SetValues();
        }

        public void Check(SerializationTarget target)
        {
            Assert.IsTrue(Utils.ValuesEquals(Int32ArrayField, target.Int32ArrayField));
            Assert.AreEqual(Int16Field, target.Int16Field);
            Assert.AreEqual(Int32Field, target.Int32Field);
            Assert.AreEqual(Int64Field, target.Int64Field);
            Assert.AreEqual(UInt16Field, target.UInt16Field);
            Assert.AreEqual(UInt32Field, target.UInt32Field);
            Assert.AreEqual(UInt64Field, target.UInt64Field);
            Assert.AreEqual(DoubleField, target.DoubleField);
            Assert.AreEqual(SingleField, target.SingleField);
            Assert.AreEqual(ByteField, target.ByteField);
            Assert.AreEqual(SByteField, target.SByteField);
            Assert.AreEqual(StringField, target.StringField);
            Assert.AreEqual(GuidField, target.GuidField);

            Assert.IsTrue(Utils.ValuesEquals(Int32ArrayProperty, target.Int32ArrayProperty));
            Assert.IsTrue(Utils.ValuesEquals(Int32ArrayProperty, target.Int32ArrayProperty));
            Assert.IsTrue(Utils.ValuesEquals(StringArrayProperty, target.StringArrayProperty));
            Assert.IsTrue(Utils.ValuesEquals(StringListProperty, target.StringListProperty));
            Assert.IsTrue(Utils.ValuesEquals(DictionaryProperty, target.DictionaryProperty));
            Assert.AreEqual(Int16Property, target.Int16Property);
            Assert.AreEqual(Int32Property, target.Int32Property);
            Assert.AreEqual(Int64Property, target.Int64Property);
            Assert.AreEqual(UInt16Property, target.UInt16Property);
            Assert.AreEqual(UInt32Property, target.UInt32Property);
            Assert.AreEqual(UInt64Property, target.UInt64Property);
            Assert.AreEqual(DoubleProperty, target.DoubleProperty);
            Assert.AreEqual(SingleProperty, target.SingleProperty);
            Assert.AreEqual(ByteProperty, target.ByteProperty);
            Assert.AreEqual(SByteProperty, target.SByteProperty);
            Assert.AreEqual(StringProperty, target.StringProperty);
            Assert.AreEqual(GuidProperty, target.GuidProperty);

            Assert.AreEqual(TestString, target.TestString);

            SubTargetField.Check(target.SubTargetField);
            SubTargetProperty.Check(target.SubTargetProperty);
        }
    }

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
            var obj = new SerializationTarget();
            obj.SetValues();

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
                var resultObj = deserializer.Read<SerializationTarget>();
                obj.Check(resultObj);
            }
        }
    }
}
