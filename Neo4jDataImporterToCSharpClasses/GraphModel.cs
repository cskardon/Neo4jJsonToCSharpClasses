namespace Neo4jDataImporterToCSharpClasses;

using Newtonsoft.Json;

internal class Neo4jImporter
{
    [JsonProperty("dataModel")]
    public DataModel DataModel { get; set; }
}

internal class DataModel
{
    [JsonProperty("graphModel")]
    public GraphModel GraphModel { get; set; }
}

internal class GraphModel
{
    [JsonProperty("nodeSchemas")]
    public IDictionary<string, NodeSchema> NodeSchemas { get; set; }

    [JsonProperty("relationshipSchemas")]
    public IDictionary<string, RelationshipSchema> RelationshipSchemas { get; set; }
}
