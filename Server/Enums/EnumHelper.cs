using System.ComponentModel;

namespace OnlineStore.Server.Enums;

public static class EnumHelper
{
    public static string GetDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}