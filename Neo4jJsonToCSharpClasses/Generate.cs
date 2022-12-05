namespace Neo4jJsonToCSharpClasses;

using System.Text;

public static class Generate
{
    public static StringBuilder StartNodeClassFile => new StringBuilder(/*BaseNodeClass*/);//.AppendLine();
    public static StringBuilder StartRelationshipsClassFile => new StringBuilder(/*BaseRelationshipClass*/);//.AppendLine();

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
        public static string Class(NormalizedNode node, bool upperCamelCaseProperties)
        {
            var className = $"{node.Label.ToUpperCamelCase()}Node";
            var properties = (GenerateProperties(node.Properties.ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();
            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}"
                : string.Empty;

            return $@"
public class {className}
{{
    public const string Labels = ""{node.Label}"";
    {propertiesString}
}}";
        }
    }

    public static class OutputRelationship
    {
        public static StringBuilder Consts<TNode, TProperty>(ICollection<RelationshipNormalized> relationships, IDictionary<string, TNode> nodes)
            where TProperty : IProperty
            where TNode : INode<TProperty>
        {
            var output = new StringBuilder("public static class RelationshipTypes").AppendLine().AppendLine("{");

            foreach (var relationship in relationships)
            {
                var type = $"{relationship.Type.ToUpperCamelCase()}";
                var examples = relationship.SourceAndTargets.Select(x => GenerateExamples<TNode, TProperty>(relationship.Type, x.SourceNode, x.TargetNode, nodes, true));
                output.AppendLine($"    ///<summary>").AppendLine(string.Join(Environment.NewLine, examples)).AppendLine("    ///</summary>");
                output.AppendLine($"    public const string {type} = \"{relationship.Type}\";").AppendLine();
            }

            return output.Append("}");
        }

        public static string Class<TNode, TProperty>(RelationshipNormalized relationship, IDictionary<string, TNode> nodes, bool upperCamelCaseProperties)
            where TProperty : IProperty
            where TNode : INode<TProperty>
        {
            var type = $"{relationship.Type.ToUpperCamelCase()}Relationship";
            var properties = (GenerateProperties(relationship.Properties.ToList(), upperCamelCaseProperties) ?? Array.Empty<string>()).ToList();

            var examples = relationship.SourceAndTargets.Select(x => GenerateExamples<TNode, TProperty>(relationship.Type, x.SourceNode, x.TargetNode, nodes));
            var propertiesString = properties.Any()
                ? $@"{string.Join($"{Environment.NewLine}    ", properties)}"
                : string.Empty;

            return $@"
///<summary>
{string.Join(Environment.NewLine, examples)}
///</summary>
public class {type}
{{
    public const string Type = ""{relationship.Type}"";
    {propertiesString}
}}";
        }

        private static string GenerateExamples<TNode, TProperty>(string type, string source, string target, IDictionary<string, TNode> nodes, bool addTab = false)
            where TProperty : IProperty
            where TNode : INode<TProperty>
        {
            var startNode = nodes[source]?.Label ?? "NOT SET";
            var endNode = nodes[target]?.Label ?? "NOT SET";
            return $"{(addTab?"    ":string.Empty)}/// (:<see cref=\"{startNode}\"/>)-[:{type}]->(:<see cref=\"{endNode}\"/>)";
        }
    }
}