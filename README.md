# Neo4j Data Importer To C# Classes

Takes a `JSON` file generated from the [Neo4j Data Importer](http://data-importer.graphapp.io/) and creates:

* `Nodes.cs` 
* `Relationships.cs`

files containing C# POCO (**P**lain **O**ld **C**LR **O**bjects) for each of the Nodes and Relationships defined in the model.

# Usage

```
.\Neo4jDataImporterToCSharpClasses.exe --fileIn D:\temp\test-importer-model.json --folderOut D:\temp\generated\
```

If the `folderOut` folder doesn't exist - it will try to create it.