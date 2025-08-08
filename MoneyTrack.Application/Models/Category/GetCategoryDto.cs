namespace MoneyTrack.Application.Models.Category;

public class GetCategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string? Description { get; set; }
}