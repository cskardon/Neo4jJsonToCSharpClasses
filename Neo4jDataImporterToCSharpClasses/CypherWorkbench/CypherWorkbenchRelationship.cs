namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbenchRelationship : IRelationship<CypherWorkbenchProperty>
{
    /// <inheritdoc />
    [JsonProperty("type")]
    public string? Type { get; set; }

    /// <inheritdoc />
    [JsonProperty("startNodeLabelKey")]
    public string? SourceNode { get; set; }

    /// <inheritdoc />
    [JsonProperty("endNodeLabelKey")]
    public string? TargetNode { get; set; }
    
    public ICollection<CypherWorkbenchProperty>? Properties => PropertiesDictionary?.Values ?? Array.Empty<CypherWorkbenchProperty>();

    /// <inheritdoc />
    [JsonProperty("properties")]
    public IDictionary<string, CypherWorkbenchProperty>? PropertiesDictionary { get; set; }
}