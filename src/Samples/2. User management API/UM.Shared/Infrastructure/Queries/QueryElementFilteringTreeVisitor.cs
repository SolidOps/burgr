using SolidOps.UM.Shared.Domain.Entities;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SolidOps.UM.Shared.Infrastructure.Queries;

public class QueryElementFilteringTreeVisitor<T, TObject>
    where TObject : IEntityOfDomain<T>
    where T : struct
{
    public ParameterExpression ParameterExpression { get; } = Expression.Parameter(typeof(TObject), "obj");

    public QueryElementFilteringTreeVisitor()
    {
    }

    public Expression Visit(BaseQueryElement criteria)
    {
        if (criteria is AllQueryElement allCriteria)
        {
            return VisitAll(allCriteria);
        }
        else if (criteria is AnyQueryElement anyCriteria)
        {
            return VisitAny(anyCriteria);
        }
        else if (criteria is SingleQueryElement singleCriteria)
        {
            return VisitSingle(singleCriteria);
        }
        throw new NotImplementedException();
    }

    private Expression VisitAll(AllQueryElement allCriteria)
    {
        Expression expression = null;
        foreach (var c in allCriteria.Elements)
        {
            if (expression == null)
                expression = Visit(c);
            else
                expression = Expression.And(expression, Visit(c));
        }
        return expression;
    }

    private Expression VisitAny(AnyQueryElement anyCriteria)
    {
        Expression expression = null;
        foreach (var c in anyCriteria.Elements)
        {
            if (expression == null)
                expression = Visit(c);
            else
                expression = Expression.Or(expression, Visit(c));
        }
        return expression;
    }

    private Expression VisitSingle(SingleQueryElement singleCriteria)
    {
        var fields = GetFields(typeof(TObject), singleCriteria.Member);

        var basicOperator = singleCriteria.CriteriaOperation;
        var value = singleCriteria.Value;

        MemberExpression left = null;
        int remaining = fields.Count;
        foreach (var member in fields)
        {
            remaining--;
            var field = ConversionHelper.ConvertToPascalCase(member.field);
            var propertyInfo = member.type.GetProperty(field);
            if (remaining == 0 && propertyInfo.PropertyType.GetInterface("IEFEntity`2") != null)
            {
                propertyInfo = member.type.GetProperty(field + "Id");
            }
            if (left == null)
            {
                left = Expression.Property(ParameterExpression, propertyInfo);
            }
            else
                left = Expression.Property(left, propertyInfo);
        }
        var lastType = fields.Last().type;
        Expression right;
        if (value != null) // value can be null
        {
            var type = value.GetType();
            if (!type.IsEnum)
            {
                Type NonNullType = Nullable.GetUnderlyingType(lastType);

                if (NonNullType != null)
                {
                    right = Expression.Constant(value, lastType);
                }
                else
                {
                    right = Expression.Constant(value);
                }
            }
            else if (Nullable.GetUnderlyingType(lastType) != null)
            {
                int? value2 = (int)value;
                right = Expression.Constant(value2, typeof(int?));
            }
            else
            {
                right = Expression.Constant((int)value);
            }
        }
        else
        {
            right = Expression.Constant(null);
        }

        Expression resultExpression;
        switch (basicOperator)
        {
            case CriteriaOperation.Equal:
                {
                    // expression for "value = false" is translated to "Not value = true" which create problems when there are several parameters
                    if (left.Member is PropertyInfo leftInfo
                        && leftInfo.PropertyType == typeof(bool)
                        && right is ConstantExpression constant
                        && constant.Value.Equals(false))
                    {
                        resultExpression = Expression.Equal(Expression.Convert(left, typeof(int)), Expression.Constant(0));
                    }
                    else if (value != null && left.Member is PropertyInfo leftInfo2
                        && Nullable.GetUnderlyingType(leftInfo2.PropertyType) != null)
                    {
                        resultExpression = Expression.Equal(Expression.Convert(left, Nullable.GetUnderlyingType(leftInfo2.PropertyType)), right);
                    }
                    // handle enums
                    else if (left.Member is PropertyInfo leftEnumInfo1
                        && leftEnumInfo1.PropertyType.IsEnum)
                    {
                        resultExpression = Expression.Equal(Expression.Convert(left, typeof(int)), right);
                    }
                    else
                    {
                        resultExpression = Expression.Equal(left, right);
                    }
                }
                break;
            case CriteriaOperation.Different:
                if (left.Member is PropertyInfo leftEnumInfo2
                        && leftEnumInfo2.PropertyType.IsEnum)
                {
                    resultExpression = Expression.NotEqual(Expression.Convert(left, typeof(int)), right);
                }
                else
                {
                    resultExpression = Expression.NotEqual(left, right);
                }
                break;
            case CriteriaOperation.GreaterThan:
                if (value != null && left.Member is PropertyInfo leftGT
                        && Nullable.GetUnderlyingType(leftGT.PropertyType) != null)
                {
                    resultExpression = Expression.GreaterThan(Expression.Convert(left, Nullable.GetUnderlyingType(leftGT.PropertyType)), right);
                }
                else
                {
                    resultExpression = Expression.GreaterThan(left, right);
                }
                break;
            case CriteriaOperation.GreaterThanOrEqual:
                if (value != null && left.Member is PropertyInfo leftGTO
                       && Nullable.GetUnderlyingType(leftGTO.PropertyType) != null)
                {
                    resultExpression = Expression.GreaterThanOrEqual(Expression.Convert(left, Nullable.GetUnderlyingType(leftGTO.PropertyType)), right);
                }
                else
                {
                    resultExpression = Expression.GreaterThanOrEqual(left, right);
                }
                break;
            case CriteriaOperation.LesserThan:
                if (value != null && left.Member is PropertyInfo leftLT
                       && Nullable.GetUnderlyingType(leftLT.PropertyType) != null)
                {
                    resultExpression = Expression.LessThan(Expression.Convert(left, Nullable.GetUnderlyingType(leftLT.PropertyType)), right);
                }
                else
                {
                    resultExpression = Expression.LessThan(left, right);
                }
                break;
            case CriteriaOperation.LesserThanOrEqual:
                if (value != null && left.Member is PropertyInfo leftLTO
                       && Nullable.GetUnderlyingType(leftLTO.PropertyType) != null)
                {
                    resultExpression = Expression.LessThanOrEqual(Expression.Convert(left, Nullable.GetUnderlyingType(leftLTO.PropertyType)), right);
                }
                else
                {

                    resultExpression = Expression.LessThanOrEqual(left, right);
                }
                break;
            case CriteriaOperation.Like:
                {
                    string checkMethodName;
                    var stringValue = value as string;
                    if (stringValue.StartsWith(FilterHelper.JOKERCHAR) && stringValue.EndsWith(FilterHelper.JOKERCHAR))
                    {
                        checkMethodName = "Contains";
                        right = Expression.Constant(stringValue.Substring(1, stringValue.Length - 2));
                    }
                    else if (stringValue.StartsWith(FilterHelper.JOKERCHAR))
                    {
                        checkMethodName = "EndsWith";
                        right = Expression.Constant(stringValue.Substring(1));
                    }
                    else if (stringValue.EndsWith(FilterHelper.JOKERCHAR))
                    {
                        checkMethodName = "StartsWith";
                        right = Expression.Constant(stringValue.Substring(0, stringValue.Length - 1));
                    }
                    else
                    {
                        checkMethodName = "Equals";
                    }

                    // Expression to do is : left => left.ToLower().Contains(right.ToLower())

                    var toLowerMethodInfo = typeof(string).GetMethod("ToLower", new Type[] { });
                    var toLowerLeftCallExpression = Expression.Call(left, toLowerMethodInfo);
                    var toLowerRightCallExpression = Expression.Call(right, toLowerMethodInfo);
                    var checkMethodInfo = typeof(string).GetMethod(checkMethodName, new Type[] { typeof(string) });
                    var methodCallExpression = Expression.Call(toLowerLeftCallExpression, checkMethodInfo, toLowerRightCallExpression);

                    resultExpression = methodCallExpression as Expression;
                }
                break;
            case CriteriaOperation.In:
                {
                    var containsMethodInfo = right.Type.GetMethod("Contains");
                    if (left.Type == typeof(Guid))
                    {
                        // Expression to do is : left => right.Contains(left.ToString().ToLower())

                        var toStringMethodInfo = left.Type.GetMethod("ToString", new Type[] { });
                        var toStringLeftCallExpression = Expression.Call(left, toStringMethodInfo);

                        //var toLowerMethodInfo = typeof(string).GetMethod("ToLower", new Type[] { });
                        //var toLowerLeftCallExpression = Expression.Call(toStringLeftCallExpression, toLowerMethodInfo);
                        // var methodCallExpression = Expression.Call(right, containsMethodInfo, toLowerLeftCallExpression);

                        var methodCallExpression = Expression.Call(right, containsMethodInfo, toStringLeftCallExpression);

                        resultExpression = methodCallExpression as Expression;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                break;
            default:
                throw new NotImplementedException();
        }
        return resultExpression;
    }

    private List<(string field, Type type)> GetFields(Type type, string fieldName)
    {
        var parts = fieldName.Split(".");
        if (parts.Count() == 1)
        {
            return new List<(string field, Type type)>() { (fieldName, type) };
        }

        var current = parts[0];
        parts = parts.Skip(1).ToArray();
        var res = GetFields(type.GetProperty(current).PropertyType, string.Join(".", parts));
        if (res != null)
        {
            res = res.Prepend((current, type)).ToList();
            return res;
        }
        return null;

    }
}

public delegate Expression SpecialOperatorHandler(ParameterExpression parameter, string value);
