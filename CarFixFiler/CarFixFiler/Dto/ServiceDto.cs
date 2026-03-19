using System.ComponentModel.DataAnnotations;

namespace CarFixFiler.Dto;

public class ServiceDto
{
    [Required]
    public string CustomerName { get; set; }
    public string CustomerTelephone { get; set; }
    public int LaborCostDiscount { get; set; }
    public int PartCostDiscount { get; set; }
    [Required]
    public string LicensePlateNumber { get; set; }
    public string Vin { get; set; }
    public string EngineCode { get; set; }
    public int Power { get; set; }
    public int EngineDisplacement { get; set; }
    public int ManufatureYear { get; set; }
    public int Mileage { get; set; }
    [Required]
    public DateTime ServiceDate { get; set; }
    public string ServiceDescription { get; set; }
    public int ServiceCost { get; set; }

    public List<ServiceItemDto> ServiceItems { get; set; } = [];
}
