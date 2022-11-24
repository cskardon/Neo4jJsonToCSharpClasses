namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbench
{
    [JsonProperty("metadata")]
    public CypherWorkbenchMetadata? Metadata { get; set; }

    [JsonProperty("dataModel")]
    public CypherWorkbenchDataModel? DataModel { get; set; }
}