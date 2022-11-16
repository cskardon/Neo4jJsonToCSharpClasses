namespace Neo4jDataImporterToCSharpClasses;

using Newtonsoft.Json;

internal class NodeSchema
{
    [JsonProperty("label")]
    public string Label { get; set; }
    [JsonProperty("properties")]
    public ICollection<Property> Properties { get; set; }
}