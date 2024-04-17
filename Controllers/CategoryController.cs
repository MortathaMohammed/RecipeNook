using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeNook.Data;
using RecipeNook.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RecipeNook.Controllers;

public class CategoryController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IHostingEnvironment _hostingEnvironment;
    public CategoryController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
    {
        _dbContext = dbContext;
        _hostingEnvironment = hostingEnvironment;
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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory()
    {
        return View();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        string imagepath = Path.Combine(ImageFolder, category.Image.FileName);
        category.Image.CopyTo(new FileStream(imagepath, FileMode.Create));
        category.ImageUrl = category.Image.FileName;
        var newCategory = new Category
        {
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };

        await _dbContext.Categories.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(GetCategories));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        return View(category);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> EditCategory(Category category)
    {

        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        string imagepath = Path.Combine(ImageFolder, category.Image.FileName);

        var oldImage = _dbContext.Categories.Find(category.Id)!.ImageUrl;
        var ImageOldPath = Path.Combine(ImageFolder, oldImage);
        category.ImageUrl = category.Image.FileName;

        if (imagepath != ImageOldPath)
        {
            // Delete Old File
            System.IO.File.Delete(ImageOldPath);

            // Save New File
            category.Image.CopyTo(new FileStream(imagepath, FileMode.Create));
        }

        var oldCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        oldCategory.Name = category.Name;
        oldCategory.ImageUrl = category.ImageUrl;

        _dbContext.Categories.Update(oldCategory);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(GetCategories));

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return View(category);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(Category category)
    {
        string ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

        var oldImage = _dbContext.Categories.Find(category.Id)!.ImageUrl;
        var ImageOldPath = Path.Combine(ImageFolder, oldImage);

        // Delete Old File
        System.IO.File.Delete(ImageOldPath);
        var Dcategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

        _dbContext.Categories.Remove(Dcategory);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(GetCategories));
    }
}