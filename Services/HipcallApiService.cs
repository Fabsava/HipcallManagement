namespace HipcallManagement.Services;

public class HipcallApiService : IHipcallApiService
{
    protected readonly HttpClient _httpClient;

    public HipcallApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
