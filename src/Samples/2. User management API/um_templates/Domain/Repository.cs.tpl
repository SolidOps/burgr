using SolidOps.UM.Shared.Domain.UnitOfWork;
using MetaCorp.Template.Domain.Queries;

namespace MetaCorp.Template.Domain.Repositories
{
    public partial interface ITemplateDataAccessFactory : IDataAccessFactory
    {
    }

    #region foreach MODEL[EN][AG]

    public partial interface ICLASSNAMERepository : IReadWriteDomainRepository<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>
    {
        // Queries

        #region foreach QUERYABLE_PROPERTY
        Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null, bool disableAccessRightCheck = false);
        #endregion foreach QUERYABLE_PROPERTY

        #region foreach UNIQUE_QUERYABLE_PROPERTY
        Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null, bool disableAccessRightCheck = false);
        #endregion foreach UNIQUE_QUERYABLE_PROPERTY

        #region foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
        Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_MULTIPLE_CONSTRAINT_(/*_CONSTRAINT_PARAMS_TYPED_*/, string includes = null, bool disableAccessRightCheck = false);
        #endregion foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY

        Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetList(CLASSNAMEQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);

        Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingle(CLASSNAMEQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);

        Domain._DOMAINTYPE_.CLASSNAME MapToDomain(Domain._DOMAINTYPE_.CLASSNAME efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

    #endregion foreach MODEL
}

namespace MetaCorp.Template.Domain.Queries
{
    #region foreach MODEL[EN][AG]
    public class CLASSNAMEQueryFilter : BaseQueryFilter
    {
        public CLASSNAMEQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {

        }
    }
    #endregion foreach MODEL
}
