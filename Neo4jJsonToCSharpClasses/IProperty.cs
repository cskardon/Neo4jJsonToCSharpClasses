namespace Neo4jJsonToCSharpClasses;

public interface IProperty
{
    string? Name { get; }
    string? Type { get; }
    string? Id { get; }
}