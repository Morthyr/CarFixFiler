using CarFixFiler.Dto;

namespace CarFixFiler.Services;

public interface IServiceService 
{
    Task UpdateServiceAsync(ServiceDto service);
    Task<IEnumerable<DisplayServiceDto>> GetServicesAsync();
    Task<ServiceDto> GetServiceAsync(string licensePlateNumber, DateTime date);
    Task<bool> DeleteAsync(ServiceDto service);
}