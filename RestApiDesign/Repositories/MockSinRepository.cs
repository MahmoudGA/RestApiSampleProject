using RestApiDesign.DTOs;
using RestApiDesign.Models;

namespace RestApiDesign.Repositories
{
    public class MockSinRepository : ISinRepository
    {
        private List<SinDto> _data;

        public MockSinRepository()
        {
            _data = new List<SinDto>()
        {
            new SinDto() { FullName = "Ahmed", Username = "ahmed", Password = "ahmed", NationalId = "29908082502001", SocialInsuranceNumber = "34598787", Status = "Active" },
            new SinDto() { FullName = "Ali", Username = "ali", Password = "ali", NationalId = "29908082502002", SocialInsuranceNumber = "34598788", Status = "Active" },
            new SinDto() { FullName = "Mohamed", Username = "mohamed", Password = "mohamed", NationalId = "29908082502003", SocialInsuranceNumber = "34598789", Status = "Active" },
            new SinDto() { FullName = "Mahmoud", Username = "mahmoud", Password = "mahmoud", NationalId = "29908082502004", SocialInsuranceNumber = "34598790", Status = "InActive" }
        };
        }

        public Task<bool> Exists(string sin)
        {
            return Task.FromResult(_data.Any(s => s.SocialInsuranceNumber == sin));
        }

        public Task<SinDto> GetSin(string sin)
        {
            return Task.FromResult(_data.FirstOrDefault(s => s.SocialInsuranceNumber == sin));
        }

        public Task<bool> Create(CreateSinDto createSinDto)
        {
            _data.Add(new SinDto() { FullName = createSinDto.FullName, NationalId = createSinDto.NationalId, SocialInsuranceNumber = createSinDto.Sin });
            return Task.FromResult(true);
        }

        public Task<IEnumerable<SinDto>> Search(string sin, string status)
        {
            var searchResults = _data.Where(s => s.SocialInsuranceNumber.StartsWith(sin.Substring(0, 4)) && s.Status == status).ToList();
            return Task.FromResult<IEnumerable<SinDto>>(searchResults);
        }

        public Task<string> Authenticate(string username, string password)
        {
            return Task.FromResult(_data.FirstOrDefault(s => s.Username == username && s.Password == password)?.NationalId);
        }

        public Task<bool> NationalIdExists(string nationalId)
        {
            return Task<bool>.FromResult(_data.Any(s => s.NationalId == nationalId));    
        }
    }

}
