using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ShopsanityPriceType
    {
        [EnumMember(Value = "random")]
        Random,
        [EnumMember(Value = "random_starting")]
        Random99,
        [EnumMember(Value = "random_adult")]
        Random200,
        [EnumMember(Value = "random_giant")]
        Random500,
        [EnumMember(Value = "random_tycoon")]
        Random999,
        [EnumMember(Value = "affordable")]
        Affordable,
    }
}
