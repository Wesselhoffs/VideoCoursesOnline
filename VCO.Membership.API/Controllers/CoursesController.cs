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
        return Results.NotFound();
    }
        
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
