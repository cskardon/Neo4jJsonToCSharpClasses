using System.Text;
using EnsureThat;
using Neo4jJsonToCSharpClasses;
using Neo4jJsonToCSharpClasses.CypherWorkbench;
using Neo4jJsonToCSharpClasses.DataImporter;
using Newtonsoft.Json;

string filename = null!;
string folderOut = null!;
string format = null!;
bool useUpperCamelCaseForProperties = true;

if (!ParseArgs(args))
    return;

if (!Directory.Exists(folderOut))
    Directory.CreateDirectory(folderOut);

var contentIn = await ReadFile(filename);

var nodeClasses = new StringBuilder(Generate.BaseNodeClass + Environment.NewLine);
var relationshipClasses = new StringBuilder(Generate.BaseRelationshipClass + Environment.NewLine);

switch (format.ToLowerInvariant())
{
    case "di":
    case "dataimporter":
        ParseDataImporter();
        break;
    case "cw":
    case "cypherworkbench":
        ParseCypherWorkbench();
        break;
    default: throw new ArgumentOutOfRangeException(nameof(format), format, $"Unsupported format '{format}'!");
}

await WriteFile(nodeClasses.ToString(), folderOut, true);
await WriteFile(relationshipClasses.ToString(), folderOut, false);




async Task WriteFile(string content, string folder, bool isNode)
{
    var filename = isNode ? "Nodes.cs" : "Relationships.cs";
    await File.WriteAllTextAsync(Path.Combine(folder, filename), content);
}

void ParseDataImporter()
{
    var model = JsonConvert.DeserializeObject<Neo4jImporter>(contentIn);

    foreach (var nodeSchema in model.DataModel.GraphModel.Nodes)
    {
        string nodeClass = Generate.OutputNode.Class(nodeSchema.Value, useUpperCamelCaseForProperties);
        nodeClasses.Append(nodeClass).AppendLine();
    }

    foreach (var relationshipSchema in model.DataModel.GraphModel.Relationships)
    {
        var startNode = model.DataModel.GraphModel.Nodes[relationshipSchema.Value.SourceNode]?.Label ?? "NOT SET";
        var endNode = model.DataModel.GraphModel.Nodes[relationshipSchema.Value.TargetNode]?.Label ?? "NOT SET";
        string relationshipClass = Generate.OutputRelationship.Class(relationshipSchema.Value, startNode, endNode, useUpperCamelCaseForProperties);
        relationshipClasses.Append(relationshipClass).AppendLine();
    }
}

void ParseCypherWorkbench()
{
    var model = JsonConvert.DeserializeObject<CypherWorkbench>(contentIn);
    foreach (var node in model.DataModel.Nodes)
    {
        string nodeClass = Generate.OutputNode.Class(node.Value, useUpperCamelCaseForProperties);
        nodeClasses.Append(nodeClass).AppendLine();
    }

    var normalizedRelationships = new Dictionary<string, RelationshipNormalized>();
    foreach (var relationship in model.DataModel.Relationships)
    {
        if (!normalizedRelationships.ContainsKey(relationship.Value.Type.ToLowerInvariant()))
        {
            normalizedRelationships.Add(relationship.Value.Type.ToLowerInvariant(), relationship.Value.ToNormalized());
            continue;
        }
        
        normalizedRelationships[relationship.Value.Type.ToLowerInvariant()].AddSourceTargetsAndProperties(relationship.Value);
    }

    foreach (var relationshipClass in 
             normalizedRelationships.Select(normalizedRelationship => Generate.OutputRelationship.Class(normalizedRelationship.Value, model.DataModel.Nodes, useUpperCamelCaseForProperties)))
    {
        relationshipClasses.Append(relationshipClass);
    }
}

bool ParseArgs(string[] args)
{
    for (int i = 0; i < args.Length; i++)
    {
        switch (args[i].ToLowerInvariant())
        {
            case "--filein":
                filename = args[++i]; break;
            case "--folderout":
                folderOut = args[++i]; break;
            case "--format":
                format = args[++i]; break;
            case "--ucc":
            case "--upperCamelCaseProperties":
                bool.TryParse(args[++i], out useUpperCamelCaseForProperties);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args), args[i], $"Unknown parameter '{args[i]}'");
        }
    }

    return !string.IsNullOrWhiteSpace(filename) && !string.IsNullOrWhiteSpace(folderOut);
}

async Task<string> ReadFile(string filename)
{
    Ensure.That(filename).IsNotNullOrWhiteSpace();
    if(!File.Exists(filename))
        throw new FileNotFoundException("File not found.", filename);

    return await File.ReadAllTextAsync(filename);
}