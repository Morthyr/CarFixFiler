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
        .Select(s => new DisplayServiceDto
        {
            CustomerName = s.Car.CustomerName,
            CustomerTelephone = s.Car.Customer.Telephone,
            LicensePlateNumber = s.LicensePlateNumber,
            ServiceCost = s.Cost,
            ServiceDate = s.Date,
            ServiceDescription = s.Description
        }).ToListAsync();

    public async Task UpdateServiceAsync(ServiceDto service)
    {
        Data.Customer customer = (await _context.Customers.FindAsync(service.CustomerName))
            ?? new Data.Customer { Name = service.CustomerName };
        customer.Telephone = service.CustomerTelephone;
        customer.LaborCostDiscount = service.LaborCostDiscount;
        customer.PartCostDiscount = service.PartCostDiscount;

        _context.Update(customer);

        Data.Car car = (await _context.Car.FindAsync(service.LicensePlateNumber))
            ?? new Data.Car { LicensePlateNumber = service.LicensePlateNumber };
        car.CustomerName = service.CustomerName;
        car.Vin = service.Vin;
        car.EngineCode = service.EngineCode;
        car.Power = service.Power;
        car.EngineDisplacement = service.EngineDisplacement;
        car.ManufatureYear = service.ManufatureYear;
        car.Mileage = service.Mileage;

        _context.Update(car);

        Data.Service serv = (await _context.Services.FindAsync(service.LicensePlateNumber, service.ServiceDate))
            ?? new Data.Service { LicensePlateNumber = service.LicensePlateNumber, Date = service.ServiceDate };
        serv.Description = service.ServiceDescription;
        serv.Cost = service.ServiceCost;

        _context.Update(serv);

        foreach (var serviceItem in service.ServiceItems)
        {
            Data.ServiceItem servItem = (await _context.ServiceItems.FindAsync(service.LicensePlateNumber, service.ServiceDate, serviceItem.ItemName))
                ?? new Data.ServiceItem { LicensePlateNumber = service.LicensePlateNumber, Date = service.ServiceDate, ItemName = serviceItem.ItemName };
            servItem.PurchasePrice = serviceItem.PurchasePrice;
            servItem.SellingPrice = serviceItem.SellingPrice;
            servItem.Amount = serviceItem.Amount;
            _context.Update(servItem);
        }

        await _context.SaveChangesAsync();
    }
}
