namespace Neo4jJsonToCSharpClasses.DataImporter;

using Newtonsoft.Json;

internal class DataImporterNode : INode<DataImporterProperty>
{
    /// <inheritdoc />
    [JsonProperty("label")]
    public string? Label { get; set; }

    /// <inheritdoc />
    [JsonProperty("properties")]
    public ICollection<DataImporterProperty>? Properties { get; set; }
}