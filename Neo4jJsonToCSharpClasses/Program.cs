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
StringBuilder nodeClasses, relationshipClasses, constClass;

switch (format.ToLowerInvariant())
{
    case "di":
    case "dataimporter":
        Parser.DataImporter.Parse(contentIn, useUpperCamelCaseForProperties, out nodeClasses, out relationshipClasses, out constClass);
        break;
    case "cw":
    case "cypherworkbench":
        Parser.CypherWorkbench.Parse(contentIn, useUpperCamelCaseForProperties, out nodeClasses, out relationshipClasses, out constClass);
        break;
    case "ar":
    case "arrows":
        Console.WriteLine("**NOTE FOR ARROWS FILES**");
        Console.WriteLine("\t* This makes the assumption properties are defined as 'name:Type', if you have this the other way around this version will break!");
        Console.WriteLine("\t* This uses 'captions' for labels in this version.");
        Parser.Arrows.Parse(contentIn,useUpperCamelCaseForProperties,out nodeClasses,out relationshipClasses, out constClass);
        break;
    default: 
        throw new ArgumentOutOfRangeException(nameof(format), format, $"Unsupported format '{format}'!");
}

await WriteFile(nodeClasses.ToString(), folderOut, "Nodes.cs");
await WriteFile(relationshipClasses.ToString(), folderOut, "Relationships.cs");
await WriteFile(constClass.ToString(), folderOut, "RelationshipTypes.cs");

async Task WriteFile(string content, string folder, string filenameOut)
{
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