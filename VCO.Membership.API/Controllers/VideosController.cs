namespace VCO.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly IDbService _db;

    public VideosController(IDbService db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            List<VideoDTO>? sections = await _db.GetAsync<Video, VideoDTO>();
            return Results.Ok(sections);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IResult> Get(int id)
    {
        try
        {
            var section = await _db.SingleAsync<Video, VideoDTO>(i => i.Id.Equals(id));
            if (section is null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(section);
            }
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] CreateVideoDTO dto)
    {
        try
        {
            if (dto is null)
            {
                return Results.BadRequest();
            }

            var section = await _db.AddAsync<Video, CreateVideoDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success is false)
            {
                return Results.BadRequest();
            }
            return Results.Created(_db.GetURI(section), section);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] VideoDTO dto)
    {
        try
        {
            if (dto is null)
            {
                return Results.BadRequest("No entity provided");
            }
            if (id.Equals(dto.Id) is false)
            {
                return Results.BadRequest("Differing ids");
            }

            var exists = await _db.AnyAsync<Video>(c => c.Id.Equals(id));

            if (exists is false)
            {
                return Results.NotFound("Could not find entity");
            }

            _db.Update<Video, VideoDTO>(dto, dto.Id);

            var success = await _db.SaveChangesAsync();

            if (success is false)
            {
                return Results.BadRequest();
            }

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        try
        {
            var deleted = await _db.DeleteAsync<Video>(id);

            if (deleted is false)
            {
                return Results.NotFound();
            }

            var success = await _db.SaveChangesAsync();

            if (success is false)
            {
                return Results.BadRequest();
            }

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
