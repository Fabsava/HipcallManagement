using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HipcallManagement.DTOs;

namespace HipcallManagement.Services;

public class HipcallApiService : IHipcallApiService
{
    protected readonly HttpClient _httpClient;

    public HipcallApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<CompanyDto>>("/api/v3/companies");
        return response ?? Enumerable.Empty<CompanyDto>();
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v3/companies", request);
        response.EnsureSuccessStatusCode();
        var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
        return company!;
    }
}
