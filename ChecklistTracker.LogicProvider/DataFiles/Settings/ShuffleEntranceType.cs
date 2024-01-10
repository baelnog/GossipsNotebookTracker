using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    internal enum ShuffleEntranceType
    {
        [EnumMember(Value = "off")]
        Off = 0,
        [EnumMember(Value = "simple")]
        Simple = 1,
        [EnumMember(Value = "special")]
        Special = 2, // Not a real setting
        [EnumMember(Value = "all")]
        All = Simple | Special,
    }
}
