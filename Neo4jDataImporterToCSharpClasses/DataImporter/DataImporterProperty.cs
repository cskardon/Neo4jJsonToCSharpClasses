namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

public class DataImporterProperty : IProperty
{
    [JsonProperty("property")]
    public string? Name { get; set; }
    [JsonProperty("type")]
    public string? Type { get; set; }
    [JsonProperty("identifier")]
    public string? Id { get; set; }
}

