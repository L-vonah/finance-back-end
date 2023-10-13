using System.ComponentModel;

namespace finance.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T category) where T : Enum
        {
            var fieldInfo = category.GetType().GetField(category.ToString());
            var attributes = fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes == null || attributes.Length == 0) return category.ToString();
            return attributes[0].Description;
        }
    }
}
