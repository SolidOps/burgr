@startuml
hide empty members

#region foreach MODEL[AG]
class CLASSNAME <<aggregate root>>
#endregion foreach MODEL
#region foreach MODEL[EN]
class CLASSNAME <<entity>>
#endregion foreach MODEL
#region foreach MODEL[VO]
class CLASSNAME <<value object>>
#endregion foreach MODEL

#region foreach MODEL[AG][EN][VO]
class CLASSNAME {
    #region foreach PROPERTY[S][NO][CA][NAR][PUO]
    +_PROPERTYNAME_: _SIMPLE__TYPE_/*_ISNULL_*/
    #endregion foreach PROPERTY
    #region foreach RULE
    +_RULE_()
    #endregion foreach RULE
}

#region foreach PROPERTY[E][NO][CA][NAR][PUO]
CLASSNAME --> _ENUMTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][NO][NAR][AG][EN][NN]
CLASSNAME --> "1" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][NO][NAR][AG][EN][N]
CLASSNAME --> "0..1" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][NO][NAR][VO][NN]
CLASSNAME *-- "1" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][NO][NAR][VO][N]
CLASSNAME *-- "0..1" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][NO][AR][VO]
CLASSNAME *-- "*" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY
#region foreach PROPERTY[M][LNA][AG][EN]
CLASSNAME o-- "*" _PROPERTYTYPE_ : _PROPERTYNAME_
#endregion foreach PROPERTY

#endregion foreach MODEL
#region foreach ENUMTYPE
enum _ENUMTYPE_ {
    VALUES
}
#endregion foreach ENUMTYPE
@enduml
