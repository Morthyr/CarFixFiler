using Microsoft.EntityFrameworkCore;

namespace CarFixFiler.Data;

[PrimaryKey(nameof(LicensePlateNumber), nameof(Date))]
[Index(nameof(Date), AllDescending = true)]
public class Service
{
    public string LicensePlateNumber { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }

    public Car Car { get; set; }
    public IEnumerable<ServiceItem> ServiceItems { get; set; }
    
}
