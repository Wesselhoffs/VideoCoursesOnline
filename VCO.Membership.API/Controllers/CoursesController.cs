using VCO.Common.DTOs;

namespace VCO.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly IDbService _db;

    public CoursesController(IDbService db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IResult> Get(bool freeOnly)
    {
        try
        {
            await _db.Include<Instructor>();
            List<CourseDTO>? courses = freeOnly ? await _db.GetAsync<Course, CourseDTO>(c => c.Free.Equals(freeOnly))
                                                : await _db.GetAsync<Course, CourseDTO>();
            return Results.Ok(courses);
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
            _db.Include<Instructor>();
            _db.Include<Section>();
            _db.Include<Video>();
            var course = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id));

            return Results.Ok(course);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] CreateCourseDTO dto)
    {
        try
        {
            if (dto == null) return Results.BadRequest();

            var course = await _db.AddAsync<Course, CreateCourseDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success == false)
            {
                return Results.BadRequest();
            }
            return Results.Created(_db.GetURI(course), course);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] EditCourseDTO dto)
    {
        try
        {
            if (dto is null)return Results.BadRequest("No entity provided");
            if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

            var exists = await _db.AnyAsync<Instructor>(i => i.Id.Equals(dto.InstructorId));
            if (!exists) return Results.NotFound("Could not find related entity");

            exists = await _db.AnyAsync<Course>(c => c.Id.Equals(id));
            if (!exists) return Results.NotFound("Could not find entity");

            _db.Update<Course, EditCourseDTO>(dto, dto.Id);

            var success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

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
            var success = await _db.DeleteAsync<Course>(id);

            if (!success) return Results.NotFound();

            success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
