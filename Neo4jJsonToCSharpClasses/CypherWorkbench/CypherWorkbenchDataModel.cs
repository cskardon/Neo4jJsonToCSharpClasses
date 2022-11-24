namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbenchDataModel
{
    [JsonProperty("nodeLabels")]
    public IDictionary<string, CypherWorkbenchNode>? Nodes { get; set; }

    [JsonProperty("relationshipTypes")]
    public IDictionary<string, CypherWorkbenchRelationship>? Relationships { get; set; }
}