namespace VCO.Common.DTOs;

public class SectionDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CourseId { get; set; }
    public string Course { get; set; }
    public virtual List<VideoDTO> Videos { get; set; }
}
public class CreateSectionDTO
{
    public string Title { get; set; }
    public int CourseId { get; set; }
}
public class EditSectionDTO : CreateSectionDTO
{
    public int Id { get; set; }
}
