using SolidOps.Burgr.Essential.Yaml.Model;

namespace SolidOps.Burgr.Essential.Yaml;

// The model features a YAML model uses, named as in a template set's SupportedFeatures.
public static class ModelFeatures
{
    public const string AggregateRoots = "aggregate_roots";
    public const string Entities = "entities";
    public const string ValueObjects = "value_objects";
    public const string Transients = "transients";
    public const string Events = "events";
    public const string Services = "services";
    public const string Enums = "enums";
    public const string References = "references";
    public const string ExternalReferences = "external_references";
    public const string Navigations = "navigations";
    public const string SynchronizedNavigations = "synchronized_navigations";
    public const string CalculatedProperties = "calculated_properties";
    public const string CalculatedCollections = "calculated_collections";
    public const string PrimitiveCollections = "primitive_collections";
    public const string CompositeUniqueConstraints = "composite_unique_constraints";
    public const string NonPersistedProperties = "non_persisted_properties";
    public const string CompositeKeys = "composite_keys";
    public const string Rights = "rights";
    public const string Rules = "rules";
    public const string Factories = "factories";
    public const string UiComponents = "ui_components";

    public static SortedSet<string> Detect(IDictionary<string, YamlModelContentV1> contentByModule)
    {
        var features = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var (moduleName, content) in contentByModule)
        {
            AddObjects(features, AggregateRoots, content.aggregate_roots, moduleName);
            AddObjects(features, Entities, content.entities, moduleName);
            AddObjects(features, ValueObjects, content.value_objects, moduleName);
            AddObjects(features, Transients, content.transients, moduleName);
            AddObjects(features, Events, content.events, moduleName);
            if (content.services?.Count > 0)
            {
                features.Add(Services);
            }
            if (content.enums?.Count > 0)
            {
                features.Add(Enums);
            }
        }
        return features;
    }

    private static void AddObjects<T>(SortedSet<string> features, string feature, Dictionary<string, T> objects, string moduleName) where T : BaseObject
    {
        if (objects == null || objects.Count == 0)
        {
            return;
        }
        features.Add(feature);

        foreach (var value in objects.Values.Where(v => v != null))
        {
            if (value.key?.Count > 0)
            {
                features.Add(CompositeKeys);
            }
            if (!string.IsNullOrEmpty(value.event_produced) || value.event_consumers?.Count > 0)
            {
                features.Add(Events);
            }
            if (value.api?.Values.Any(a => a != null && (!string.IsNullOrEmpty(a.mandatory_right) || !string.IsNullOrEmpty(a.ownership_override_right))) == true)
            {
                features.Add(Rights);
            }
            if (value.rules?.Count > 0)
            {
                features.Add(Rules);
            }
            if (value.factories?.Count > 0)
            {
                features.Add(Factories);
            }
            if (value.components?.Count > 0 || value.views?.Count > 0)
            {
                features.Add(UiComponents);
            }
            foreach (var property in value.properties ?? [])
            {
                AddProperty(features, GetType(property.Value), moduleName);
                if (property.Value is Dictionary<object, object> options && options.TryGetValue("multiple_unique_constraint_with", out var with) && with != null)
                {
                    features.Add(CompositeUniqueConstraints);
                }
            }
        }
    }

    private static void AddProperty(SortedSet<string> features, string type, string moduleName)
    {
        var typeInfo = new TypeInfo(type, moduleName);
        if (typeInfo.IsCalculated)
        {
            features.Add(typeInfo.IsArray ? CalculatedCollections : CalculatedProperties);
        }
        else if (typeInfo.IsNonPersisted)
        {
            features.Add(NonPersistedProperties);
        }
        else if (typeInfo.IsSynchronized)
        {
            features.Add(SynchronizedNavigations);
        }
        else if (typeInfo.IsNavigation)
        {
            features.Add(Navigations);
        }
        else if (typeInfo.TypeType == TypeType.Model)
        {
            features.Add(References);
        }

        if (typeInfo.IsArray && !typeInfo.IsCalculated && !typeInfo.IsNonPersisted && (typeInfo.TypeType == TypeType.Simple || typeInfo.TypeType == TypeType.Enum))
        {
            features.Add(PrimitiveCollections);
        }

        if (typeInfo.TypeType == TypeType.ReferencedModel)
        {
            features.Add(ExternalReferences);
        }
    }

    // same reading as ObjectModelParser: a type name, or a map whose type defaults to string
    private static string GetType(object property)
    {
        if (property is Dictionary<object, object> dictionary)
        {
            return dictionary.TryGetValue("type", out var type) && type is string name ? name : "string";
        }
        return property as string ?? "string";
    }
}
