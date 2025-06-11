using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Application.Events;
using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Presentation.ETag;

public class ETagsEntityEventHandler<TEntity, TEvent, T> : IEntityEventHandler<TEntity, TEvent, T>
    where TEvent : ISerializableEventOf<TEntity>
    where TEntity : class, IDomainEntity<T>
    where T: struct
{
    private readonly IServiceProvider serviceProvider;
    private readonly ETagConfiguration eTagConfiguration;

    public ETagsEntityEventHandler(IServiceProvider serviceProvider, ETagConfiguration eTagConfiguration)
    {
        this.serviceProvider = serviceProvider;
        this.eTagConfiguration = eTagConfiguration;
    }

    public async Task<IOpsResult> Handle(BusMessage<TEvent> message)
    {
        if (message.Event.EventContext != "Add"
            && message.Event.EventContext != "Update"
            && message.Event.EventContext != "Remove")
        {
            return IOpsResult.Ok();
        }

        if (eTagConfiguration.AreETagsManagedForType(typeof(TEntity)))
        {
            var repoType = typeof(IETagRepository<,>).MakeGenericType(typeof(TEntity), typeof(T));
            var repo = (IETagRepository<TEntity, T>)serviceProvider.GetRequiredService(repoType);
            switch (message.Event.EventContext)
            {
                case "Add":
                case "Update":
                    repo.ChangeETag(message.Event.DataId);
                    repo.ChangeWholeTableETag();
                    break;
                case "Remove":
                    repo.RemoveETag(message.Event.DataId);
                    repo.ChangeWholeTableETag();
                    break;
                default:
                    break;
            }
        }

        var tagTypes = eTagConfiguration.GetETagsManagedTypesDependingOf(typeof(TEntity));
        foreach (var tagType in tagTypes)
        {
            var subRepoType = typeof(IETagRepository<,>).MakeGenericType(tagType, typeof(T));
            var subRepo = (IETagRepository<TEntity, T>)serviceProvider.GetRequiredService(subRepoType);
            switch (message.Event.EventContext)
            {
                case "Add":
                case "Update":
                case "Delete":
                    subRepo.Clear();
                    break;
                default:
                    break;
            }
        }

        return IOpsResult.Ok();
    }
}
