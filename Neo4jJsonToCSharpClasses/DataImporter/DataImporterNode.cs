namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

public class DataImporterNode : BaseNode<DataImporterProperty>
{
    /// <inheritdoc />
    [JsonProperty("label")]
    public override string? Label { get; set; }

    /// <inheritdoc />
    [JsonProperty("properties")]
    public override ICollection<DataImporterProperty>? Properties { get; }
}