namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbenchNode : INode<CypherWorkbenchProperty>
{
    /// <inheritdoc />
    [JsonProperty("label")]
    public string? Label { get; set; }

    public ICollection<CypherWorkbenchProperty>? Properties => PropertiesDictionary?.Values ?? Array.Empty<CypherWorkbenchProperty>();

    /// <inheritdoc />
    [JsonProperty("properties")]
    public IDictionary<string, CypherWorkbenchProperty>? PropertiesDictionary { get; set; }
}