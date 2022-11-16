using System.Text;
using EnsureThat;
using Neo4jDataImporterToCSharpClasses;
using Newtonsoft.Json;

string filename = null!;
string folderOut = null!;

if (!ParseArgs(args))
    return;

if (!Directory.Exists(folderOut))
    Directory.CreateDirectory(folderOut);

var contentIn = await ReadFile(filename);
var model = JsonConvert.DeserializeObject<Neo4jImporter>(contentIn);

var nodeClasses = new StringBuilder(Generate.BaseNodeClass + Environment.NewLine);
var relationshipClasses = new StringBuilder(Generate.BaseRelationshipClass + Environment.NewLine);

foreach (var nodeSchema in model.DataModel.GraphModel.NodeSchemas)
{
    string nodeClass = Generate.Node.Class(nodeSchema.Value);
    nodeClasses.Append(nodeClass).AppendLine();
}

foreach (var relationshipSchema in model.DataModel.GraphModel.RelationshipSchemas)
{
    var startNode = model.DataModel.GraphModel.NodeSchemas[relationshipSchema.Value.SourceNode]?.Label ?? "NOT SET";
    var endNode = model.DataModel.GraphModel.NodeSchemas[relationshipSchema.Value.TargetNode]?.Label ?? "NOT SET";
    string relationshipClass = Generate.Relationship.Class(relationshipSchema.Value, startNode, endNode);
    relationshipClasses.Append(relationshipClass).AppendLine();
}

await WriteFile(nodeClasses.ToString(), folderOut, true);
await WriteFile(relationshipClasses.ToString(), folderOut, false);


async Task WriteFile(string content, string folder, bool isNode)
{
    var filename = isNode ? "Nodes.cs" : "Relationships.cs";
    await File.WriteAllTextAsync(Path.Combine(folder, filename), content);
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
