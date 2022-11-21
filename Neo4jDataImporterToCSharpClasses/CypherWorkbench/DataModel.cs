namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Neo4jJsonToCSharpClasses;
using Newtonsoft.Json;

internal class CypherWorkbench
{
    [JsonProperty("dataModel")]
    public DataModel DataModel { get; set; }
}

internal class DataModel
{
    [JsonProperty("nodeLabels")]
    public IDictionary<string, CypherWorkbenchNode> Nodes { get; set; }
}

internal class CypherWorkbenchNode : INode<CypherWorkbenchProperty>
{
    /// <inheritdoc />
    [JsonProperty("classType")]
    public string? Label { get; set; }

    /// <inheritdoc />
    [JsonProperty("properties")]
    public ICollection<CypherWorkbenchProperty>? Properties { get; set; }
}

internal class CypherWorkbenchProperty : IProperty
{
    /// <inheritdoc />
    [JsonProperty("name")]
    public string Name { get; }

    /// <inheritdoc />
    [JsonProperty("datatype")]
    public string Type { get; }

    /// <inheritdoc />
    [JsonProperty("key")]
    public string Id { get; }
}

internal class CypherWorkbenchRelationship : IRelationship<CypherWorkbenchProperty>
{
    /// <inheritdoc />
    [JsonProperty("classType")]
    public string? Type { get; set; }

    /// <inheritdoc />
    [JsonProperty("startNodeLabelKey")]
    public string? SourceNode { get; set; }

    /// <inheritdoc />
    [JsonProperty("endNodeLabelKey")]
    public string? TargetNode { get; set; }

    /// <inheritdoc />
    [JsonProperty("properties")]
    public ICollection<CypherWorkbenchProperty>? Properties { get; set; }
}