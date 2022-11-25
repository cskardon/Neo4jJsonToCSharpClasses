namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

public class Neo4jImporter
{
    [JsonProperty("version")]
    public Version Version { get; set; }

    [JsonProperty("dataModel")]
    public DataImporterDataModel? DataModel { get; set; }
}