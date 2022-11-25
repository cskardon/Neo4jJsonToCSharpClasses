namespace Neo4jJsonToCSharpClasses;

public abstract class BaseNode<T> : INode<T> 
    where T : IProperty
{
    /// <inheritdoc />
    public abstract string? Label { get; set; }

    /// <inheritdoc />
    public abstract ICollection<T>? Properties { get; }
}


public interface INode<T>
    where T : IProperty
{
    string? Label { get; }
    ICollection<T>? Properties { get; }
}