using System.Text;

namespace VCO.Common.Services;

public class AdminService : IAdminService
{
    public readonly MembershipHttpClient Http;

    public AdminService(MembershipHttpClient httpClient)
    {
        Http = httpClient;
    }

    public async Task<List<TDto>> GetAsync<TDto>(string uri)
    {
        try
        {
            using var response = await Http.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var dtoList = await response.Content.ReadFromJsonAsync<List<TDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return dtoList ?? new List<TDto>();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<TDto>? SingleAsync<TDto>(string uri)
    {
        try
        {
            using var response = await Http.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<TDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return dto ?? default;
        }
        catch (Exception)
        {
            throw;
        }
    }

	public async Task CreateAsync<TDto>(string uri, TDto dto)
	{
		try
		{
			using var response = await Http.Client.PostAsJsonAsync(uri, dto);
			response.EnsureSuccessStatusCode();
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task EditAsync<TDto>(string uri, TDto dto)
	{
		try
		{
			using var response = await Http.Client.PutAsJsonAsync(uri, dto);
			response.EnsureSuccessStatusCode();
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task DeleteAsync<TDto>(string uri)
	{
		try
		{
			using var response = await Http.Client.DeleteAsync(uri);
			response.EnsureSuccessStatusCode();
		}
		catch (Exception)
		{
			throw;
		}
	}
}
