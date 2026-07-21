using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HipcallManagement.DTOs;

public class CreateContactRequest
{
    [JsonPropertyName("assign_to_user_id")]
    public int? AssignToUserId { get; set; }

    [JsonPropertyName("company_id")]
    public int? CompanyId { get; set; }

    [JsonPropertyName("custom_url")]
    public string? CustomUrl { get; set; }

    [JsonPropertyName("emails")]
    public List<EmailDto> Emails { get; set; } = new();

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("job_title")]
    public string? JobTitle { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("linkedin_url")]
    public string? LinkedinUrl { get; set; }

    [JsonPropertyName("phones")]
    public List<PhoneDto> Phones { get; set; } = new();
}
