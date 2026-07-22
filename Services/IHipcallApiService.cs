using System.Collections.Generic;
using System.Threading.Tasks;
using HipcallManagement.DTOs;

namespace HipcallManagement.Services;

public interface IHipcallApiService
{
    Task<IEnumerable<CompanyDto>> GetCompaniesAsync();
    Task<CompanyDto?> GetCompanyByIdAsync(int id);
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyRequest request);
    Task<IEnumerable<ContactDto>> GetContactsAsync();
    Task<ContactDto?> GetContactByIdAsync(int id);
    Task<ContactDto> CreateContactAsync(CreateContactRequest request);
    Task<IEnumerable<UserDto>> GetUsersAsync();
}
