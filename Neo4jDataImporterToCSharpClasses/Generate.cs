namespace Neo4jJsonToCSharpClasses;

using System.Text;

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

    public static StringBuilder StartNodeClassFile => new StringBuilder(BaseNodeClass).AppendLine();
    public static StringBuilder StartRelationshipsClassFile => new StringBuilder(BaseRelationshipClass).AppendLine();

    private static IEnumerable<string>? GenerateProperties(IEnumerable<IProperty> properties, bool upperCamelCaseProperties)
    {
        return properties?.Select(property => GenerateProperty(property, upperCamelCaseProperties)).ToList();
    }

    private static string GenerateProperty(IProperty property, bool upperCamelCaseProperty)
    {
        var name = upperCamelCaseProperty ? property.Name.ToUpperCamelCase() : property.Name;
        return $"public {property.Type.ToCSharpType()} {name} {{get; set;}}";
    }

    public static class OutputNode
    {
        public static string Class<T, TP>(T node, bool upperCamelCaseProperties)
            where TP : IProperty
            where T : INode<TP>
        {
            var properties = (GenerateProperties(node.Properties.Cast<IProperty>().ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();
            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}

    "
                : string.Empty;

            return $@"
public class {node.Label}: NodeBase
{{
    {propertiesString}public {node.Label}()
        :base(""{node.Label}"") {{}}
}}";
        }
    }

    public static class OutputRelationship
    {
        public static string Class<T, TP>(RelationshipNormalized relationship, IDictionary<string, T> nodes, bool upperCamelCaseProperties)
            where TP : IProperty
            where T : INode<TP>
        {
            var type = relationship.Type.ToUpperCamelCase();
            var properties = (GenerateProperties(relationship.Properties.ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();

            var examples = relationship.SourceAndTargets.Select(x => GenerateExamplesX<T, TP>(relationship.Type, x.SourceNode, x.TargetNode, nodes));
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

        private static string GenerateExamplesX<T, TP>(string type, string source, string target, IDictionary<string, T> nodes)
            where TP : IProperty
            where T : INode<TP>
        {
            var startNode = nodes[source]?.Label ?? "NOT SET";
            var endNode = nodes[target]?.Label ?? "NOT SET";
            return $"/// (:<see cref=\"{startNode}\"/>)-[:{type}]->(:<see cref=\"{endNode}\"/>)";
        }
    }
}