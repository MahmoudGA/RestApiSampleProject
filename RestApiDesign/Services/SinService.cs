using RestApiDesign.DTOs;
using RestApiDesign.Models;
using RestApiDesign.Repositories;
using RestApiDesign.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestApiDesign.Services
{
    public class SinService : ISinService
    {
        private readonly ILogger<SinService> _logger;
        private readonly ISinRepository _sinRepository;
        public SinService(ILogger<SinService> logger, ISinRepository sinRepository)
        {
            _logger = logger;
            _sinRepository = sinRepository;
        }
        public Task<bool> Exists(string sin)
        {
            return _sinRepository.Exists(sin);
        }

        public Task<SinDto> GetSin(string sin)
        {
            _logger.LogInformation($"Get sin details for Sin: {sin}");
            return _sinRepository.GetSin(sin);
        }

        public Task<bool> Create(CreateSinDto createSinDto)
        {
            //_data.Add(new SinDto() { FullName = createSinDto.FullName, NationalId = createSinDto.NationalId, SocialInsuranceNumber = createSinDto.Sin });
            if (Exists(createSinDto.Sin).Result)
                throw new ValidationException("Sin already exists");
            
            _sinRepository.Create(createSinDto);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<SinDto>> Search(string sin, string status)
        {
            _logger.LogInformation($"Search {sin}");
            List<SinDto> searchResults = _sinRepository.Search(sin, status).Result.ToList(); //_data.Where(s => s.SocialInsuranceNumber.StartsWith(sin.Substring(0, 4)) && s.Status == status).ToList();
            _logger.LogInformation($"Search results: {searchResults.Count}");
            return Task.FromResult<IEnumerable<SinDto>>(searchResults);
        }

        public Task<string> Authenticate(string username, string password)
        {
            return _sinRepository.Authenticate(username, password);
        }

        public Task<bool> NationalIdExists(string nationalId)
        {
            return _sinRepository.NationalIdExists(nationalId);
        }
    }
}
