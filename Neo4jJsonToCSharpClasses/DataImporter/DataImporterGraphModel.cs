namespace Neo4jJsonToCSharpClasses.DataImporter;
using Newtonsoft.Json;

public class Neo4jImporter
{
    [JsonProperty("dataModel")]
    public DataImporterDataModel? DataModel { get; set; }
}

public class DataImporterDataModel
{
    [JsonProperty("graphModel")]
    public DataImporterGraphModel? GraphModel { get; set; }
}

public class DataImporterGraphModel
{
    [JsonProperty("nodeSchemas")]
    public IDictionary<string, DataImporterNode>? Nodes { get; set; }

    [JsonProperty("relationshipSchemas")]
    public IDictionary<string, DataImporterRelationship>? Relationships { get; set; }
}
