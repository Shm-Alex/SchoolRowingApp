using System;

namespace SchoolRowingApp.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}
