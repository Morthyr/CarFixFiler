using CarFixFiler.Dto;
using Microsoft.EntityFrameworkCore;

namespace CarFixFiler.Services;

public class ServiceService : IServiceService
{
    private readonly ILogger<ServiceService> _logger;
    private readonly Data.CarFixFilerContext _context;

    public ServiceService(Data.CarFixFilerContext context, ILogger<ServiceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<DisplayServiceDto>> GetServicesAsync() => await _context.Services
        .Include(s => s.Car).ThenInclude(c => c.Customer)
        .Include(s => s.ServiceItems)
        .OrderByDescending(s => s.Date)
        .Select(s => new DisplayServiceDto
        {
            CustomerName = s.Car.CustomerName,
            CustomerTelephone = s.Car.Customer.Telephone,
            LicensePlateNumber = s.LicensePlateNumber,
            ServiceCost = s.Cost,
            ServiceDate = s.Date,
            ServiceDescription = s.Description
        }).ToListAsync();

    public async Task<ServiceDto> GetServiceAsync(string licensePlateNumber, DateTime date)
    {
        var service = await _context.Services
            .Include(s => s.Car).ThenInclude(c => c.Customer)
            .Include(s => s.ServiceItems)
            .FirstOrDefaultAsync(s => s.LicensePlateNumber == licensePlateNumber && s.Date == date);

        if (service == null)
        {
            return null;
        }

        return new ServiceDto
        {
            CustomerName = service.Car.CustomerName,
            CustomerTelephone = service.Car.Customer.Telephone,
            LaborCostDiscount = service.Car.Customer.LaborCostDiscount,
            PartCostDiscount = service.Car.Customer.PartCostDiscount,

            LicensePlateNumber = service.Car.LicensePlateNumber,
            Vin = service.Car.Vin,
            EngineCode = service.Car.EngineCode,
            Power = service.Car.Power,
            EngineDisplacement = service.Car.EngineDisplacement,
            ManufatureYear = service.Car.ManufatureYear,
            Mileage = service.Car.Mileage,

            ServiceDate = service.Date,
            ServiceDescription = service.Description,
            ServiceCost = service.Cost,

            ServiceItems = service.ServiceItems.Select(si => new ServiceItemDto
            {
                ItemName = si.ItemName,
                Amount = si.Amount,
                PurchasePrice = si.PurchasePrice,
                SellingPrice = si.SellingPrice
            }).ToList()
        };
    }

    public async Task UpdateServiceAsync(ServiceDto service)
    {
        // Query for existing customer by business key (Name) using indexed column
        Data.Customer customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Name == service.CustomerName)
            ?? new Data.Customer { Name = service.CustomerName };
        
        customer.Telephone = service.CustomerTelephone;
        customer.LaborCostDiscount = service.LaborCostDiscount;
        customer.PartCostDiscount = service.PartCostDiscount;

        _context.Update(customer);

        // Query for existing car by business key (LicensePlateNumber) using indexed column
        Data.Car car = await _context.Car
            .FirstOrDefaultAsync(c => c.LicensePlateNumber == service.LicensePlateNumber)
            ?? new Data.Car { LicensePlateNumber = service.LicensePlateNumber };
        
        car.CustomerName = service.CustomerName;
        car.Vin = service.Vin;
        car.EngineCode = service.EngineCode;
        car.Power = service.Power;
        car.EngineDisplacement = service.EngineDisplacement;
        car.ManufatureYear = service.ManufatureYear;
        car.Mileage = service.Mileage;

        _context.Update(car);

        // Query for existing service by composite business key (LicensePlateNumber, Date) using indexed columns
        Data.Service serv = await _context.Services
            .FirstOrDefaultAsync(s => s.LicensePlateNumber == service.LicensePlateNumber && s.Date == service.ServiceDate)
            ?? new Data.Service { LicensePlateNumber = service.LicensePlateNumber, Date = service.ServiceDate };
        
        serv.Description = service.ServiceDescription;
        serv.Cost = service.ServiceCost;

        _context.Update(serv);

        // Query for existing service items by composite business key using indexed columns
        foreach (var serviceItem in service.ServiceItems)
        {
            Data.ServiceItem servItem = await _context.ServiceItems
                .FirstOrDefaultAsync(si => si.LicensePlateNumber == service.LicensePlateNumber 
                    && si.Date == service.ServiceDate 
                    && si.ItemName == serviceItem.ItemName)
                ?? new Data.ServiceItem 
                { 
                    LicensePlateNumber = service.LicensePlateNumber, 
                    Date = service.ServiceDate, 
                    ItemName = serviceItem.ItemName 
                };
            
            servItem.ProductNumber = serviceItem.ProductNumber;
            servItem.PurchasePrice = serviceItem.PurchasePrice;
            servItem.SellingPrice = serviceItem.SellingPrice;
            servItem.Amount = serviceItem.Amount;
            _context.Update(servItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(ServiceDto service)
    {
        // Query for existing service by composite business key using indexed columns
        Data.Service serv = await _context.Services
            .FirstOrDefaultAsync(s => s.LicensePlateNumber == service.LicensePlateNumber && s.Date == service.ServiceDate);
        
        if(serv is null)
            return false;

        _context.Services.Remove(serv);

        await _context.SaveChangesAsync();
        return true;
    }
}
