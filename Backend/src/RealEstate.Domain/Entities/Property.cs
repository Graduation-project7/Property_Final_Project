using RealEstate.Domain.Common;
using RealEstate.Domain.Enums;


namespace RealEstate.Domain.Entities;

public class Property : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PropertyType PropertyType { get; set; }
    public ListingType ListingType { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Property() { }

    public ICollection<PropertyImg> PropertyImgs { get; set; } = new List<PropertyImg>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
