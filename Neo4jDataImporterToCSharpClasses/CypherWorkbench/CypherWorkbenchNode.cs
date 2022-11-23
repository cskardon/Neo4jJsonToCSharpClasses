namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbenchNode : BaseNode<CypherWorkbenchProperty>
{
    [JsonProperty("properties")]
    protected IDictionary<string, CypherWorkbenchProperty>? PropertiesDictionary { get; set; }

    /// <inheritdoc />
    [JsonProperty("label")]
    public override string? Label { get; set; }

    /// <inheritdoc />
    public override ICollection<CypherWorkbenchProperty>? Properties => PropertiesDictionary.Values ?? Array.Empty<CypherWorkbenchProperty>();
}