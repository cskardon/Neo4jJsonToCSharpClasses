using Neo4jJsonToCSharpClasses.DataImporter;

namespace Neo4jJsonToCSharpClasses;

using Neo4jJsonToCSharpClasses.CypherWorkbench;

public static class Generate
{
    public const string BaseNodeClass = @"public abstract class NodeBase
{
    protected NodeBase(string labels)
    {
        Labels = labels;
    }

    public static string? Labels { get; private set; }
}";

    public const string BaseRelationshipClass = @"public abstract class RelationshipBase
{
    protected RelationshipBase(string type)
    {
        Type = type;
    }

    public static string? Type { get; private set; }
}";

    private static IEnumerable<string>? GenerateProperties(ICollection<IProperty>? properties, bool upperCamelCaseProperties)
    {
        var output = new List<string>();
        foreach (var property in properties) output.Add(GenerateProperty(property, upperCamelCaseProperties));

        return output;
    }

    private static string GenerateProperty(IProperty property, bool upperCamelCaseProperty)
    {
        var name = upperCamelCaseProperty ? property.Name.ToUpperCamelCase() : property.Name;

        return $"public {property.Type.ToCSharpType()} {name} {{get; set;}}";
    }

    public static class OutputNode
    {
        public static string Class<T>(INode<T> dataImporterNode, bool upperCamelCaseProperties) where T : IProperty
        {
            var properties = (GenerateProperties(dataImporterNode.Properties.Cast<IProperty>().ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();
            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}

    "
                : string.Empty;

            return $@"
public class {dataImporterNode.Label}: NodeBase
{{
    {propertiesString}public {dataImporterNode.Label}()
        :base(""{dataImporterNode.Label}"") {{}}
}}";
        }
    }

    public static class OutputRelationship
    {
        public static string Class<T>(IRelationship<T> dataImporterRelationship, string startNode, string endNode, bool upperCamelCaseProperties) where T : IProperty
        {
            var type = dataImporterRelationship.Type.ToUpperCamelCase();
            var properties = (GenerateProperties(dataImporterRelationship.Properties.Cast<IProperty>().ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();

            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}

    "
                : string.Empty;

            return $@"
///<summary>
/// (:<see cref=""{startNode}""/>)-[:{dataImporterRelationship.Type}]->(:<see cref=""{endNode}""/>)
///</summary>
public class {type}: RelationshipBase
{{
    {propertiesString}public {type}()
        :base(""{dataImporterRelationship.Type}"") {{}}
}}";
        }

        public static string Class(RelationshipNormalized relationship, IDictionary<string, CypherWorkbenchNode> nodes, bool upperCamelCaseProperties)
        {
            var type = relationship.Type.ToUpperCamelCase();
            var properties = (GenerateProperties(relationship.Properties.Cast<IProperty>().ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();

            var examples = relationship.SourceAndTargets.Select(x => GenerateExamples(relationship.Type, x.SourceNode, x.TargetNode, nodes));
            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}

    "
                : string.Empty;

            return $@"
///<summary>
{string.Join(Environment.NewLine, examples)}
///</summary>
public class {type}: RelationshipBase
{{
    {propertiesString}public {type}()
        :base(""{relationship.Type}"") {{}}
}}";
        }


        private static string GenerateExamples(string type, string source, string target, IDictionary<string, CypherWorkbenchNode> nodes) 
        {
            var startNode = nodes[source]?.Label ?? "NOT SET";
            var endNode = nodes[target]?.Label ?? "NOT SET";
            return $"/// (:<see cref=\"{startNode}\"/>)-[:{type}]->(:<see cref=\"{endNode}\"/>)";
        }
    }


}