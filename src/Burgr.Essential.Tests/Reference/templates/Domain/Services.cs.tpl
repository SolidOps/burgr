namespace MetaCorp.Template.Application
{
    #region foreach DOMAIN_SERVICE
    public partial interface ISERVICENAMEService
    {
        #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
        Task _DOVOIDACTION_(/*PARAMETER_DEFINITION*/); // POST SlugTemplate/SlugSERVICENAME/_DOVOIDACTIONURL_
        #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
        #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
        Task<object> _DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/); // SlugTemplate/SlugSERVICENAME/_DOSIMPLEACTIONURL_
        #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
        #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
        Task<object[]> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/); // SlugTemplate/SlugSERVICENAME/_DOMODELLISTACTIONURL_
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    }
    #endregion foreach DOMAIN_SERVICE
}
