using System.Globalization;

namespace MauiToDo.Extension
{
    public static class GeneralExtensions
    {
        public static string UppercaseFirst(string text)
        {
            if (string.IsNullOrEmpty(text)) 
                return string.Empty;
            char[] stringToCharArray = text.ToCharArray();
            stringToCharArray[0] = char.ToUpper(stringToCharArray[0]);
            return new string(stringToCharArray);
        }
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
    }
}
