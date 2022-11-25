namespace Neo4jJsonToCSharpClasses.DataImporter;
using Newtonsoft.Json;

public class DataImporterGraphModel
{
    [JsonProperty("nodeSchemas")]
    public IDictionary<string, DataImporterNode>? Nodes { get; set; }

    [JsonProperty("relationshipSchemas")]
    public IDictionary<string, DataImporterRelationship>? Relationships { get; set; }
}
