using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using HipcallManagement.DTOs;

namespace HipcallManagement.Services;

public class HipcallApiService : IHipcallApiService
{
    protected readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public HipcallApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v3/companies");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        
        if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
        {
            return jsonDocument.RootElement.Deserialize<IEnumerable<CompanyDto>>(_jsonOptions) ?? Enumerable.Empty<CompanyDto>();
        }
        else if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<IEnumerable<CompanyDto>>(_jsonOptions) ?? Enumerable.Empty<CompanyDto>();
        }
        
        return Enumerable.Empty<CompanyDto>();
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v3/companies", request);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);
        
        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<CompanyDto>(_jsonOptions)!;
        }
        
        return jsonDocument.RootElement.Deserialize<CompanyDto>(_jsonOptions)!;
    }
}
