using Karma.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Karma
{
    public class KarmaDeserializer : IDisposable
    {
        private static Dictionary<Type, Func<KarmaReader, object>> typeReaders = new Dictionary<Type, Func<KarmaReader, object>>()
        {
            { typeof(Int16),  (reader) => reader.ReadInt16()  },
            { typeof(Int32),  (reader) => reader.ReadInt32()  },
            { typeof(Int64),  (reader) => reader.ReadInt64()  },
            { typeof(UInt16), (reader) => reader.ReadUInt16() },
            { typeof(UInt32), (reader) => reader.ReadUInt32() },
            { typeof(UInt64), (reader) => reader.ReadUInt64() },
            { typeof(Double), (reader) => reader.ReadDouble() },
            { typeof(Single), (reader) => reader.ReadSingle() },
            { typeof(Byte),   (reader) => reader.ReadByte()   },
            { typeof(SByte),  (reader) => reader.ReadSByte()  },
            { typeof(String), (reader) => reader.ReadString() },
            { typeof(Guid),   (reader) => reader.ReadGuid()   }
        };

        private KarmaReader reader;
        private Stream stream;
        private KarmaOptions options;

        private bool nullValuesSerialized;

        private Dictionary<Type, KarmaMap> typeMapCache = new Dictionary<Type, KarmaMap>();

        public KarmaDeserializer(Stream stream, KarmaOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream can not be read.");
            }

            this.stream = stream;
            this.reader = new KarmaReader(stream);
            this.options = options ?? new KarmaOptions();

            ReadHeader();
        }

        private void ReadHeader()
        {
            var signature = reader.ReadInt32();
            if (signature != KarmaSerializer.Magic)
            {
                throw new InvalidDataException("Invalid signature.");
            }

            var dataVersion = reader.ReadInt32();
            if (dataVersion > KarmaSerializer.Version)
            {
                throw new InvalidDataException("Serialzied version is higher than reader's.");
            }

            nullValuesSerialized = reader.ReadBool();
        }

        public T Read<T>()
        {
            return (T)ReadObject(typeof(T));
        }

        public object ReadObject(Type type)
        {
            KarmaMap typeMap;
            if (!typeMapCache.TryGetValue(type, out typeMap))
            {
                typeMap = new KarmaMap(type);
                typeMapCache.Add(type, typeMap);
            }

            var instance = CreateTypeInstance(type);

            var itemsCount = reader.ReadInt32();
            for (int i = 0; i < itemsCount; ++i)
            {
                var itemName = reader.ReadString();
                KarmaMapItem item;
                if (!typeMap.TryGetItem(itemName, out item))
                {
                    throw new KarmaException($"Data stream contains unknown member: {itemName}");
                }

                if (nullValuesSerialized)
                {
                    var isNull = reader.ReadBool();
                    if (isNull)
                    {
                        item.SetValue(instance, null);
                        continue;
                    }
                }

                item.SetValue(instance, ReadValue(item.MemberType));
            }

            return instance;
        }

        private object ReadValue(Type type)
        {
            Func<KarmaReader, object> typeReader;
            if (typeReaders.TryGetValue(type, out typeReader))
            {
                return typeReader(reader);
            }

            if (type.IsArray)
            {
                return ReadArray(type);
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return ReadList(type);
                }

                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return ReadDictionary(type);
                }
            }

            return ReadObject(type);
        }

        private Array ReadArray(Type type)
        {
            var length = reader.ReadInt32();
            var elementType = type.GetElementType();
            var array = Array.CreateInstance(elementType, length);
            for (int i = 0; i < length; ++i)
            {
                array.SetValue(ReadValue(elementType), i);
            }

            return array;
        }

        private IList ReadList(Type type)
        {
            var length = reader.ReadInt32();
            var elementType = type.GenericTypeArguments[0];
            var list = (IList)CreateTypeInstance(type);
            for (int i = 0; i < length; ++i)
            {
                list.Add(ReadValue(elementType));
            }

            return list;
        }

        private IDictionary ReadDictionary(Type type)
        {
            var length = reader.ReadInt32();
            var keyType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];
            var dictionary = (IDictionary)CreateTypeInstance(type);
            for (int i = 0; i < length; ++i)
            {
                var key = ReadValue(keyType);
                var value = ReadValue(valueType);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private object CreateTypeInstance(Type type, params object[] args)
        {
            if (options.AllowCreatingObjectWithoutConstructor)
            {
                var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (constructors.Length == 0)
                {
                    goto reliable_solution;
                }

                var argTypes = new Type[args.Length];

                for (int i = 0; i < args.Length; ++i)
                {
                    if (args[i] != null)
                    {
                        argTypes[i] = args[i].GetType();
                    }
                }

                var matchedConstructors = new List<ConstructorInfo>(constructors.Length);
                for (int i = 0; i < constructors.Length; ++i)
                {
                    if (FilterConstructorParameters(constructors[i], argTypes))
                    {
                        goto reliable_solution;
                    }
                }

                return FormatterServices.GetUninitializedObject(type);
            }

reliable_solution:
            return Activator.CreateInstance(type, args);
        }

        private bool FilterConstructorParameters(ConstructorInfo info, Type[] parameterTypes)
        {
            var constructorParameters = info.GetParameters();
            if (constructorParameters.Length != parameterTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < parameterTypes.Length; ++i)
            {
                if (parameterTypes[i] != null && !Object.ReferenceEquals(constructorParameters[i].ParameterType, parameterTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            stream.Dispose();
            reader.Dispose();
        }
    }
}
