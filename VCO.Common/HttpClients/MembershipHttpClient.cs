namespace VCO.Common.HttpClients;

public class MembershipHttpClient
{
    private readonly HttpClient Client; 

    public MembershipHttpClient(HttpClient client)
	{
        Client = client;
    }
}
