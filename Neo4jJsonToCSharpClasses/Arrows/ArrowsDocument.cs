namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class ArrowsDocument
{
    public IDictionary<string, ArrowsNode> Nodes => FileNodes.ToDictionary(x => x.Id, x => x);

    [JsonProperty("nodes")]
    protected ICollection<ArrowsNode> FileNodes { get; set; }

    [JsonProperty("relationships")]
    protected ICollection<ArrowsRelationship> FileRelationships { get; set; }

    public IDictionary<string, ArrowsRelationship> Relationships => FileRelationships.ToDictionary(x => x.Id, x => x);
}

public class ArrowsNode : BaseNode<ArrowsProperty>
{
    ///<summary>
    /// (:<see cref="BaseNode{T}"/>)-[:MEME_IP]->(:<see cref="ArrowsNode"/>)
    ///</summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /*TODO: Caption vs Labels (which is an array of string)*/

    /// <inheritdoc />
    [JsonProperty("caption")]
    public override string? Label { get; set; }

    [JsonProperty("properties")] 
    protected IDictionary<string, string> FileProperties { get; set; } = new Dictionary<string, string>();

    /// <inheritdoc />
    public override ICollection<ArrowsProperty>? Properties
    {
        get => FileProperties.Select(x => new ArrowsProperty {Name = x.Key, Type = x.Value}).ToList();
        set => throw new NotImplementedException();
    }
}

public class ArrowsProperty : IProperty
{
    /// <inheritdoc />
    public string? Name { get; set; }

    /// <inheritdoc />
    public string? Type { get; set; }

    /// <inheritdoc />
    public string? Id => throw new NotImplementedException("Arrows properties don't have an ID");
}

//property-type
public class ArrowsRelationship : IRelationship<ArrowsProperty>
{
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <inheritdoc />
    [JsonProperty("type")]
    public string? Type { get; set; }

    /// <inheritdoc />
    [JsonProperty("fromId")]
    public string? SourceNode { get; set; }

    /// <inheritdoc />
    [JsonProperty("toId")]
    public string? TargetNode { get; set; }

    [JsonProperty("properties")] 
    protected IDictionary<string, string> FileProperties { get; set; } = new Dictionary<string, string>();

    /// <inheritdoc />
    public ICollection<ArrowsProperty>? Properties => FileProperties.Select(x => new ArrowsProperty { Name = x.Key, Type = x.Value }).ToList();
}