using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HipcallManagement.DTOs;

public class CompanyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("assign_to_user_id")]
    public int? AssignToUserId { get; set; }

    [JsonPropertyName("custom_url")]
    public string? CustomUrl { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("emails")]
    public List<EmailDto> Emails { get; set; } = new();

    [JsonPropertyName("linkedin_url")]
    public string? LinkedinUrl { get; set; }

    [JsonPropertyName("phones")]
    public List<PhoneDto> Phones { get; set; } = new();

    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; set; }
}
