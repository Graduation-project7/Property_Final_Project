using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    protected AuditableEntity(int id) : base(id)
    {

    }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

}
