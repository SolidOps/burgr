namespace SolidOps.Burgr.Essential.Yaml.Model;

public class YamlModelContentV1
{
    public Dictionary<string, aggregate_root> aggregate_roots { get; set; } = new Dictionary<string, aggregate_root>();

    public Dictionary<string, entity> entities { get; set; } = new Dictionary<string, entity>();

    public Dictionary<string, service> services { get; set; } = new Dictionary<string, service>();

    public Dictionary<string, transient> transients { get; set; } = new Dictionary<string, transient>();

    public Dictionary<string, value_object> value_objects { get; set; } = new Dictionary<string, value_object>();

    public Dictionary<string, @enum> enums { get; set; } = new Dictionary<string, @enum>();

    public Dictionary<string, @event> events { get; set; } = new Dictionary<string, @event>();
}

// aggregates
public class base_object
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

    public Dictionary<string, api_description> api { get; set; }

    public List<string> event_consumers { get; set; }

    public Dictionary<string, string> dependencies { get; set; }

    public Dictionary<string, component_description> components { get; set; }

    public Dictionary<string, view_description> views { get; set; }

    public Dictionary<string, List<string>> rules { get; set; }
}
public class aggregate_root : base_object
{

}

public class entity : base_object
{

}

public class property
{
    public string type { get; set; }
    public int? max_size { get; set; }
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

public class api_description
{
    public bool is_anonymous { get; set; }

    public string mandatory_right { get; set; }

    public string ownership_override_right { get; set; }
}

public class component_description
{

}

public class view_description
{

}

// service
public class service
{
    public string @namespace { get; set; }
    public bool is_anonymous { get; set; }
    public bool create_rest_services { get; set; }
    public string mandatory_right { get; set; }
    public string ownership_override_right { get; set; }
    public bool is_internal { get; set; }
    public string implements_interfaces { get; set; }
    public Dictionary<string, method> methods { get; set; }
    public Dictionary<string, string> dependencies { get; set; }
}

public class method
{
    public Dictionary<string, string> inputs { get; set; } // value is type as string
    public string result { get; set; } // value is type as string
    public bool no_transaction { get; set; }
    public string mandatory_right { get; set; }
    public string ownership_override_right { get; set; }
    public bool force_post { get; set; }
    public bool create_component { get; set; }
}

// transient
public class transient : base_object
{

}

// value object
public class value_object : base_object
{

}

// event
public class @event : base_object
{
    public string data_type { get; set; }
}


// enum
public class @enum
{
    public string @namespace { get; set; }
    public Dictionary<string, int> values { get; set; }
}
