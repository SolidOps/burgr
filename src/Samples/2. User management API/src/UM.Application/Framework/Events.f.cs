using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SolidOps.UM.Application;

/// <summary>An event of the UM module, handled in process by every <see cref="IEventHandler{TEvent}"/> of it.</summary>
public interface IEvent
{
    /// <summary>What happened: <see cref="EventContext.Add"/>, <see cref="EventContext.Update"/>, <see cref="EventContext.Remove"/>, or a context of your own.</summary>
    string Context { get; }
}

/// <summary>The contexts of the events published by the generated use cases, once the change is saved.</summary>
public static class EventContext
{
    public const string Add = "add";
    public const string Update = "update";
    public const string Remove = "remove";
}

/// <summary>
/// Handles <typeparamref name="TEvent"/>. Every class of the application assembly
/// implementing it is registered by AddUMApplication(); a failing handler fails the
/// request, but the change that raised the event is already saved.
/// </summary>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

public interface IEventPublisher
{
    /// <summary>Runs the handlers of <paramref name="event"/>, one after the other, now.</summary>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}

public sealed class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        foreach (var handler in serviceProvider.GetServices<IEventHandler<TEvent>>())
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}

public static class EventRegistration
{
    /// <summary><see cref="IEventPublisher"/>, and every concrete <see cref="IEventHandler{TEvent}"/> of <paramref name="assembly"/>.</summary>
    public static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IEventPublisher, EventPublisher>();
        foreach (var type in assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false }))
        {
            foreach (var handled in type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            {
                services.AddScoped(handled, type);
            }
        }
        return services;
    }
}
