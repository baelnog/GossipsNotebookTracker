using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ChecklistTracker.CoreUtils;

public static class EnumExtensions
{

    private static DoubleConcurrentDictionary<Type, string, Enum> EnumsByJsonString { get; } = new DoubleConcurrentDictionary<Type, string, Enum>();

    private static ConcurrentDictionary<Enum, string?> EnumMemberNames { get; } = new ConcurrentDictionary<Enum, string?>();

    public static Enum ToEnumByMemberName<T>(this string enumMemberName) where T : struct, Enum
    {
        return EnumsByJsonString
            .GetOrNew(typeof(T))
            .GetOrAdd(enumMemberName, enumMemberName =>
            {
                return Enum.GetValues<T>()
                    .Where(eValue => eValue.GetEnumMemberName() == enumMemberName)
                    .First();

            });
    }

    public static string? GetEnumMemberName(this Enum enumValue)
    {
        return EnumMemberNames.GetOrAdd(enumValue, value =>
        {
            var memberInfo = value.GetType().GetMember(value.ToString()).First();
            var attribute = memberInfo.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value;
        });
    }
}
