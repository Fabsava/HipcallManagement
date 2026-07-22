using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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

    public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/v3/companies/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<CompanyDto>(_jsonOptions);
        }

        return jsonDocument.RootElement.Deserialize<CompanyDto>(_jsonOptions);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyRequest request)
    {
        if (request.Emails != null && (!request.Emails.Any() || request.Emails.All(e => string.IsNullOrWhiteSpace(e.Email))))
        {
            request.Emails = null!;
        }
        if (request.Phones != null && (!request.Phones.Any() || request.Phones.All(p => string.IsNullOrWhiteSpace(p.Number))))
        {
            request.Phones = null!;
        }
        if (string.IsNullOrWhiteSpace(request.Description)) request.Description = null;
        if (string.IsNullOrWhiteSpace(request.WebsiteUrl)) request.WebsiteUrl = null;
        if (string.IsNullOrWhiteSpace(request.LinkedinUrl)) request.LinkedinUrl = null;
        if (string.IsNullOrWhiteSpace(request.CustomUrl)) request.CustomUrl = null;

        var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v3/companies", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                if (doc.RootElement.TryGetProperty("errors", out var errorsElement))
                {
                    var errorMessages = new List<string>();
                    foreach (var prop in errorsElement.EnumerateObject())
                    {
                        var fieldName = prop.Name;
                        if (prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var err in prop.Value.EnumerateArray())
                            {
                                errorMessages.Add($"{fieldName}: {err.GetString()}");
                            }
                        }
                        else
                        {
                            errorMessages.Add($"{fieldName}: {prop.Value.GetString()}");
                        }
                    }
                    if (errorMessages.Any())
                    {
                        throw new HttpRequestException(string.Join(" | ", errorMessages));
                    }
                }
            }
            catch (JsonException)
            {
            }
            throw new HttpRequestException($"HTTP {(int)response.StatusCode}: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<CompanyDto>(_jsonOptions)!;
        }

        return jsonDocument.RootElement.Deserialize<CompanyDto>(_jsonOptions)!;
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync()
    {
        var response = await _httpClient.GetAsync("/api/v3/contacts");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
        {
            return jsonDocument.RootElement.Deserialize<IEnumerable<ContactDto>>(_jsonOptions) ?? Enumerable.Empty<ContactDto>();
        }
        else if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<IEnumerable<ContactDto>>(_jsonOptions) ?? Enumerable.Empty<ContactDto>();
        }

        return Enumerable.Empty<ContactDto>();
    }

    public async Task<ContactDto?> GetContactByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/v3/contacts/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<ContactDto>(_jsonOptions);
        }

        return jsonDocument.RootElement.Deserialize<ContactDto>(_jsonOptions);
    }

    public async Task<ContactDto> CreateContactAsync(CreateContactRequest request)
    {
        if (request.Emails != null && (!request.Emails.Any() || request.Emails.All(e => string.IsNullOrWhiteSpace(e.Email))))
        {
            request.Emails = null!;
        }
        if (request.Phones != null && (!request.Phones.Any() || request.Phones.All(p => string.IsNullOrWhiteSpace(p.Number))))
        {
            request.Phones = null!;
        }
        if (string.IsNullOrWhiteSpace(request.JobTitle)) request.JobTitle = null;
        if (string.IsNullOrWhiteSpace(request.LinkedinUrl)) request.LinkedinUrl = null;
        if (string.IsNullOrWhiteSpace(request.CustomUrl)) request.CustomUrl = null;

        var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v3/contacts", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                if (doc.RootElement.TryGetProperty("errors", out var errorsElement))
                {
                    var errorMessages = new List<string>();
                    foreach (var prop in errorsElement.EnumerateObject())
                    {
                        var fieldName = prop.Name;
                        if (prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var err in prop.Value.EnumerateArray())
                            {
                                errorMessages.Add($"{fieldName}: {err.GetString()}");
                            }
                        }
                        else
                        {
                            errorMessages.Add($"{fieldName}: {prop.Value.GetString()}");
                        }
                    }
                    if (errorMessages.Any())
                    {
                        throw new HttpRequestException(string.Join(" | ", errorMessages));
                    }
                }
            }
            catch (JsonException)
            {
            }
            throw new HttpRequestException($"HTTP {(int)response.StatusCode}: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(content);

        if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
        {
            return dataElement.Deserialize<ContactDto>(_jsonOptions)!;
        }

        return jsonDocument.RootElement.Deserialize<ContactDto>(_jsonOptions)!;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/v3/users");
            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<UserDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);

            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                return jsonDocument.RootElement.Deserialize<IEnumerable<UserDto>>(_jsonOptions) ?? Enumerable.Empty<UserDto>();
            }
            else if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
            {
                return dataElement.Deserialize<IEnumerable<UserDto>>(_jsonOptions) ?? Enumerable.Empty<UserDto>();
            }

            return Enumerable.Empty<UserDto>();
        }
        catch
        {
            return Enumerable.Empty<UserDto>();
        }
    }
}
