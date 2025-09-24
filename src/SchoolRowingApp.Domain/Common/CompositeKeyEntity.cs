namespace SchoolRowingApp.Domain.Common;

/// <summary>
/// Базовый класс для сущностей с составным первичным ключом.
/// </summary>
public abstract class CompositeKeyEntity : AuditableEntity
{
    // Нет единого Id, ключ определяется в конкретных сущностях
}