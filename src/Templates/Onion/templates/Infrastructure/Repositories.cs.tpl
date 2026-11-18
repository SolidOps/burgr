#nullable enable

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _MODULE_NAMESPACE_.Infrastructure;

#region foreach MODEL[AG]
public partial class CLASSNAMERepository(_MODULE_DbContext db) : EfRepository<Domain.CLASSNAME>(db), Domain.ICLASSNAMERepository
{
    protected override IQueryable<Domain.CLASSNAME> WithDetails(IQueryable<Domain.CLASSNAME> query)
    {
        return query
            #region foreach PROPERTY[M][LNA][AG][EN]
            .Include(x => x._PROPERTYNAME_)
            #endregion foreach PROPERTY
            ;
    }
}

#endregion foreach MODEL
