using System.Collections.Generic;

namespace SchoolRowingApp.Domain.Common;
/// <summary>
/// Конкретная реализация для сущностей с GUID-ключом.
/// </summary>
public abstract class GuidEntity : Entity<Guid>
{
    protected GuidEntity()
    {
        Id = Guid.NewGuid();
    }
}
