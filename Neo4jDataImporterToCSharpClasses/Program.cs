using System.Text;
using EnsureThat;
using Neo4jJsonToCSharpClasses;

string filename = null!, folderOut = null!, format = null!;
var useUpperCamelCaseForProperties = true;

if (!ParseArgs(args))
    return;

if (!Directory.Exists(folderOut))
    Directory.CreateDirectory(folderOut);

var contentIn = await ReadFile();
StringBuilder nodeClasses, relationshipClasses;

switch (format.ToLowerInvariant())
{
    case "di":
    case "dataimporter":
        Parser.DataImporter.Parse(contentIn, useUpperCamelCaseForProperties, out nodeClasses, out relationshipClasses);
        break;
    case "cw":
    case "cypherworkbench":
        Parser.CypherWorkbench.Parse(contentIn, useUpperCamelCaseForProperties, out nodeClasses, out relationshipClasses);
        break;
    case "ar":
    case "arrows":
    default: 
        throw new ArgumentOutOfRangeException(nameof(format), format, $"Unsupported format '{format}'!");
}

await WriteFile(nodeClasses.ToString(), folderOut, true);
await WriteFile(relationshipClasses.ToString(), folderOut, false);

async Task WriteFile(string content, string folder, bool isNode)
{
    var filenameOut = isNode ? "Nodes.cs" : "Relationships.cs";
    await File.WriteAllTextAsync(Path.Combine(folder, filenameOut), content);
}

bool ParseArgs(string[] args)
{
    for (var i = 0; i < args.Length; i++)
        switch (args[i].ToLowerInvariant())
        {
            case "--filein":
                filename = args[++i];
                break;
            case "--folderout":
                folderOut = args[++i];
                break;
            case "--format":
                format = args[++i];
                break;
            case "--ucc":
            case "--upperCamelCaseProperties":
                bool.TryParse(args[++i], out useUpperCamelCaseForProperties);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args), args[i], $"Unknown parameter '{args[i]}'");
        }

    return !string.IsNullOrWhiteSpace(filename) && !string.IsNullOrWhiteSpace(folderOut);
}

async Task<string> ReadFile()
{
    Ensure.That(filename).IsNotNullOrWhiteSpace();
    if (!File.Exists(filename))
        throw new FileNotFoundException("File not found.", filename);

    return await File.ReadAllTextAsync(filename);
}