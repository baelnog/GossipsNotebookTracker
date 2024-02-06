using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChecklistTracker.Config.SettingsTypes;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ShopsanityType
{
    [EnumMember(Value = "off")]
    Off,
    [EnumMember(Value = "0")]
    Items0 = 0,
    [EnumMember(Value = "1")]
    Items1 = 1,
    [EnumMember(Value = "2")]
    Items2 = 2,
    [EnumMember(Value = "3")]
    Items3 = 3,
    [EnumMember(Value = "4")]
    Items4 = 4,
    [EnumMember(Value = "random")]
    Random,
}
