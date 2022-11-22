# Neo4j Data Importer To C# Classes

Takes a `JSON` file generated from the [Neo4j Data Importer](http://data-importer.graphapp.io/) and creates:

* `Nodes.cs` 
* `Relationships.cs`

files containing C# POCO (**P**lain **O**ld **C**LR **O**bjects) for each of the Nodes and Relationships defined in the model.

This should work on Windows, Linux and Mac as long as you have the DotNet 6.0 SDK installed.

# Usage

Just some blurb about how to use the app.

## Arguments

* --format <FILE TYPE>
  * `--format dataImporter` will attempt to parse a Data Importer generated JSON file (alt: `--format di`)
  * `--format cypherworkbench` will attempt to parse a Cypher Workbench generated JSON file (alt: `--format cw`)
* --useCamelCaseProperties <TRUE|FALSE>
  * if `true` (default) the parser will output classes with properties using Upper Camel Case - the standard C# code guidelines
  * if `false` the parser will output classes with the _same_ case as the `fileIn` file
* --fileIn <FILE>
  * the JSON file to read, probably best to wrap with `"` or `'`
* --folderOut <FILE>
  * the location to output the generated classes, probably best to wrap with `"` or `'`
    * `Nodes.cs` contains the node classes
    * `Relationships.cs` contains the relationship classes.
  * If this folder doesn't exist - it will be created.

## Example

```
.\Neo4jJsonToCSharpClasses.exe --format di --fileIn "D:\temp\test-importer-model.json" --folderOut "D:\temp\generated\"
```