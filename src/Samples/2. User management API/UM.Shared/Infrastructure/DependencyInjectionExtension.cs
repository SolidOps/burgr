using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;

namespace SolidOps.UM.Shared.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddScopedFromExisting<INew, IExisting>(this IServiceCollection services) where INew : class
    {
        services.AddScoped(p => p.GetService<IExisting>() as INew);
    }

    public static void AddSingletonFromExisting<INew, IExisting>(this IServiceCollection services) where INew : class
    {
        services.AddSingleton(p => p.GetService<IExisting>() as INew);
    }

    public static void ReplaceTransientImplementation<TInterface, TNewImplementation>(this IServiceCollection services)
        where TInterface : class
        where TNewImplementation : class, TInterface
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(TInterface));
        services.Remove(sd);
        services.AddTransient<TInterface, TNewImplementation>();
    }

    public static void ReplaceScopedImplementation<TInterface, TNewImplementation>(this IServiceCollection services)
        where TInterface : class
        where TNewImplementation : class, TInterface
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(TInterface));
        services.Remove(sd);
        services.AddScoped<TInterface, TNewImplementation>();
    }

    public static void ReplaceSingletonImplementation<TInterface, TNewImplementation>(this IServiceCollection services)
        where TInterface : class
        where TNewImplementation : class, TInterface
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(TInterface));
        services.Remove(sd);
        services.AddSingleton<TInterface, TNewImplementation>();
    }

    public static void ReplaceSingletonImplementation<TInterface>(this IServiceCollection services, TInterface instance)
        where TInterface : class
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(TInterface));
        services.Remove(sd);
        services.AddSingleton<TInterface>(instance);
    }

    public static void Remove<TInterface>(this IServiceCollection services)
        where TInterface : class
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(TInterface));
        if (sd != null)
            services.Remove(sd);
    }

    public static void AddAsyncInitializerOnTop<T>(this IServiceCollection services)
        where T : class, IAsyncInitializer
    {
        var sds = services.Where(sd => sd.ServiceType == typeof(IAsyncInitializer)).ToList();
        foreach (var sd in sds)
        {
            services.Remove(sd);
        }
        services.AddAsyncInitializer<T>();
        foreach (var sd in sds)
        {
            services.AddAsyncInitializer(sd.ImplementationType);
        }
    }

    public static void AddAsyncInitializerBefore<TNew, TExisting>(this IServiceCollection services)
        where TNew : class, IAsyncInitializer
        where TExisting : class, IAsyncInitializer
    {
        var sds = services.Where(sd => sd.ServiceType == typeof(IAsyncInitializer)).ToList();
        foreach (var sd in sds)
        {
            services.Remove(sd);
        }
        bool found = false;
        foreach (var sd in sds)
        {
            if (sd.ImplementationType == typeof(TExisting))
            {
                services.AddAsyncInitializer<TNew>();
                found = true;
            }
            services.AddAsyncInitializer(sd.ImplementationType);
        }

        if (!found)
            services.AddAsyncInitializer<TNew>();
    }

    public static void AddAsyncInitializerAfter<TNew, TExisting>(this IServiceCollection services)
        where TNew : class, IAsyncInitializer
        where TExisting : class, IAsyncInitializer
    {
        var sds = services.Where(sd => sd.ServiceType == typeof(IAsyncInitializer)).ToList();
        foreach (var sd in sds)
        {
            services.Remove(sd);
        }

        bool found = false;
        foreach (var sd in sds)
        {
            services.AddAsyncInitializer(sd.ImplementationType);
            if (sd.ImplementationType == typeof(TExisting))
            {
                services.AddAsyncInitializer<TNew>();
                found = true;
            }
        }

        if (!found)
            services.AddAsyncInitializer<TNew>();
    }

    public static void RemoveAsyncInitializer<TExisting>(this IServiceCollection services)
        where TExisting : class, IAsyncInitializer
    {
        var sds = services.Where(sd => sd.ServiceType == typeof(IAsyncInitializer)).ToList();
        foreach (var sd in sds)
        {
            services.Remove(sd);
        }
        foreach (var sd in sds)
        {
            if (sd.ImplementationType != typeof(TExisting))
            {
                services.AddAsyncInitializer(sd.ImplementationType);
            }
        }
    }
}
