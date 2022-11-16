namespace Neo4jDataImporterToCSharpClasses;

internal static class Generate
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

    internal static class Node
    {
        internal static string Class(NodeSchema node)
        {
            var properties = GenerateProperties(node.Properties).ToList();
            var propertiesString = (properties.Any()) ? 
                $@"{string.Join($"{Environment.NewLine}    ", properties)}

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
    
    internal static class Relationship
    {
        internal static string Class(RelationshipSchema relationship, string startNode, string endNode)
        {
            var type = relationship.Type.ToUpperCamelCase();
            var properties = GenerateProperties(relationship.Properties).ToList();

            var propertiesString = (properties.Any()) ? 
                $@"{string.Join($"{Environment.NewLine}    ", properties)}

    " 
                : string.Empty;

            return $@"
///<summary>
/// (:<see cref=""{startNode}""/>)-[:{relationship.Type}]->(:<see cref=""{endNode}""/>)
///</summary>
public class {type}: RelationshipBase
{{
    {propertiesString}public {type}()
        :base(""{relationship.Type}"") {{}}
}}";
        }
    }
    
    private static IEnumerable<string> GenerateProperties(ICollection<Property> properties)
    {
        var output = new List<string>();
        foreach (var property in properties)
        {
            output.Add(GenerateProperty(property));
        }

        return output;
    }

    private static string GenerateProperty(Property property)
    {
        return $"public {JavaTypeToCSharpType(property.Type)} {property.Name} {{get; set;}}";
    }

    private static string JavaTypeToCSharpType(string type)
    {
        switch (type.ToLowerInvariant())
        {
            case "string": return "string";
            case "integer": return "int";
            case "float": return "float";
            case "boolean": return "bool";
            case "datetime": return "DateTime";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, $"Unknown type '{type}' to parse.");
        }
    }
}

public static class StringExtensions
{
    public static string ToUpperCamelCase(this string value)
    {
        var split = value.Split("_ ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        split = split.Select(x => x.ToLowerInvariant()).Select(x => char.ToUpperInvariant(x[0]) + x.Substring(1)).ToArray();

        return string.Join(string.Empty, split);
    }
}