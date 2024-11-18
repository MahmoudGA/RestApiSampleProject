using RestApiDesign.DTOs;
using RestApiDesign.Models;

namespace RestApiDesign.Repositories
{
    public class CrmSinRepository : ISinRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CrmSinRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> Exists(string sin)
        {
            throw new NotImplementedException();
        }

        public Task<SinDto> GetSin(string sin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(CreateSinDto createSinDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SinDto>> Search(string sin, string status)
        {
            throw new NotImplementedException();
        }

        public Task<string> Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NationalIdExists(string nationalId)
        {
            throw new NotImplementedException();
        }
    }

}
