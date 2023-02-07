namespace VCO.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorsController : ControllerBase
{
    private readonly IDbService _db;

    public InstructorsController(IDbService db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            List<InstructorDTO>? instructors = await _db.GetAsync<Instructor, InstructorDTO>();
            return Results.Ok(instructors);
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
            var instructor = await _db.SingleAsync<Instructor, InstructorDTO>(i => i.Id.Equals(id));
            if (instructor is null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(instructor);
            }
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] CreateInstructorDTO dto)
    {
        try
        {
            if (dto is null)
            {
                return Results.BadRequest();
            }

            var instructor = await _db.AddAsync<Instructor, CreateInstructorDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success is false)
            {
                return Results.BadRequest();
            }
            return Results.Created(_db.GetURI(instructor), instructor);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] InstructorDTO dto)
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

            var exists = await _db.AnyAsync<Instructor>(c => c.Id.Equals(id));

            if (exists is false)
            {
                return Results.NotFound("Could not find entity");
            }

            _db.Update<Instructor, InstructorDTO>(dto, dto.Id);

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
            var deleted = await _db.DeleteAsync<Instructor>(id);

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
