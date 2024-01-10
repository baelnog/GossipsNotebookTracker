using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    internal enum OpenKakarikoType
    {
        [EnumMember(Value = "open")]
        Open,
        [EnumMember(Value = "zelda")]
        AutoOpenOnZeldasLetter,
        [EnumMember(Value = "closed")]
        ClosedGate,
    }
}
