using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dOSCEngine.Engine;


[JsonConverter(typeof(StringEnumConverter))]
public enum EntityPropertyEnum
{
    Name,
    DisplayName,
    DelayTime,
    DelayTimeUnits,
    DecimalPlaceCount,
    ShowPercent,
    ShowNumbersOnly,
    Description,
    Type,
    PortTypeOverride,
    IsOverrideSource,
    Mode,
    CSSClass,
    HasError,
    PortCount,
    HideLabels,
    Option,
    OSCAddress,
    IsAvatarParameter,
    Max,
    NoMax,
    Min,
    NoMin,
    Power,
    Category,
    Icon,
    TextIcon,
    RequiresVisualRefresh,
    History,
    MaxQueue,
    ConstantValue,
    AllowedTypes,
    WriteAsFloat,
    SendChatMessageImmediately,
    SendChatMessageWithSound,
    Amplitude,
    Frequency,
    Note,
    NoteColor,
}