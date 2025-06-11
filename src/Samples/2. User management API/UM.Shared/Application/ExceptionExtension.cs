using System.Collections;
using System.Text;

namespace SolidOps.UM.Shared.Application;

public static class ExceptionExtension
{
    public static string GetExceptionMessage(this Exception exception)
    {
        StringBuilder stb = new StringBuilder();
        BuildErrorMessage(stb, exception, false);
        return stb.ToString();
    }

    public static string GetExceptionMessage(string type, string message, string stackTrace)
    {
        StringBuilder stb = new StringBuilder();
        BuildErrorMessage(stb, type, message, stackTrace, null, false);
        return stb.ToString();
    }

    private static void BuildErrorMessage(StringBuilder target, Exception exception, bool isInner)
    {
        if (exception != null)
        {
            BuildErrorMessage(target, exception.GetType().Name, exception.Message, exception.StackTrace, exception.Data, isInner);

            if (exception.InnerException != null)
                BuildErrorMessage(target, exception.InnerException, true);
            
        }
    }

    private static void BuildErrorMessage(StringBuilder target, string type, string message, string stackTrace, IDictionary data, bool isInner)
    {
        if (!isInner)
        {
            target.AppendFormat(@"
---------------------------------------------------
[{0}] {1} : 

{2}
", type, message, stackTrace);
        }
        else
        {
            target.AppendFormat(@"
*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
Inner => [{0}] {1} : 

{2}
", type, message, stackTrace);
        }

        if(data != null)
        {
            target.Append(@"
#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
");
            foreach(var key in data.Keys)
            {
                var value = data[key];
                target.AppendFormat(@"{0} | {1} |
", key.ToString(), value.ToString());
            }

            target.Append(@"#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
");
        }

    }
}
