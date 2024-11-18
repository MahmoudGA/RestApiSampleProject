using RestApiDesign.DTOs;
using RestApiDesign.Models;

namespace RestApiDesign.Repositories
{
    public interface ISinRepository
    {
        Task<bool> Exists(string sin);
        Task<SinDto> GetSin(string sin);
        Task<bool> NationalIdExists(string nationalId);
        Task<bool> Create(CreateSinDto createSinDto);
        Task<IEnumerable<SinDto>> Search(string sin, string status);
        Task<string> Authenticate(string username, string password);
    }
}
