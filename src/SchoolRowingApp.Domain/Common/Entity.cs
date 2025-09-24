using System.ComponentModel.DataAnnotations;

namespace SchoolRowingApp.Domain.Common;

/// <summary>
/// Базовый класс для сущностей с единичным первичным ключом.
/// Позволяет использовать разные типы ключей (Guid, int и т.д.).
/// </summary>
/// <typeparam name="TId">Тип первичного ключа</typeparam>
public abstract class Entity<TId> : AuditableEntity
{
    [Key]
    public TId Id { get; protected set; }
}
