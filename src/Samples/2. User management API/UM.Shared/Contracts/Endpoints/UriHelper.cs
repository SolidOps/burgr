using SolidOps.SubZero;

namespace SolidOps.UM.Shared.Contracts.Endpoints;

public class UriHelper
{
    private static readonly Lazy<UriHelper> lazy =
    new Lazy<UriHelper>(() => new UriHelper());

    public static UriHelper Instance { get { return lazy.Value; } }

    public Dictionary<string, string> Aliases { get; set; }

    private UriHelper()
    {
        Aliases = new Dictionary<string, string>();
    }

    public void AddAlias(string key, string value)
    {
        if (!Aliases.ContainsKey(key))
        {
            Aliases.Add(key, value);
        }
    }

    public string GetUri<T>(string id, string subRoute, bool composed = false)
    {
        var dataType = typeof(T);
        var parts = dataType.FullName.Split(".").ToList();
        var name = parts.Last();
        var moduleName = parts[parts.IndexOf("API") - 1];
        var api = TextHelper.GenerateSlug(moduleName) + "/" + TextHelper.GenerateSlug(name.Replace("DTO", ""));
        if (Aliases.ContainsKey(api))
        {
            api = Aliases[api];
        }
        var uri = api; // "api/" + api;
        if (id != null)
        {
            if (composed)
            {
                uri += "/composed";
                uri += "/" + id.Replace("|", "%7C");
            }
            else
            {
                uri += "/" + id;
            }
        }
        if (subRoute != null)
        {
            uri += "/" + subRoute;
        }

        return uri;
    }

    public string MakeUri(string baseUri, params string[] queryParameters)
    {
        return baseUri;
    }

    #region Convert
    public static string Convert<T>(T input)
    {
        if (input is DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("s") + "Z";
        }
        else if(input is System.Double doubleValue)
        {
            return doubleValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (input is System.Single singleValue)
        {
            return singleValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (input is System.Decimal decimalValue)
        {
            return decimalValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            if(input != null)
                return input.ToString();
            return "";
        }
    }
    #endregion
}
