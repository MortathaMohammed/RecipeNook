using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeNook.Models;
public class Recipe
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Ingredients { get; set; }
    public string? Directions { get; set; }

    [NotMapped]
    public IFormFile Image { get; set; }
    public string ImageUrl { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

}