﻿using System.Linq.Expressions;
using System.Reflection;

namespace SolidOps.UM.Shared.Infrastructure;

public static class QueryHelper
{
    private static readonly MethodInfo OrderByMethod =
        typeof(Queryable).GetMethods().Single(method =>
        method.Name == "OrderBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo OrderByDescendingMethod =
        typeof(Queryable).GetMethods().Single(method =>
        method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

    private static readonly MethodInfo ThenByMethod =
        typeof(Queryable).GetMethods().Single(method =>
        method.Name == "ThenBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo ThenByDescendingMethod =
        typeof(Queryable).GetMethods().Single(method =>
        method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

    public static bool PropertyExists<T>(this IQueryable<T> source, string propertyName)
    {
        return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.Instance) != null;
    }

    public static IQueryable<T> OrderByProperty<T>(
       this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return source;
        }
        ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        MethodInfo genericMethod =
          OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> OrderByPropertyDescending<T>(
        this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return source;
        }
        ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        MethodInfo genericMethod =
          OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> ThenByProperty<T>(
       this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return source;
        }
        ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        MethodInfo genericMethod =
          ThenByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> ThenByPropertyDescending<T>(
        this IQueryable<T> source, string propertyName)
    {
        if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
            BindingFlags.Public | BindingFlags.Instance) == null)
        {
            return source;
        }
        ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
        MethodInfo genericMethod =
          ThenByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        object ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }
}
