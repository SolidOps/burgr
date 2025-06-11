//using SolidOps.UM.Shared.Contracts.Results;
//using SolidOps.SubZero;
//using System.Text.Json;

//namespace SolidOps.UM.Application.UseCases;

//public partial class ConfigurationsUseCase
//{
//    public override async Task<IOpsResult<string>> GetConfiguration(string application, string environment)
//    {
//        var configurations = await _dependencyApplicationRepository.GetConfigurations(application, environment);

//        var dictionary = new Dictionary<string, string>();
//        foreach (var configuration in configurations)
//        {
//            if (!string.IsNullOrEmpty(configuration))
//            {
//                var json = JsonDocument.Parse(configuration).RootElement.Clone();
//                Do(dictionary, json, "");
//            }
//        }
//        return IOpsResult.Ok(Serializer.Serialize(dictionary));
//    }

//    private void Do(Dictionary<string, string> output, JsonElement input, string parentKey)
//    {
//        foreach (var prop in input.EnumerateObject())
//        {
//            if (prop.Value.ValueKind == JsonValueKind.Object)
//            {
//                Do(output, prop.Value, parentKey + prop.Name + ":");
//            }
//            else
//            {
//                var key = parentKey + prop.Name;
//                if (!output.ContainsKey(key))
//                    output.Add(key, prop.Value.ToString());
//                else
//                    output[key] = prop.Value.ToString();
//            }
//        }
//    }
//}
