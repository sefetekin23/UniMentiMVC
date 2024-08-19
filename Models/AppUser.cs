using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniMenti.Models;

public class AppUser:IdentityUser
{
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    public string? name { get; set; }
    public string? address { get; set; }
    

}
