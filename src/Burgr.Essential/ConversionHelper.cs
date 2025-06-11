using SolidOps.SubZero;
using System.Globalization;

namespace SolidOps.Burgr.Essential;

public static class ConversionHelper
{
    public static string ConvertToPascalCase(string text)
    {
        return TextHelper.ConvertToPascalCase(text);
    }

    public static string ConvertToCamelCase(string text)
    {
        if (text == null)
        {
            return null;
        }

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                returnedString += arr[i][..1].ToUpper(CultureInfo.InvariantCulture) + arr[i][1..];
            }

            string first = returnedString[..1];
            if (first.ToLower() != first)
            {
                returnedString = first.ToLower() + returnedString[1..];
            }
            return returnedString;
        }
        else
        {
            return string.Empty;
        }
    }
}

public enum ReturnType
{
    Void = 0,
    Simple = 1,
    Model = 2,
    ModelList = 3,
    Identity = 4
}
