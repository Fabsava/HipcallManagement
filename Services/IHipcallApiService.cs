using System.Collections.Generic;
using System.Threading.Tasks;
using HipcallManagement.DTOs;

namespace HipcallManagement.Services;

public interface IHipcallApiService
{
    Task<IEnumerable<CompanyDto>> GetCompaniesAsync();
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyRequest request);
}
