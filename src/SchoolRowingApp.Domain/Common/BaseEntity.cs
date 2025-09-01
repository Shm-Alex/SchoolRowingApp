using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRowingApp.Domain.Common;

public abstract class BaseEntity
{
	// This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
	// Using non-generic integer types for simplicity
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();

	public DateTime Created { get; set; } = DateTime.UtcNow;
	public DateTime? LastModified { get; set; }

	private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
