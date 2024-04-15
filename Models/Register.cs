using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace RecipeNook.Models;
public class Register
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    [DisplayName("Confirm Password")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}