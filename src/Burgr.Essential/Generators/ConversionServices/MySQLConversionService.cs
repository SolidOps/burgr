using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using System.Globalization;

namespace SolidOps.Burgr.Essential.Generators.ConversionServices;

public class MySQLConversionService : BaseConversionService
{
    public MySQLConversionService() : base("MySQL")
    {

    }
    public override string SimplePropertyType(ModelDescriptor model, bool preventList)
    {
        Type simpleType = GetSimpleType(model.Get("SimpleType"));
        return ConvertToMySQLPropertyType(simpleType, model.Get<int?>("FieldSize"), model.Is("MaxSize"), false, model.Is("Null"));
    }

    public override string EnumPropertyType(ModelDescriptor model, string suffix, bool preventList)
    {
        if (model.Is("Null"))
            return "int NULL";
        return "int NOT NULL";
    }

    public override string ModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        return ConvertToMySQLPropertyType(model.Get("IdentityKeysType") ?? GeneratorOptions.IdentityKeysType, false, model.Is("List"));
    }

    public override string ReferencedModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        return ConvertToMySQLPropertyType(model.Get("IdentityKeysType") ?? GeneratorOptions.IdentityKeysType, false, model.Is("Null"));
    }

    public override string ReplaceIdentityType(string text, string type)
    {
        var result = text.Replace(Tags.PrimaryIdentityKeyType, ConvertToMySQLPropertyType(type, true));
        result = result.Replace(Tags.IdentityKeyType, ConvertToMySQLPropertyType(type));

        return result;
    }

    public static string ConvertToMySQLPropertyType(Type simpleType, int? fieldSize, bool maxSize, bool isUnique, bool isNULL)
    {
        Type nullable = Nullable.GetUnderlyingType(simpleType);
        if (nullable != null)
        {
            return ConvertToMySQLPropertyType(nullable, fieldSize, maxSize, isUnique, isNULL);
        }

        string sqlType;
        if (simpleType == typeof(string))
        {
            if (maxSize)
            {
                sqlType = "text";
            }
            else
            {
                if (!fieldSize.HasValue || fieldSize.Value <= 0)
                {
                    fieldSize = Constants.StandardMaxSize;
                }

                sqlType = string.Format(CultureInfo.InvariantCulture, "varchar({0})", fieldSize);
            }
        }
        else
        {
            sqlType = simpleType == typeof(int)
                ? "int"
                : simpleType == typeof(uint)
                                ? "int"
                                : simpleType == typeof(long)
                                                ? "int"
                                                : simpleType == typeof(ulong)
                                                                ? "int"
                                                                : simpleType == typeof(double)
                                                                                ? "decimal(20,6)"
                                                                                : simpleType == typeof(Guid)
                                                                                                ? "varchar(50)"
                                                                                                : simpleType == typeof(decimal)
                                                                                                                ? "decimal(20,6)"
                                                                                                                : simpleType == typeof(DateTime)
                                                                                                                                ? "datetime"
                                                                                                                                : simpleType == typeof(bool)
                                                                                                                                                ? "tinyint(1)"
                                                                                                                                                : simpleType == typeof(byte[])
                                                                                                                                                                ? "varbinary(5000)"
                                                                                                                                                                : simpleType.IsEnum
                                                                                                                                                                            ? "int"
                                                                                                                                                                            : throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Could not match property type with mysql type : {0}", simpleType.Name));
        }

        if (!isNULL)
        {
            sqlType += " NOT";
        }

        sqlType += " NULL";

        return sqlType;
    }

    public static string ConvertToMySQLPropertyType(string type, bool isPrimary = false, bool? isNull = null)
    {
        if (type == null) throw new ArgumentNullException("type");

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

    public static string ConvertToSQLPropertyType(Type simpleType, int? fieldSize, bool isUnique)
    {
        Type nullable = Nullable.GetUnderlyingType(simpleType);
        if (nullable != null)
        {
            return ConvertToSQLPropertyType(nullable, fieldSize, isUnique);
        }

        string sqlType;
        if (simpleType == typeof(string))
        {
            if (!fieldSize.HasValue && isUnique)
            {
                fieldSize = 50; //### default value
            }

            sqlType = fieldSize != null ? string.Format(CultureInfo.InvariantCulture, "[nvarchar]({0})", fieldSize) : "[nvarchar](MAX)";
        }
        else
        {
            sqlType = simpleType == typeof(int)
                ? "int"
                : simpleType == typeof(double)
                                ? "float"
                                : simpleType == typeof(Guid)
                                                ? "uniqueidentifier"
                                                : simpleType == typeof(decimal)
                                                                ? "[decimal](9,2)"
                                                                : simpleType == typeof(DateTime)
                                                                                ? "datetime"
                                                                                : simpleType == typeof(bool)
                                                                                                ? "bit"
                                                                                                : simpleType == typeof(byte[])
                                                                                                                ? "[varbinary](MAX)"
                                                                                                                : simpleType.IsEnum
                                                                                                                            ? "int"
                                                                                                                            : throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Could not match property type with sql type : {0}", simpleType.Name));
        }

        return sqlType;
    }
}

