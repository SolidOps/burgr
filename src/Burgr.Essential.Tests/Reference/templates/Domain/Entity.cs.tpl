// Reference template: one class per aggregate root and entity.
namespace MetaCorp.Template.Domain
{
    #region foreach MODEL[AG][EN]
    // CLASSNAME (MODELNAME)
    public partial class CLASSNAME
    {
        #region to remove at generation
        private const int FIELDSIZE = 0;
        #endregion to remove at generation

        #region to remove if COMPOSED_ID
        public _IDENTITY_KEY_TYPE_ Id { get; set; }
        #endregion to remove if COMPOSED_ID

        #region to remove if NOT_COMPOSED_ID
        public string ComposedId { get; set; }
        #endregion to remove if NOT_COMPOSED_ID

        #region foreach PROPERTY[S][NO][PUO]
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
        
        #endregion foreach PROPERTY
        #region foreach PROPERTY[S][NO][PRO]
        internal _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
        public _ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][NAR]
        public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][LNA]
        public List<_PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
        #endregion foreach PROPERTY

        #region foreach PROPERTY[K]
        // key part _KEYINDEX_: _PROPERTYNAME_
        #endregion foreach PROPERTY

        #region to remove if NO_LABEL
        public override string ToString() => _LABEL_;
        #endregion to remove if NO_LABEL
    }

    #endregion foreach MODEL
}
