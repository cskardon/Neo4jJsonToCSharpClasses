namespace Neo4jDataImporterToCSharpClasses;

using Newtonsoft.Json;

internal class Property
{
    [JsonProperty("property")]
    public string Name { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("identifier")]
    public string Id { get; set; }
}

