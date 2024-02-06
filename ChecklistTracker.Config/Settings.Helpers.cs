using ChecklistTracker.CoreUtils;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config
{
    public partial class Settings
    {
        public static async Task<Settings> ReadFromJson(string jsonFilePath)
        {
            return await TrackerConfig.ParseJson<Settings>(jsonFilePath).ConfigureAwait(false);
        }

        private static Lazy<IDictionary<string, Func<Settings, object?>>> SettingsByJsonName = new Lazy<IDictionary<string, Func<Settings, object?>>>(() =>
        {
            var dict = new Dictionary<string, Func<Settings, object?>>();

            foreach (var property in typeof(Settings).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var propertyNameAttribute = property.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
                if (propertyNameAttribute != null)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        dict[propertyNameAttribute.Name] = (Settings settings) =>
                        {
                            var value = property.GetValue(settings) as Enum;
                            return value?.GetEnumMemberName();
                        };
                    }
                    else
                    {
                        dict[propertyNameAttribute.Name] = (Settings settings) => property.GetValue(settings);
                    }
                }
            }

            return dict;
        });

        public bool ContainsKey(string key)
        {
            return SettingsByJsonName.Value.ContainsKey(key);
        }

        public bool IsEnabled(string key)
        {
            return SettingsByJsonName.Value[key].Invoke(this) is bool enabled && enabled;
        }

        public bool IsSettingEqual(string key, string value)
        {
            var setting = SettingsByJsonName.Value[key].Invoke(this);

            if (setting is Enum enumSetting)
            {
                return enumSetting.GetEnumMemberName() == value;
            }

            return setting?.ToString() == value;
        }

        public T GetSetting<T>(string key)
        {
            return (T) SettingsByJsonName.Value[key].Invoke(this);
        }

        public bool SettingHas(string key, string value)
        {
            var setting = SettingsByJsonName.Value[key].Invoke(this) as IEnumerable;
            var type = setting.GetType().GetGenericArguments()[0];
            if (type.IsEnum)
            {
                return setting
                    .Cast<Enum>()
                    .Any(eValue => eValue.GetEnumMemberName() == value);
            }
            return ((ISet<string>)setting).Contains(value);
        }

        private bool CheckSetContains<T>(object set, string value)
        {
            if (set is ISet<T> typedSet)
            {
                var enumVal = value.ToEnum<T>();
                return enumVal != null && typedSet.Contains(enumVal);
            }
            return false;
        }
    }
}
