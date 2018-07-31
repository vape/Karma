using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KarmaTests
{
    public static class Utils
    {
        public static bool DictionariesEquals<TKey, TValue>(Dictionary<TKey, TValue> d1, Dictionary<TKey, TValue> d2)
        {
            if (d1.Count != d2.Count)
            {
                return false;
            }

            foreach (var key in d1.Keys)
            {
                TValue d2Value;
                if (!d2.TryGetValue(key, out d2Value))
                {
                    return false;
                }

                TValue d1Value = d1[key];
                if (!ValuesEquals(d1Value, d2Value))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValuesEquals(object o1, object o2)
        {
            if (o1 == null && o2 == null)
            {
                return true;
            }
            else if (o1 == null && o2 != null)
            {
                return false;
            }
            else if (o1 != null && o2 == null)
            {
                return false;
            }

            if (o1.GetType() != o2.GetType())
            {
                throw new System.Exception("Different types.");
            }

            var type = o1.GetType();
            if (type.GetInterface(nameof(IEnumerable)) != null)
            {
                Type enumerableType;
                Type elementType;

                if (type.IsArray)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = type.GenericTypeArguments[0];
                }

                enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

                if (enumerableType.IsAssignableFrom(type))
                {
                    var method = GetSequenceEqualMethod().MakeGenericMethod(elementType);
                    return (bool)method.Invoke(null, new object[2] { o1, o2 });
                }
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var keyType = type.GenericTypeArguments[0];
                var valueType = type.GenericTypeArguments[1];
                return (bool)typeof(Utils).GetMethod(nameof(DictionariesEquals)).MakeGenericMethod(keyType, valueType).Invoke(null, new object[] { o1, o2 });
            }

            return o1.Equals(o2);
        }

        private static MethodInfo GetSequenceEqualMethod()
        {
            return typeof(Enumerable).GetMethods().Where((m) => m.Name == nameof(Enumerable.SequenceEqual)).First((m) => m.GetParameters().Length == 2);
        }
    }
}
