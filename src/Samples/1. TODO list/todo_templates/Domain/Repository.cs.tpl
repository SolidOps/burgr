using SolidOps.TODO.Shared.Domain;

namespace MetaCorp.Template.Domain.Repositories;

#region foreach MODEL[EN][AG]

public partial interface ICLASSNAMERepository : IReadWriteDomainRepository<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>
{
    // Queries

    #region foreach QUERYABLE_PROPERTY
    Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null);
    #endregion foreach QUERYABLE_PROPERTY

    #region foreach UNIQUE_QUERYABLE_PROPERTY
    Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null);
    #endregion foreach UNIQUE_QUERYABLE_PROPERTY

    #region foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
    Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_MULTIPLE_CONSTRAINT_(/*_CONSTRAINT_PARAMS_TYPED_*/, string includes = null);
    #endregion foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
}

#endregion foreach MODEL

