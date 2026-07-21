using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HipcallManagement.DTOs;

public class CreateUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("phone_countries")]
    public List<string> PhoneCountries { get; set; } = new();

    [JsonPropertyName("phone_prefix")]
    public string? PhonePrefix { get; set; }

    [JsonPropertyName("redirect_number")]
    public string? RedirectNumber { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
