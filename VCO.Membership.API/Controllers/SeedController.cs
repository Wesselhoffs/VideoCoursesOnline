namespace VCO.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly IDbService _db;

    public SeedController(IDbService dbService)
    {
        _db = dbService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> Seed()
    {
        try
        {
            await _db.SeedMembershipData();
            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.BadRequest();
        }
    }
}
