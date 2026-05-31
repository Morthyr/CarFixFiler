using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarFixFiler.Data;

[Index(nameof(Date), AllDescending = true)]
public class Service
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string LicensePlateNumber { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }

    public Guid CarId { get; set; }
    public Car Car { get; set; }
    public IEnumerable<ServiceItem> ServiceItems { get; set; }
}
