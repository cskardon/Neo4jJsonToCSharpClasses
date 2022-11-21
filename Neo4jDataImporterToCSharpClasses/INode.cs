namespace Neo4jJsonToCSharpClasses;

public interface INode<T> where T:IProperty
{
    string? Label { get; }
    ICollection<T>? Properties { get; }
}