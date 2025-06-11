using Microsoft.EntityFrameworkCore.Infrastructure;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Text.Json.Serialization;

#region foreach MODEL[AG][EN]
namespace MetaCorp.Template.Domain._DOMAINTYPE_
{
    public partial class CLASSNAME : IEFEntity<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>
    {
        public ILazyLoader LazyLoader { get; set; } = null;

        public CLASSNAME(ILazyLoader lazyLoader) : this()
        {
            LazyLoader = lazyLoader;
        }

        protected override void LazyLoad(string navigationName)
        {
            LazyLoader.Load(this, navigationName);
        }
    }
}
#endregion foreach MODEL