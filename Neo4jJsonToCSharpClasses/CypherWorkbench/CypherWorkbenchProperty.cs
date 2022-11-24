namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

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