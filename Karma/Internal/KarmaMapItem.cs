using System;
using System.Reflection;

namespace Karma.Internal
{
    internal class KarmaMapItem
    {
        public string Name;
        public Type MemberType;
        public KarmaMapItemType ItemType;
        public FieldInfo FieldInfo;
        public PropertyInfo PropertyInfo;

        public void SetValue(object obj, object value)
        {
            switch (ItemType)
            {
                case KarmaMapItemType.Field:
                    FieldInfo.SetValue(obj, value);
                    break;
                case KarmaMapItemType.Property:
                    PropertyInfo.SetValue(obj, value);
                    break;
            }
        }

        public object GetValue(object obj)
        {
            switch (ItemType)
            {
                case KarmaMapItemType.Field:
                    return FieldInfo.GetValue(obj);
                case KarmaMapItemType.Property:
                    return PropertyInfo.GetValue(obj);
            }

            return null;
        }
    }
}
