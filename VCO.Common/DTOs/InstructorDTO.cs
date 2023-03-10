namespace VCO.Common.DTOs;

public class InstructorDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
}
public class CreateInstructorDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
}
