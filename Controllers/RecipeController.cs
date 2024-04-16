using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeNook.Data;
using RecipeNook.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RecipeNook.Controllers;
public class RecipeController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IHostingEnvironment _hostingEnvironment;
    public RecipeController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
    {
        _dbContext = dbContext;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<IActionResult> GetRecipeByCategoryId(int id)
    {
        var recipes = await _dbContext.Recipes.Include(c => c.CategoryId == id).Where(c => c.CategoryId == id).ToListAsync();
        ViewBag.Name = _dbContext.Categories.Find(id).Name.ToString();
        return View(recipes);
    }

    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await _dbContext.Recipes.ToListAsync();
        return View(recipes);
    }
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(x => x.Id == id);
        return View(recipe);
    }

    public async Task<IActionResult> CreateRecipe()
    {
        ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe(Recipe recipe)
    {
        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        string imagepath = Path.Combine(ImageFolder, recipe.Image.FileName);
        recipe.Image.CopyTo(new FileStream(imagepath, FileMode.Create));
        recipe.ImageUrl = recipe.Image.FileName;

        var newAnimal = new Recipe
        {
            Name = recipe.Name,
            Description = recipe.Description,
            Directions = recipe.Directions,
            ImageUrl = recipe.ImageUrl,
            CategoryId = recipe.CategoryId
        };
        await _dbContext.Recipes.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> EditRecipe(int id)
    {
        var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "Name");
        return View(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> EditRecipe(Recipe recipe)
    {
        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        string imagepath = Path.Combine(ImageFolder, recipe.Image.FileName);


        var oldImage = _dbContext.Recipes.Find(recipe.Id).ImageUrl;
        var ImageOldPath = Path.Combine(ImageFolder, oldImage);
        recipe.ImageUrl = recipe.Image.FileName;

        if (imagepath != ImageOldPath)
        {
            // Delete Old File
            System.IO.File.Delete(ImageOldPath);

            // Save New File
            recipe.Image.CopyTo(new FileStream(imagepath, FileMode.Create));
        }

        var oldRecipe = _dbContext.Recipes.FirstOrDefault(a => a.Id == recipe.Id);

        oldRecipe.Name = recipe.Name;
        oldRecipe.Description = recipe.Description;
        oldRecipe.Directions = recipe.Directions;
        oldRecipe.ImageUrl = recipe.ImageUrl;
        oldRecipe.CategoryId = recipe.CategoryId;

        _dbContext.Recipes.Update(oldRecipe);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction("GetRecipeByCategoryId", "Recipe", new { id = recipe.CategoryId });

    }

    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(c => c.Id == id);
        return View(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAnimal(Recipe recipe)
    {
        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

        var oldImage = _dbContext.Recipes.Find(recipe.Id).ImageUrl;
        var ImageOldPath = Path.Combine(ImageFolder, oldImage);

        // Delete Old File
        System.IO.File.Delete(ImageOldPath);



        var Drecipe = _dbContext.Recipes.FirstOrDefault(c => c.Id == recipe.Id);

        _dbContext.Remove(Drecipe);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction("GetRecipeByCategoryId", "Recipe", new { id = recipe.CategoryId });
    }
}