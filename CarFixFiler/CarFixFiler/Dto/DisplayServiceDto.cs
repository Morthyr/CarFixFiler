namespace CarFixFiler.Dto;

public class DisplayServiceDto
{
    public string CustomerName { get; set; }
    public string CustomerTelephone { get; set; }

    public string LicensePlateNumber { get; set; }
    public DateTime ServiceDate { get; set; }
    public string ServiceDescription { get; set; }
    public int ServiceCost { get; set; }
}
