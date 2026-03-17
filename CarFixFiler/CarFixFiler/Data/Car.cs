using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarFixFiler.Data;

public class Car
{
    [Key]
    public string LicensePlateNumber { get; set; }
    public string CustomerName { get; set; }
    public string Vin { get; set; }
    public string EngineCode { get; set; }
    public int Power { get; set; }
    public int EngineDisplacement { get; set; }
    public int ManufatureYear { get; set; }
    public int Mileage { get; set; }

    public Customer Customer { get; set; }
    public IEnumerable<Service> Services { get; set; }
}
