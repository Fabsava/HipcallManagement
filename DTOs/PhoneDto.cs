using System.Text.Json.Serialization;

namespace HipcallManagement.DTOs;

public class PhoneDto
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;
}
