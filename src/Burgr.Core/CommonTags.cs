using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidOps.Burgr.Core
{
    public static class CommonTags
    {
        public const string Slug = "Slug";
        public const string IdentityKeyType = "_IDENTITY_KEY_TYPE_";
        public const string Namespace = "MetaCorp.Template";
        public const string ShortModule = "Template";
        public const string PrimaryIdentityKeyType = "_PRIMARY_IDENTITY_KEY_TYPE_";

        public static string ConvertToJSPropertyType(string type)
        {
            return type is "Guid" or "string" ? "string" : "number";
        }

        public static string ConvertToMySQLPropertyType(string type, bool isPrimary = false, bool? isNull = null)
        {
            string sqlType = type is "Guid" or "string" ? "varchar(50)" : "INT";
            if (isPrimary)
            {
                sqlType += " NOT NULL";
                if (type is not "Guid" and not "string")
                {
                    sqlType += " AUTO_INCREMENT";
                }
            }
            else
            {
                if (isNull.HasValue)
                {
                    if (!isNull.Value)
                    {
                        sqlType += " NOT";
                    }
                    sqlType += " NULL";
                }
            }

            return sqlType;
        }

        public static string ConvertToJSLibrary(string moduleName)
        {
            return moduleName.ToLower().Replace(".", "-");
        }
    }
}
