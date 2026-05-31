using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarFixFiler.Data;

public class ServiceItem
{
    public string LicensePlateNumber { get; set; }
    public DateTime Date { get; set; }
    public string ItemName { get; set; }
    public string ProductNumber { get; set; }
    public int? PurchasePrice { get; set; }
    public int SellingPrice { get; set; }
    public int Amount { get; set; }

    public Service Service { get; set; }
}
