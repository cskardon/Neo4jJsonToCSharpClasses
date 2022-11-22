namespace Neo4jJsonToCSharpClasses;

using System.Text;
using Neo4jJsonToCSharpClasses.CypherWorkbench;
using Neo4jJsonToCSharpClasses.DataImporter;
using Newtonsoft.Json;

public static class Parser
{
    private static IDictionary<string, RelationshipNormalized> GetNormalizedRelationships<T, TP>(IEnumerable<T> relationships) 
        where TP: IProperty 
        where T : IRelationship<TP>
    {
        var normalizedRelationships = new Dictionary<string, RelationshipNormalized>();
        foreach (var relationship in relationships)
        {
            if (!normalizedRelationships.ContainsKey(relationship.Type.ToLowerInvariant()))
            {
                normalizedRelationships.Add(relationship.Type.ToLowerInvariant(), relationship.ToNormalized());
                continue;
            }

            normalizedRelationships[relationship.Type.ToLowerInvariant()].AddSourceTargetsAndProperties(relationship);
        }

        return normalizedRelationships;
    }

    private static StringBuilder ParseNormalizedRelationships<T, TP>(IDictionary<string, RelationshipNormalized> normalizedRelationships, IDictionary<string, T> nodes, bool useUpperCamelCaseForProperties)
        where TP: IProperty
        where T : INode<TP>
    {
        var relationshipClasses = Generate.StartRelationshipsClassFile;
        foreach (var relationshipClass in
                 normalizedRelationships.Select(normalizedRelationship => Generate.OutputRelationship.Class<T, TP>(normalizedRelationship.Value, nodes, useUpperCamelCaseForProperties)))
            relationshipClasses.Append(relationshipClass);

        return relationshipClasses;
    }

    private static StringBuilder ParseNodes<T, TP>(IDictionary<string, T> model, bool useUpperCamelCaseForProperties)
        where TP : IProperty
        where T : INode<TP>
    {
        var nodeClasses = Generate.StartNodeClassFile;
        foreach (var node in model)
        {
            var nodeClass = Generate.OutputNode.Class<T, TP>(node.Value, useUpperCamelCaseForProperties);
            nodeClasses.Append(nodeClass).AppendLine();
        }

        return nodeClasses;
    }

    private static StringBuilder ParseRelationships<TRel, TNode, TProperty>(IDictionary<string, TNode> nodes, IDictionary<string, TRel> relationships, bool useUpperCamelCaseForProperties)
        where TProperty : IProperty
        where TNode : INode<TProperty>
        where TRel : IRelationship<TProperty>
    {
        var normalizedRelationships = GetNormalizedRelationships<TRel, TProperty>(relationships.Values);
        return ParseNormalizedRelationships<TNode, TProperty>(normalizedRelationships, nodes, useUpperCamelCaseForProperties);
    }

    public static class CypherWorkbench
    {
        private static Version VersionWorksWith = new(1, 3, 0);

        public static void Parse(string contentIn, bool useUpperCamelCaseForProperties, out StringBuilder nodeClasses, out StringBuilder relationshipClasses)
        {
            var model = JsonConvert.DeserializeObject<Neo4jJsonToCSharpClasses.CypherWorkbench.CypherWorkbench>(contentIn);
            if (model == null)
                throw new InvalidDataException("CypherWorkbench: The file could not be parsed as a JSON file.");

            if (model.Metadata.Version != VersionWorksWith)
                Console.WriteLine($"This is set to work with {VersionWorksWith} of the Cypher Workbench JSON - double check results!");

            nodeClasses = ParseNodes<CypherWorkbenchNode, CypherWorkbenchProperty>(model.DataModel.Nodes, useUpperCamelCaseForProperties);
            relationshipClasses = ParseRelationships<CypherWorkbenchRelationship, CypherWorkbenchNode, CypherWorkbenchProperty>(model.DataModel.Nodes, model.DataModel.Relationships, useUpperCamelCaseForProperties);
        }
    }

    public static class DataImporter
    {
        public static void Parse(string contentIn, bool useUpperCamelCaseForProperties, out StringBuilder nodeClasses, out StringBuilder relationshipClasses)
        {
            var model = JsonConvert.DeserializeObject<Neo4jImporter>(contentIn);
            if (model == null)
                throw new InvalidDataException("DataImporter: The file could not be parsed as a JSON file.");

            nodeClasses = ParseNodes<DataImporterNode, DataImporterProperty>(model.DataModel.GraphModel.Nodes, useUpperCamelCaseForProperties);
            relationshipClasses = ParseRelationships<DataImporterRelationship, DataImporterNode, DataImporterProperty>(model.DataModel.GraphModel.Nodes, model.DataModel.GraphModel.Relationships, useUpperCamelCaseForProperties);
        }
    }
}