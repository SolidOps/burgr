namespace SolidOps.Burgr.Essential.Yaml.Template;

public class YamlTemplateContentV1
{
    public Dictionary<string, SourceTemplate> source_templates { get; set; } = new Dictionary<string, SourceTemplate>();
}

public class SourceTemplate
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

    public Options options { get; set; }
}

public class Options
{
    public string model_prefix { get; set; }
    public string model_suffix { get; set; }
}
