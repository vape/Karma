using Karma.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Karma
{
    public class KarmaSerializer : IDisposable
    {
        public const int Magic = 0xFADE;
        public const int Version = 2;

        private static Dictionary<Type, Action<KarmaWriter, object>> typeWriters = new Dictionary<Type, Action<KarmaWriter, object>>()
        {
            { typeof(Int16),  (writer, obj) => writer.WriteInt16((Int16)obj)   },
            { typeof(Int32),  (writer, obj) => writer.WriteInt32((Int32)obj)   },
            { typeof(Int64),  (writer, obj) => writer.WriteInt64((Int64)obj)   },
            { typeof(UInt16), (writer, obj) => writer.WriteUInt16((UInt16)obj) },
            { typeof(UInt32), (writer, obj) => writer.WriteUInt32((UInt32)obj) },
            { typeof(UInt64), (writer, obj) => writer.WriteUInt64((UInt64)obj) },
            { typeof(Double), (writer, obj) => writer.WriteDouble((Double)obj) },
            { typeof(Single), (writer, obj) => writer.WriteSingle((Single)obj) },
            { typeof(Byte),   (writer, obj) => writer.WriteByte((Byte)obj)     },
            { typeof(SByte),  (writer, obj) => writer.WriteSByte((SByte)obj)   },
            { typeof(String), (writer, obj) => writer.WriteString((String)obj) },
            { typeof(Guid),   (writer, obj) => writer.WriteGuid((Guid)obj)     },
        };

        private KarmaWriter writer;
        private Stream stream;
        private KarmaOptions options;

        private Dictionary<Type, KarmaMap> typeMapCache = new Dictionary<Type, KarmaMap>();

        public KarmaSerializer(Stream stream, KarmaOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Can not write to stream.");
            }

            this.writer = new KarmaWriter(stream);
            this.stream = stream;
            this.options = options ?? new KarmaOptions();

            WriteHeader();
        }

        private void WriteHeader()
        {
            writer.WriteInt32(Magic);
            writer.WriteInt32(Version);
        }

        public void Write(object obj, bool serializeTypeName = false)
        {
            var type = obj.GetType();

            writer.WriteBool(serializeTypeName);
            if (serializeTypeName)
            {
                writer.WriteString(type.FullName);
            }

            KarmaMap typeMap;
            if (!typeMapCache.TryGetValue(type, out typeMap))
            {
                typeMap = new KarmaMap(type);
                typeMapCache.Add(type, typeMap);
            }

            var itemValueMap = typeMap.MapToValues(
                obj, 
                null, 
                (value) =>
                {
                    if (!options.SerializeDefaultValues)
                    {
                        return value != null;
                    }

                    return true;
                }
            );

            writer.WriteInt32(itemValueMap.Count);
            foreach (var item in itemValueMap)
            {
                writer.WriteString(item.Item1.Name);

                if (item.Item2 == null)
                {
                    writer.WriteBool(true);
                    continue;
                }
                else
                {
                    writer.WriteBool(false);
                    WriteValue(item.Item2, item.Item1.MemberType.IsAbstract);
                }
            }
        }

        private void WriteValue(object obj, bool serializeTypeName = false)
        {
            var type = obj.GetType();
            Action<KarmaWriter, object> typeWriter;
            if (typeWriters.TryGetValue(type, out typeWriter))
            {
                typeWriter(writer, obj);
                return;
            }

            if (type.IsArray)
            {
                WriteArray((Array)obj);
                return;
            }
            
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    WriteList((IList)obj);
                    return;
                }

                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    WriteDictionary((IDictionary)obj);
                    return;
                }
            }

            Write(obj, serializeTypeName);
        }

        private void WriteArray(Array array)
        {
            var elementType = array.GetType().GetElementType();
            var serializeType = elementType.IsAbstract || elementType == typeof(System.Object);

            writer.WriteInt32(array.Length);
            for (int i = 0; i < array.Length; ++i)
            {
                WriteValue(array.GetValue(i), serializeType);
            }
        }

        private void WriteList(IList list)
        {
            writer.WriteInt32(list.Count);
            for (int i = 0; i < list.Count; ++i)
            {
                WriteValue(list[i], true);
            }
        }

        private void WriteDictionary(IDictionary dictionary)
        {
            writer.WriteInt32(dictionary.Count);
            foreach (var key in dictionary.Keys)
            {
                WriteValue(key);
                WriteValue(dictionary[key]);
            }
        }

        public void Dispose()
        {
            writer.Dispose();
        }
    }
}
