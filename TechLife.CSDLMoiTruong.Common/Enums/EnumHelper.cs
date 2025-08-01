using Microsoft.AspNetCore.Mvc.Rendering;
using TechLife.CSDLMoiTruong.Common.Enums;

public static class EnumHelper
{
    public static string GetStringValue(Enum value)
    {
        var type = value.GetType();
        var field = type.GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(StringValueAttribute), false)
                         .FirstOrDefault() as StringValueAttribute;
        return attr?.Value ?? value.ToString();
    }

    // Overload hỗ trợ int rõ ràng (fix CS1503)
    public static string GetStringValue<TEnum>(int value) where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
            return string.Empty;

        var enumValue = (Enum)Enum.ToObject(typeof(TEnum), value);
        return GetStringValue(enumValue);
    }
    public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
    {
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new Exception("T must be an Enumeration type.");
        }

        return (T)Enum.ToObject(enumType, intValue);
    }
    public static T GetEnumValue<T>(string str) where T : struct, IConvertible
    {
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new Exception("T must be an Enumeration type.");
        }
        return Enum.TryParse(str, true, out T val) ? val : default;
    }


    public static List<SelectListItem> GetSelectListFromEnum<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem
        {
            Text = GetStringValue(e),
            Value = (Convert.ToInt32(e)).ToString()
        }).ToList();
    }
}