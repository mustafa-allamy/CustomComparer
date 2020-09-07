using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomComparer
{
    public class CustomComparer
    {
        public static List<PropertyCompareResult> Compare<T, T1>(T oldObject, T1 newObject)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo[] propertiesT1 = typeof(T1).GetProperties();
            List<PropertyCompareResult> result = new List<PropertyCompareResult>();

            foreach (PropertyInfo p in properties)
            {
                if (p.CustomAttributes.All(ca => ca.AttributeType != typeof(LogDataMemberAttribute)))
                {
                    continue;
                }
                if (propertiesT1.All(x => x.Name != p.Name))
                {
                    continue;
                }
                object oldValue = p.GetValue(oldObject), newValue = propertiesT1.FirstOrDefault(x => x.Name == p.Name)?.GetValue(newObject);

                if (!object.Equals(oldValue, newValue))
                {
                    result.Add(new PropertyCompareResult(p.Name, oldValue, newValue));
                }
            }

            return result;
        }
    }
    public class PropertyCompareResult
    {
        public string Name { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public PropertyCompareResult(string name, object oldValue, object newValue)
        {
            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
