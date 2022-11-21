using Neo4jJsonToCSharpClasses.DataImporter;

namespace Neo4jJsonToCSharpClasses;

using System.Reflection.Metadata.Ecma335;

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

    internal static class OutputNode
    {
        internal static string Class(DataImporterNode dataImporterNode)
        {
            var properties = (GenerateProperties(dataImporterNode.Properties.Cast<IProperty>().ToList()) ?? Array.Empty<string>()).ToList();
            var propertiesString = (properties.Any()) ? 
                $@"{string.Join($"{Environment.NewLine}    ", properties)}

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
    
    internal static class OutputRelationship
    {
        internal static string Class(DataImporterRelationship dataImporterRelationship, string startNode, string endNode)
        {
            var type = dataImporterRelationship.Type.ToUpperCamelCase();
            var properties = (GenerateProperties(dataImporterRelationship.Properties.Cast<IProperty>().ToList()) ?? Array.Empty<string>()).ToList();

            var propertiesString = (properties.Any()) ? 
                $@"{string.Join($"{Environment.NewLine}    ", properties)}

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
    }
    
    private static IEnumerable<string>? GenerateProperties(ICollection<IProperty>? properties)
    {
        var output = new List<string>();
        foreach (var property in properties)
        {
            output.Add(GenerateProperty(property));
        }

        return output;
    }

    private static string GenerateProperty(IProperty property)
    {
        return $"public {property.Type.ToCSharpType()} {property.Name} {{get; set;}}";
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