using RealEstate.Domain.Common;


namespace RealEstate.Domain.Entities;

public class PropertyImg : AuditableEntity
{
    public string ImgUrl { get;  set; } = string.Empty;

    public int PropertyId { get; set; }

    public bool IsPrimary { get; set; } 

    public Property Property { get; set; }

    public PropertyImg()
    {
        
    }
}
