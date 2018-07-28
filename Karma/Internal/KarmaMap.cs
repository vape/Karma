using System;
using System.Collections.Generic;
using System.Reflection;

namespace Karma.Internal
{
    internal class KarmaMap
    {
        public IEnumerable<KarmaMapItem> Items
        { get { return map.Values; } }

        private Type targetType;
        private Dictionary<string, KarmaMapItem> map;

        public KarmaMap(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            this.targetType = targetType;
            CreateMap();
        }

        public List<Tuple<KarmaMapItem, object>> MapToValues(object obj, Predicate<KarmaMapItem> itemPredicate = null, Predicate<object> valuePredicate = null)
        {
            var result = new List<Tuple<KarmaMapItem, object>>(map.Count);

            foreach (var item in map.Values)
            {
                if (itemPredicate == null || itemPredicate(item))
                {
                    var value = item.GetValue(obj);
                    if (valuePredicate == null || valuePredicate(value))
                    {
                        result.Add(new Tuple<KarmaMapItem, object>(item, value));
                    }
                }
            }

            return result;
        }

        public bool TryGetItem(string name, out KarmaMapItem item)
        {
            return map.TryGetValue(name, out item);
        }

        private void CreateMap()
        {
            map = new Dictionary<string, KarmaMapItem>();

            foreach (var field in targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = field.GetCustomAttribute<KarmaMemberAttribute>();
                if (attribute != null)
                {
                    var name = attribute.CustomName ?? field.Name;
                    map.Add(name, new KarmaMapItem()
                    {
                        FieldInfo = field,
                        Name = name,
                        ItemType = KarmaMapItemType.Field,
                        MemberType = field.FieldType
                    });
                }
            }

            foreach (var property in targetType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = property.GetCustomAttribute<KarmaMemberAttribute>();
                if (attribute != null)
                {
                    var name = attribute.CustomName ?? property.Name;
                    map.Add(name, new KarmaMapItem()
                    {
                        PropertyInfo = property,
                        Name = name,
                        ItemType = KarmaMapItemType.Property,
                        MemberType = property.PropertyType
                    });
                }
            }
        }
    }
}
