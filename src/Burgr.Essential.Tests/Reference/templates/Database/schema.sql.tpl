-- Reference template: one table per persisted object.
#region foreach MODEL[AG][EN]
CREATE TABLE MODELNAME (
#region foreach PROPERTY[S][NO]
    _SIMPLE__PROPERTYNAME_ VARCHAR(FIELDSIZE),
#endregion foreach PROPERTY
#region foreach PROPERTY[M][R][NO][NAR]
    _PROPERTYNAME__id CHAR(36),
#endregion foreach PROPERTY
    PRIMARY KEY (IDCOLUMNNAME)
);

#endregion foreach MODEL
