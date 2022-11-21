namespace Neo4jJsonToCSharpClasses;

internal interface IRelationship<TProperty> 
    where TProperty : IProperty
{
    string? Type { get; }
    
    string? SourceNode { get; }
    
    string? TargetNode { get; }
    
    ICollection<TProperty>? Properties { get; }
}