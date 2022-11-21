namespace Neo4jJsonToCSharpClasses;

internal interface INode<T> where T:IProperty
{
    string? Label { get; }
    ICollection<T>? Properties { get; }
}