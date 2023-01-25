var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureAutoMapper();


builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsAllAccessPolicy", opt => opt.AllowAnyOrigin()
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VCOContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VCOConnection")));
builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsAllAccessPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureAutoMapper()
{
    var config = new AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Video, VideoDTO>().ReverseMap();
        cfg.CreateMap<Instructor, InstructorDTO>().ReverseMap().ForMember(dest => dest.Courses, src => src.Ignore());
        cfg.CreateMap<Course, CourseDTO>().ReverseMap().ForMember(dest => dest.Instructor, src => src.Ignore());
        cfg.CreateMap<Section, SectionDTO>().ForMember(dest => dest.Course, src => src.MapFrom(s => s.Course.Title))
                                            .ReverseMap()
                                            .ForMember(dest => dest.Course, src => src.Ignore());
    });
    var mapper = config.CreateMapper();
    builder.Services.AddSingleton(mapper);
}