namespace Neo4jJsonToCSharpClasses.Tests.CypherWorkbench;

using FluentAssertions;
using Neo4jJsonToCSharpClasses.CypherWorkbench;
using Newtonsoft.Json;

public class ParsingTests
{
    private const string json = @"{
""dataModel"": {
    ""nodeLabels"": {
      ""NodeX"": {
        ""classType"": ""NodeLabel"",
        ""label"": ""Foo"",
        ""fromDataSources"": [],
        ""key"": ""NodeX"",
        ""indexes"": [],
        ""properties"": {
          ""PropertyX"": {
            ""key"": ""PropertyX"",
            ""name"": ""Bar"",
            ""datatype"": ""StringArray""
          }
        }
      }
    }
  }
}";

    [Fact]
    public void ParsesJsonCorrectly()
    {
        var model = JsonConvert.DeserializeObject<CypherWorkbench>(json)!;
        model.Should().NotBeNull();
        model.DataModel.Should().NotBeNull();
        
        var nodes = model.DataModel.Nodes;
        nodes.Should().NotBeNull();
        nodes.Count.Should().Be(1);
        nodes.Keys.First().Should().Be("NodeX");
        
        var node = nodes.First().Value;
        node.Label.Should().Be("Foo");
        node.Properties.Should().HaveCount(1);

        var property = node.Properties.First();
        property.Name.Should().Be("Bar");
        property.Id.Should().Be("PropertyX");
        property.Type.Should().Be("StringArray");
    }
}