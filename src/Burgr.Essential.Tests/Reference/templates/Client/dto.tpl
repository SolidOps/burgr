// Reference template: TypeScript DTOs, written with LF line endings.
#region foreach ENUMTYPE
export enum _ENUMTYPE_ {
  VALUES
}
#endregion foreach ENUMTYPE

#region foreach MODEL[AG][EN][EXP]
export interface CLASSNAMEDTO {
  #region to remove if COMPOSED_ID
  id: string;
  #endregion to remove if COMPOSED_ID
  #region foreach PROPERTY[S][NO][PUO]
  _SIMPLE__PROPERTYNAME_: _SIMPLE__TYPE_;
  #endregion foreach PROPERTY
  #region foreach PROPERTY[M][R][NO][NAR]
  _PROPERTYNAME_Id: string;
  #endregion foreach PROPERTY
}

#endregion foreach MODEL
