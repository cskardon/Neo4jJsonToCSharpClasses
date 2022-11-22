namespace Neo4jJsonToCSharpClasses;

using EnsureThat;

public interface IRelationship<TProperty> 
    where TProperty : IProperty
{
    string? Type { get; }
    
    string? SourceNode { get; }
    
    string? TargetNode { get; }
    
    ICollection<TProperty>? Properties { get; }
}

public class RelationshipSourceAndTarget {

    public RelationshipSourceAndTarget(string source, string target)
    {
        SourceNode = source;
        TargetNode = target;
    }

    public string? SourceNode { get;  }
    
    public string? TargetNode { get; }
}


public class RelationshipNormalized
{
    public string? Type { get; set; }
    public List<RelationshipSourceAndTarget> SourceAndTargets { get; } = new List<RelationshipSourceAndTarget>();
    public ICollection<IProperty> Properties { get; } = new List<IProperty>();

    public void AddSourceTargetsAndProperties<T>(IRelationship<T> relationship) 
        where T:IProperty
    {
        AddSourceTargetsAndProperties(relationship.SourceNode, relationship.TargetNode, Properties);
    }

    public void AddSourceTargetsAndProperties(string source, string target, IEnumerable<IProperty> properties)
    {
        Ensure.That(source).IsNotNullOrWhiteSpace();
        Ensure.That(target).IsNotNullOrWhiteSpace();
        Ensure.That(properties).IsNotNull();

        SourceAndTargets.Add(new RelationshipSourceAndTarget(source, target));
        foreach (var property in properties)
        {
            var existing = Properties.SingleOrDefault(x => string.Equals(x.Name, property.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null)
            {
                //Check type
                if (!string.Equals(existing.Type, property.Type, StringComparison.InvariantCultureIgnoreCase))
                    throw new DataMisalignedException($"Property {existing.Name} is defined multiple times, but with different types (in this instance {existing.Type} and {property.Type}), this isn't allowed.");
            }
            else
            {
                Properties.Add(property);
            }
        }
    }
}