namespace Neo4jJsonToCSharpClasses;

internal interface IProperty
{
    string Name { get; }
    string Type { get; }
    string Id { get; }
}