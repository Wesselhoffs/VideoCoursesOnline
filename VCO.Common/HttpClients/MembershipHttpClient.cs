namespace VCO.Common.HttpClients;

public class MembershipHttpClient
{
    public readonly HttpClient Client; 

    public MembershipHttpClient(HttpClient client)
	{
        Client = client;
    }
}
