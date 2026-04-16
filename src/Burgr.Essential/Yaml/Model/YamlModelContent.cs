namespace SolidOps.Burgr.Essential.Yaml.Model;

public class YamlModelContentV1
{
    public Dictionary<string, AggregateRoot> aggregate_roots { get; set; } = new Dictionary<string, AggregateRoot>();

    public Dictionary<string, Entity> entities { get; set; } = new Dictionary<string, Entity>();

    public Dictionary<string, Service> services { get; set; } = new Dictionary<string, Service>();

    public Dictionary<string, Transient> transients { get; set; } = new Dictionary<string, Transient>();

    public Dictionary<string, ValueObject> value_objects { get; set; } = new Dictionary<string, ValueObject>();

    public Dictionary<string, Enum> enums { get; set; } = new Dictionary<string, Enum>();

    public Dictionary<string, Event> events { get; set; } = new Dictionary<string, Event>();
}

// aggregates
public class BaseObject
{
    public string @namespace { get; set; }

    public string @interface { get; set; }

    public string id_column_name { get; set; }

    public string table_name { get; set; }

    public string forced_prefix { get; set; }

    public string extends { get; set; }

    public string event_produced { get; set; }

    public bool is_id_private { get; set; }

    public bool cacheable { get; set; }

    public string identity_keys_type { get; set; }

    public Dictionary<string, object> properties { get; set; } // value is string (type) or property

    public Dictionary<string, ApiDescription> api { get; set; }

    public List<string> event_consumers { get; set; }

    public Dictionary<string, string> dependencies { get; set; }

    public Dictionary<string, ComponentDescription> components { get; set; }

    public Dictionary<string, ViewDescription> views { get; set; }

    public Dictionary<string, List<string>> rules { get; set; }

    public string plural { get; set; }

    public Dictionary<string, MethodDescription> factories { get; set; }

    public bool enable_change_tracking { get; set; }
}

public class AggregateRoot : BaseObject
{

}

public class Entity : BaseObject
{

}

public class Property
{
    public string type { get; set; }
    public bool? is_max_size { get; set; }
    public int? field_size { get; set; }
    public bool is_unique { get; set; }
    public bool is_unique_case_sensitive { get; set; }
    public bool is_label { get; set; }
    public bool is_queryable { get; set; }
    public string default_value { get; set; }
    public string special_type { get; set; }
    public string back_link_navigation { get; set; }
    public string join_navigation { get; set; }
    public string db_column_name { get; set; }
    public bool is_private { get; set; }
    public string id_column_name { get; set; }
    public string multiple_unique_constraint_with { get; set; }
}

public class MethodApiDescription : ApiDescription
{
    public bool force_post { get; set; }
    public bool create_component { get; set; }
}

public class ApiDescription
{
    public bool? is_anonymous { get; set; }

    public string mandatory_right { get; set; }

    public string ownership_override_right { get; set; }
}

public class ComponentDescription
{
    public string includes { get; set; }
}

public class ViewDescription
{
}

// service
public class Service
{
    public string @namespace { get; set; }
    public string implements_interfaces { get; set; }
    public Dictionary<string, ServiceMethodDescription> methods { get; set; }
    public Dictionary<string, string> dependencies { get; set; }
    public bool api { get; set; }
    public ApiDescription api_description { get; set; }
}

public class MethodDescription
{
    public Dictionary<string, string> inputs { get; set; } // value is type as string
    public string result { get; set; } // value is type as string
}

public class ServiceMethodDescription : MethodDescription
{
    public bool no_transaction { get; set; }
    public bool api { get; set; }
    public bool allow_write { get; set; }
    public MethodApiDescription api_description { get; set; }
}

// transient
public class Transient : BaseObject
{

}

// value object
public class ValueObject : BaseObject
{

}

// event
public class Event : BaseObject
{
    public string data_type { get; set; }
}


// enum
public class Enum
{
    public string @namespace { get; set; }
    public Dictionary<string, int> values { get; set; }
}
