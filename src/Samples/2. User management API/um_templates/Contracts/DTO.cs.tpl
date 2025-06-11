using SolidOps.UM.Shared.Contracts.DTO;
using System.Collections.Generic;
using System; // needed for common types

namespace MetaCorp.Template.Contracts
{
    public static class AssemblyReference
    {
    }
}

#region foreach ENUMTYPE
namespace MetaCorp.Template.Contracts.Enums
{
    public enum _ENUMTYPE_Enum
    {
        VALUES
    }
}
#endregion foreach ENUMTYPE

namespace MetaCorp.Template.Contracts.DTO
{
    #region foreach MODEL[EN][AG][EXP]
    public partial class CLASSNAMEDTO
    {
        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        public string _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        public string _REF__PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region to remove if PRIVATE_ID
        public string Id { get; set; }
        #endregion to remove if PRIVATE_ID

        // relations

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][CA][NA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _REF__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][CA][NA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _FORLIST__REF__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY
    }

    public partial class CLASSNAMEWriteDTO
    {
        public CLASSNAMEWriteDTO()
        {

        }

        public CLASSNAMEWriteDTO(CLASSNAMEDTO source)
        {
            #region foreach PROPERTY[S][NO][NP][PUO]
            // write
            this._SIMPLE__PROPERTYNAME_ = source._SIMPLE__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][NO][NP][PUO][NAR]
            // write
            this._ENUM__PROPERTYNAME_ = source._ENUM__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][NO][NP][PUO][AR]
            // write
            this._FORLIST__ENUM__PROPERTYNAME_ = source._FORLIST__ENUM__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][NO][PUO][EN][AG]
            // write
            this._PROPERTYNAME_Id = source._PROPERTYNAME_Id;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[R][NO][PUO][EN][AG]
            // write
            this._REF__PROPERTYNAME_Id = source._REF__PROPERTYNAME_Id;
            #endregion foreach PROPERTY

            // relations

            #region foreach PROPERTY[M][NO][NA][PUO][NAR][EN][AG]
            // write
            this._PROPERTYNAME_ = new DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_WriteDTO(source._PROPERTYNAME_);
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][NO][NA][PUO][AR][EN][AG]
            // write
            this._FORLIST__PROPERTYNAME_ = new List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_WriteDTO>();
            if (source._FORLIST__PROPERTYNAME_ != null)
            {
                foreach (var item in source._FORLIST__PROPERTYNAME_)
                {
                    this._FORLIST__PROPERTYNAME_.Add(new DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_WriteDTO(item));
                }
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][NO][NA][PUO][NAR][VO]
            // write vo
            this._VALUEOBJECT__PROPERTYNAME_ = source._PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][NO][NA][PUO][AR][VO]
            // write vo
            this._VALUEOBJECT__FORLIST__PROPERTYNAME_ = source._FORLIST__PROPERTYNAME_;
            #endregion foreach PROPERTY
        }

        #region foreach PROPERTY[S][NO][NP][PUO]
        // write
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO][NAR]
        // write
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO][AR]
        // write
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        // write
        public string _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        // write
        public string _REF__PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        // relations

        #region foreach PROPERTY[M][NO][NA][PUO][NAR][EN][AG]
        // write
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_WriteDTO _PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][AR][EN][AG]
        // write
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_WriteDTO> _FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][NAR][VO]
        // write vo
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _VALUEOBJECT__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][AR][VO]
        // write vo
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _VALUEOBJECT__FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY
    }

    public partial class CLASSNAMEPatchDTO
    {
        #region foreach PROPERTY[S][NO][NP][PUO]
        // patch
        public _SIMPLE__TYPE_/*_ISPATCHNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO][NAR]
        // patch
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO][AR]
        // patch
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        // patch
        public string _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        // patch
        public string _REF__PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        // relations

        #region foreach PROPERTY[M][NO][NA][PUO][NAR][EN][AG]
        // patch
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_PatchDTO _PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][AR][EN][AG]
        // patch
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_PatchDTO> _FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][NAR][VO]
        // patch vo
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _VALUEOBJECT__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NA][PUO][AR][VO]
        // patch vo
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _VALUEOBJECT__FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY
    }
    #endregion foreach MODEL

    #region foreach MODEL[TR]
    public partial class _TR_CLASSNAMEDTO
    {
        #region to remove if PRIVATE_ID
        public string Id { get; set; }
        #endregion to remove if PRIVATE_ID

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][PUO][EN][AG]
        public string _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][PUO][EN][AG]
        public string _REF__PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY
    }
    #endregion foreach MODEL

    #region foreach MODEL[VO][EXP]
    public partial class _VO_CLASSNAMEDTO
    {
        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        // public string _PROPERTYNAME_Id { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO _PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][NA][PUO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> _FORLIST__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        public string ContinuationId { get; set; }
    }
    #endregion foreach MODEL

    #region foreach MODEL[EN][AG]
    public partial class CLASSNAMEQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }
    #endregion foreach MODEL
}

