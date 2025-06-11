using System.Globalization;

namespace SolidOps.UM.Shared.Infrastructure.Queries;

public static class ConversionHelper
{
    public static string ConvertToPascalCase(string text)
    {
        if (text == null)
            return null;

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                returnedString += arr[i].Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + arr[i].Substring(1);
            }
            return returnedString;
        }
        else
            return string.Empty;
    }

}
