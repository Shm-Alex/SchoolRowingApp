using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRowingApp.Domain.Common;

/// <summary>
/// Базовый класс для всех сущностей, содержащий аудит и доменные события.
/// Не содержит первичного ключа, что позволяет гибко определять ключ в наследниках.
/// </summary>
public abstract class AuditableEntity
{
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
