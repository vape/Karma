﻿using Karma;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KarmaTests.TestTargets
{
    public class TargetClass : IAssertable<TargetClass>
    {
        [KarmaMember]
        public TargetClass SubTargetClassField;
        [KarmaMember]
        public TargetClass SubTargetClassProperty
        { get; private set; }

        [KarmaMember]
        public TargetStruct SubTargetStructField;
        [KarmaMember]
        public TargetStruct SubTargetStructProperty
        { get; private set; }

        [KarmaMember]
        public int[] Int32ArrayField;
        [KarmaMember]
        public List<int> Int32ListField;
        [KarmaMember]
        public Dictionary<int, int> Int32Int32DictionaryField;

        [KarmaMember]
        public int[] Int32ArrayProperty
        { get; private set; }
        [KarmaMember]
        public List<int> Int32ListProperty
        { get; private set; }
        [KarmaMember]
        public Dictionary<int, int> Int32Int32DictionaryProperty
        { get; private set; }

        [KarmaMember]
        public Int16 Int16Field;
        [KarmaMember]
        protected Int32 Int32Field;
        [KarmaMember]
        private Int64 Int64Field;
        [KarmaMember]
        public UInt16 UInt16Field;
        [KarmaMember]
        protected UInt32 UInt32Field;
        [KarmaMember]
        private UInt64 UInt64Field;
        [KarmaMember]
        public Double DoubleField;
        [KarmaMember]
        protected Single SingleField;
        [KarmaMember]
        private String StringField;
        [KarmaMember]
        public Byte ByteField;
        [KarmaMember]
        protected SByte SByteField;
        [KarmaMember]
        private Guid GuidField;

        [KarmaMember]
        public Int16 Int16Property
        { get; private set; }
        [KarmaMember]
        public Int32 Int32Property
        { get; private set; }
        [KarmaMember]
        public Int64 Int64Property
        { get; private set; }
        [KarmaMember]
        public UInt16 UInt16Property
        { get; private set; }
        [KarmaMember]
        public UInt32 UInt32Property
        { get; private set; }
        [KarmaMember]
        public UInt64 UInt64Property
        { get; private set; }
        [KarmaMember]
        public Double DoubleProperty
        { get; private set; }
        [KarmaMember]
        public Single SingleProperty
        { get; private set; }
        [KarmaMember]
        public Byte ByteProperty
        { get; private set; }
        [KarmaMember]
        public SByte SByteProperty
        { get; private set; }
        [KarmaMember]
        public String StringProperty
        { get; private set; }
        [KarmaMember]
        public Guid GuidProperty
        { get; private set; }

        [KarmaMember]
        public TargetAbstractClass[] ArrayWithClassInheritedFromAbstractProperty
        { get; set; }
        [KarmaMember]
        public List<TargetAbstractClass> ListWithClassInheritedFromAbstractProperty
        { get; set; }
        [KarmaMember]
        public TargetAbstractClass TargetAbstractClassProperty
        { get; set; }
        [KarmaMember]
        public object[] ArrayOfObjectsProperty
        { get; set; }

        public void FillValues(int depth)
        {
            depth++;

            Int16Field = Int16.MaxValue;
            Int32Field = Int32.MaxValue;
            Int64Field = Int64.MaxValue;
            UInt16Field = UInt16.MaxValue;
            UInt32Field = UInt32.MaxValue;
            UInt64Field = UInt64.MaxValue;
            DoubleField = Double.MaxValue;
            SingleField = Single.MaxValue;
            StringField = "String_Строка_行";
            ByteField = Byte.MaxValue;
            SByteField = SByte.MaxValue;
            GuidField = new Guid(new byte[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31 });

            Int16Property = Int16.MinValue;
            Int32Property = Int32.MinValue;
            Int64Property = Int64.MinValue;
            UInt16Property = UInt16.MinValue;
            UInt32Property = UInt32.MinValue;
            UInt64Property = UInt64.MinValue;
            DoubleProperty = Double.MinValue;
            SingleProperty = Single.MinValue;
            StringProperty = "行_Строка_String";
            ByteProperty = Byte.MinValue;
            SByteProperty = SByte.MinValue;
            GuidProperty = new Guid(new byte[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 });

            Int32ArrayField = new int[5] { 1, 2, 3, 4, 5 };
            Int32ListField = new List<int>() { 6, 7, 8, 9, 10 };
            Int32ArrayProperty = new int[5] { 10, 20, 30, 40, 50 };
            Int32ListProperty = new List<int>() { 60, 70, 80, 90, 100 };
            ArrayWithClassInheritedFromAbstractProperty = new TargetAbstractClass[1] { new TargetClassInheritedFromAbstract(132541) };
            ListWithClassInheritedFromAbstractProperty = new List<TargetAbstractClass>() { new TargetClassInheritedFromAbstract(1111) };
            TargetAbstractClassProperty = new TargetClassInheritedFromAbstract(2222);
            ArrayOfObjectsProperty = new object[1] { new TargetClassInheritedFromAbstract(1515241) };

            Int32Int32DictionaryField = new Dictionary<int, int>();
            Int32Int32DictionaryField.Add(0, 1);
            Int32Int32DictionaryField.Add(1, 2);
            Int32Int32DictionaryField.Add(2, 3);

            Int32Int32DictionaryProperty = new Dictionary<int, int>();
            Int32Int32DictionaryProperty.Add(0, 1);
            Int32Int32DictionaryProperty.Add(1, 2);
            Int32Int32DictionaryProperty.Add(2, 3);

            if (depth < 3)
            {
                SubTargetClassField = new TargetClass();
                SubTargetClassField.FillValues(depth);

                SubTargetStructProperty = new TargetStruct();
                SubTargetStructProperty.FillValues(depth);
            }
            
            if (depth < 5)
            {
                SubTargetClassProperty = new TargetClass();
                SubTargetClassProperty.FillValues(depth);

                SubTargetStructField = new TargetStruct();
                SubTargetStructField.FillValues(depth);
            }
        }

        public void AssertEqualsTo(TargetClass target)
        {
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

            Assert.IsTrue(Utils.ValuesEquals(Int32ArrayField, target.Int32ArrayField));
            Assert.IsTrue(Utils.ValuesEquals(Int32ListField, target.Int32ListField));
            Assert.IsTrue(Utils.ValuesEquals(Int32Int32DictionaryField, target.Int32Int32DictionaryField));
            Assert.IsTrue(Utils.ValuesEquals(Int32ArrayProperty, target.Int32ArrayProperty));
            Assert.IsTrue(Utils.ValuesEquals(Int32ListProperty, target.Int32ListProperty));
            Assert.IsTrue(Utils.ValuesEquals(Int32Int32DictionaryProperty, target.Int32Int32DictionaryProperty));

            Assert.IsTrue(Utils.ValuesEquals(ArrayWithClassInheritedFromAbstractProperty, target.ArrayWithClassInheritedFromAbstractProperty));
            Assert.IsTrue(Utils.ValuesEquals(ListWithClassInheritedFromAbstractProperty, target.ListWithClassInheritedFromAbstractProperty));
            Assert.IsTrue(Utils.ValuesEquals(TargetAbstractClassProperty, target.TargetAbstractClassProperty));
            Assert.IsTrue(Utils.ValuesEquals(ArrayOfObjectsProperty, target.ArrayOfObjectsProperty));

            if (SubTargetClassField == null)
            { Assert.IsNull(target.SubTargetClassField); }
            else
            { SubTargetClassField.AssertEqualsTo(target.SubTargetClassField); }

            if (SubTargetClassProperty == null)
            { Assert.IsNull(target.SubTargetClassProperty); }
            else
            { SubTargetClassProperty.AssertEqualsTo(target.SubTargetClassProperty); }

            SubTargetStructField.AssertEqualsTo(target.SubTargetStructField);
            SubTargetStructProperty.AssertEqualsTo(target.SubTargetStructProperty);
        }
    }
}
