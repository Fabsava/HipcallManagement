using System.Text.Json.Serialization;

namespace HipcallManagement.DTOs;

public class EmailDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
