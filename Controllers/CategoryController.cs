using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeNook.Data;
using RecipeNook.Models;

namespace RecipeNook.Controllers;
public class CategoryController : Controller
{
    private readonly AppDbContext _dbContext;
    public CategoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> GetCategories()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return View(categories);
    }

    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return View(category);
    }


    public async Task<IActionResult> CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var newCategory = new Category
        {
            Name = category.Name,
        };

        await _dbContext.Categories.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(GetCategories));
    }

    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Category category)
    {

        var oldCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        oldCategory.Name = category.Name;

        _dbContext.Categories.Update(oldCategory);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(GetCategories));

    }

    public async Task<IActionResult> DeteleCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(Category category)
    {
        var Dcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        _dbContext.Categories.Remove(Dcategory);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(GetCategories));
    }
}