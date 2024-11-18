using RestApiDesign.DTOs;
using RestApiDesign.Models;

namespace RestApiDesign.Services.Interfaces
{
    public interface ISinService
    {
        Task<SinDto> GetSin(string sin);
        Task<bool> Exists(string sin);
        Task<bool> NationalIdExists(string nationalId);
        Task<bool> Create(CreateSinDto createSinDto);
        Task<IEnumerable<SinDto>> Search(string sin, string status);
        Task<string> Authenticate(string username, string password);
    }
}
