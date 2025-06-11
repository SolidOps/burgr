using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Application.Events;
using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Infrastructure;
using System.Reflection;

namespace SolidOps.UM.Shared.Presentation;

public class EventBusProvider : IEventBusProvider
{
    private readonly IServiceProvider _serviceProvider;

    private MethodInfo _notifyMethod;

    public EventBusProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual void Notify<T>(T busEvent)
        where T : ISerializableEvent
    {
        if (_serviceProvider != null)
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<T>>();
            // a bug with net6.0 leads to have repository registered twice
            var treatedHandlers = new List<string>();
            foreach (var handler in handlers)
            {
                var handlerName = handler.GetType().FullName;
                if (!treatedHandlers.Contains(handlerName))
                {
                    handler.Handle(new BusMessage<T>(busEvent)).GetAwaiter().GetResult();
                    treatedHandlers.Add(handlerName);
                }
            }

            // Generic EntityEventHandler
            var eventType = busEvent.GetEventType();
            var dataType = busEvent.GetDataType();
            var identityType = busEvent.GetIdentityType();
            if (dataType != null && identityType != null)
            {
                var entityEventHandlerType = typeof(IEntityEventHandler<,,>).MakeGenericType(dataType, eventType, identityType);

                var entityHandlers = _serviceProvider.GetServices(entityEventHandlerType);
                foreach (var handler in entityHandlers)
                {
                    ((IEventHandler<T>)handler).Handle(new BusMessage<T>(busEvent)).GetAwaiter().GetResult();
                }
            }
        }
    }

    public virtual void Notify(IEnumerable<ISerializableEvent> events)
    {
        foreach (var busEvent in events)
        {
            if (_notifyMethod == null)
            {
                _notifyMethod = this.GetType().GetMethods().Where(m => m.Name == "Notify" && m.IsGenericMethod).Single();
            }
            _notifyMethod.MakeGenericMethod(busEvent.GetEventType()).Invoke(this, new object[] { busEvent });
        }
    }
    public virtual void NotifyNonGeneric(ISerializableEvent @event)
    {
        Notify([@event]);
    }
}
