namespace Neo4jJsonToCSharpClasses.CypherWorkbench;

using Newtonsoft.Json;

public class CypherWorkbenchMetadata
{
    [JsonProperty("cypherWorkbenchVersion")]
    public Version? Version { get; set; }
}