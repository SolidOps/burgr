namespace SolidOps.Burgr.Essential.Yaml.Template;

public class YamlTemplateContentV1
{
    public Dictionary<string, source_template> source_templates { get; set; } = new Dictionary<string, source_template>();
}

public class source_template
{
    public string source { get; set; }

    public string file_suffix { get; set; }

    public string destination_path { get; set; }

    public string override_destination_folder { get; set; }

    public string destination_suffix { get; set; }

    public bool force_separate_dll { get; set; }

    public string destination_language { get; set; }
    
    public string path_style { get; set; }

    public bool is_generate_per_model_description { get; set; }

    public int? children_level { get; set; }

    public options options { get; set; }
}

public class options
{
    public string model_prefix { get; set; }
    public string model_suffix { get; set; }
}
