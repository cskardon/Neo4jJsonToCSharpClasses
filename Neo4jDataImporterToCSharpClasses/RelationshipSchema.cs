namespace Neo4jDataImporterToCSharpClasses;

using Newtonsoft.Json;

internal class RelationshipSchema
{
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("sourceNodeSchema")]
    public string SourceNode { get; set; }
    [JsonProperty("targetNodeSchema")]
    public string TargetNode { get; set; }
    [JsonProperty("properties")]
    public ICollection<Property> Properties { get; set;}
}