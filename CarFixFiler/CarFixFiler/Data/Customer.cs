using System.ComponentModel.DataAnnotations;

namespace CarFixFiler.Data;

public class Customer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; }
    public string Telephone { get; set; }
    public int LaborCostDiscount { get; set; }
    public int PartCostDiscount { get; set; }

    public IEnumerable<Car> Cars { get; set; }
}
