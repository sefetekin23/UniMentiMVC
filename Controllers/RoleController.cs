using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniMenti.Models;
using UniMenti;
using UniMenti.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using UniMenti.ViewModels;
using Microsoft.IdentityModel.Tokens;
using UniMenti.Data;
namespace UniMenti.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
            
            

            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleVM model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    if (!(await _roleManager.RoleExistsAsync(model.SelectedRole)))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.SelectedRole));
                        return RedirectToAction("RoleList", "Role");
                    }
                }
                return RedirectToAction("RoleList", "Role");
            }
            catch
            {
                return RedirectToAction("RoleList", "Role");
            }
        }

        
            [HttpGet]
        public IActionResult RoleList()
        {
            //var users = _context.Users.ToList(); 
            return View();
        }
    }
}
