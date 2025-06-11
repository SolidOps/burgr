using Microsoft.EntityFrameworkCore.Infrastructure;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Text.Json.Serialization;
// Object [AG][EN]
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class LocalUser : IEFEntity<Guid, Domain.AggregateRoots.LocalUser>
    {
        public ILazyLoader LazyLoader { get; set; } = null;
        public LocalUser(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }
        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class User : IEFEntity<Guid, Domain.AggregateRoots.User>
    {
        public ILazyLoader LazyLoader { get; set; } = null;
        public User(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }
        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public partial class UserRight : IEFEntity<Guid, Domain.Entities.UserRight>
    {
        public ILazyLoader LazyLoader { get; set; } = null;
        public UserRight(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }
        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public partial class Right : IEFEntity<Guid, Domain.Entities.Right>
    {
        public ILazyLoader LazyLoader { get; set; } = null;
        public Right(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }
        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class Invite : IEFEntity<Guid, Domain.AggregateRoots.Invite>
    {
        public ILazyLoader LazyLoader { get; set; } = null;
        public Invite(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }
        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}