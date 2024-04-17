using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeNook.Models;
public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [NotMapped]
    public IFormFile Image { get; set; }
    public string ImageUrl { get; set; }
    public List<Recipe>? Recipes { get; set; }
}