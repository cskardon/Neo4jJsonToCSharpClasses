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
                 normalizedRelationships.Select(normalizedRelationship => Generate.OutputRelationship.ClassX<T, TP>(normalizedRelationship.Value, nodes, useUpperCamelCaseForProperties)))
            relationshipClasses.Append(relationshipClass);

        return relationshipClasses;
    }


    public static class CypherWorkbench
    {
        public static void Parse(string contentIn, bool useUpperCamelCaseForProperties, out StringBuilder nodeClasses, out StringBuilder relationshipClasses)
        {
            var model = JsonConvert.DeserializeObject<Neo4jJsonToCSharpClasses.CypherWorkbench.CypherWorkbench>(contentIn);
            if (model == null)
                throw new InvalidDataException("The file could not be parsed as a JSON file.");

            nodeClasses = ParseNodes(model, useUpperCamelCaseForProperties);
            relationshipClasses = ParseRelationships(model, useUpperCamelCaseForProperties);
        }

        private static StringBuilder ParseNodes(Neo4jJsonToCSharpClasses.CypherWorkbench.CypherWorkbench model, bool useUpperCamelCaseForProperties)
        {
            var nodeClasses = Generate.StartNodeClassFile;
            foreach (var node in model!.DataModel!.Nodes!)
            {
                var nodeClass = Generate.OutputNode.Class(node.Value, useUpperCamelCaseForProperties);
                nodeClasses.Append(nodeClass).AppendLine();
            }

            return nodeClasses;
        }

        private static StringBuilder ParseRelationships(Neo4jJsonToCSharpClasses.CypherWorkbench.CypherWorkbench model, bool useUpperCamelCaseForProperties)
        {
            var normalizedRelationships = GetNormalizedRelationships<CypherWorkbenchRelationship, CypherWorkbenchProperty>(model.DataModel.Relationships.Values);
            return ParseNormalizedRelationships<CypherWorkbenchNode, CypherWorkbenchProperty>(normalizedRelationships, model.DataModel.Nodes, useUpperCamelCaseForProperties);
        }
    }

    public static class DataImporter
    {
        public static void Parse(string contentIn, bool useUpperCamelCaseForProperties, out StringBuilder nodeClasses, out StringBuilder relationshipClasses)
        {
            var model = JsonConvert.DeserializeObject<Neo4jImporter>(contentIn);
            
            nodeClasses = ParseNodes(model, useUpperCamelCaseForProperties);
            relationshipClasses = ParseRelationships(model, useUpperCamelCaseForProperties);
        }

        private static StringBuilder ParseNodes(Neo4jImporter model, bool useUpperCamelCaseForProperties)
        {
            var nodeClasses = Generate.StartNodeClassFile;
            foreach (var nodeSchema in model.DataModel.GraphModel.Nodes)
            {
                string nodeClass = Generate.OutputNode.Class(nodeSchema.Value, useUpperCamelCaseForProperties);
                nodeClasses.Append(nodeClass).AppendLine();
            }

            return nodeClasses;
        }

        private static StringBuilder ParseRelationships(Neo4jImporter model, bool useUpperCamelCaseForProperties)
        {
            var relationshipClasses = Generate.StartRelationshipsClassFile;
            foreach (var relationshipSchema in model.DataModel.GraphModel.Relationships)
            {
                var startNode = model.DataModel.GraphModel.Nodes[relationshipSchema.Value.SourceNode]?.Label ?? "NOT SET";
                var endNode = model.DataModel.GraphModel.Nodes[relationshipSchema.Value.TargetNode]?.Label ?? "NOT SET";
                string relationshipClass = Generate.OutputRelationship.Class<DataImporterRelationship, DataImporterProperty>(relationshipSchema.Value, startNode, endNode, useUpperCamelCaseForProperties);
                relationshipClasses.Append(relationshipClass).AppendLine();
            }

            return relationshipClasses;
        }
    }
}