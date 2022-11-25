namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

public class DataImporterDataModel
{
    [JsonProperty("graphModel")]
    public DataImporterGraphModel? GraphModel { get; set; }
}