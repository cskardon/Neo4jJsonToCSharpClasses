namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

public class DataImporterRelationship : IRelationship<DataImporterProperty>
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("sourceNodeSchema")] 
    public string SourceNode { get; set; }
    
    [JsonProperty("targetNodeSchema")]
    public string TargetNode { get; set; }
    
    [JsonProperty("properties")]
    public ICollection<DataImporterProperty>? Properties { get; set; }
}

