using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VCO.Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IDbService _db;

        public SectionsController(IDbService db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                await _db.Include<Video>();
                List<SectionDTO>? sections = await _db.GetAsync<Section, SectionDTO>();
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
                await _db.Include<Video>();
                var section = await _db.SingleAsync<Section, SectionDTO>(i => i.Id.Equals(id));
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
        public async Task<IResult> Post([FromBody] CreateSectionDTO dto)
        {
            try
            {
                if (dto is null)
                {
                    return Results.BadRequest();
                }

                var section = await _db.AddAsync<Section, CreateSectionDTO>(dto);

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
        public async Task<IResult> Put(int id, [FromBody] SectionDTO dto)
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

                var exists = await _db.AnyAsync<Section>(c => c.Id.Equals(id));

                if (exists is false)
                {
                    return Results.NotFound("Could not find entity");
                }

                _db.Update<Section, SectionDTO>(dto, dto.Id);

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
                var deleted = await _db.DeleteAsync<Section>(id);

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
}