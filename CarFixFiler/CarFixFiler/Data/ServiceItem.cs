using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarFixFiler.Data;

public class ServiceItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string LicensePlateNumber { get; set; }
    public DateTime Date { get; set; }
    public string ItemName { get; set; }
    public string ProductNumber { get; set; }
    public int? PurchasePrice { get; set; }
    public int SellingPrice { get; set; }
    public int Amount { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
}
