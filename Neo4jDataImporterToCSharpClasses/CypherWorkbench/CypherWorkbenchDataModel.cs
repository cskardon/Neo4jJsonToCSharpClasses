namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Neo4jJsonToCSharpClasses;
using Newtonsoft.Json;

public class CypherWorkbench
{
    [JsonProperty("dataModel")]
    public CypherWorkbenchDataModel? DataModel { get; set; }
}

public class CypherWorkbenchDataModel
{
    [JsonProperty("nodeLabels")]
    public IDictionary<string, CypherWorkbenchNode>? Nodes { get; set; }

    [JsonProperty("relationshipTypes")]
    public IDictionary<string, CypherWorkbenchRelationship>? Relationships { get; set; }
}

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

public class CypherWorkbenchProperty : IProperty
{
    /// <inheritdoc />
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <inheritdoc />
    [JsonProperty("datatype")]
    public string? Type { get; set;}
    
    /// <inheritdoc />
    [JsonProperty("key")]
    public string? Id { get; set;}
}

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