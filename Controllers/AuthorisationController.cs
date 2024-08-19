using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniMenti.Data;
using UniMenti.Models;
using UniMenti.ViewModels; // Replace with your actual namespace

[Authorize(Policy = "AdminOnly")]
public class AuthorisationController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;
    public AuthorisationController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public IActionResult AuthorisationList()
    {
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        roles.Insert(0, "");


        var viewModel = new RoleVM
        {
            Users = users,
            Roles = new SelectList(roles)
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("AssignRole")]
    public async Task<ActionResult> AssignRole(AppUser user, RoleVM rolevm)
    {
        if (rolevm.SelectedRole == null || rolevm.SelectedRole.Equals(""))
        { 
            return RedirectToAction("AuthorisationList", "Authorisation");
        }
        // Check if the role exists
        if (await _roleManager.RoleExistsAsync(rolevm.SelectedRole))
        {
            // Find the user
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser != null)
            {
                // Get all roles of the user
                var userRoles = await _userManager.GetRolesAsync(existingUser);
               

                // Check if the user already has a role
                if (userRoles.Any())
                {
                    // Remove all existing roles
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(existingUser, userRoles);

                    if (!removeRolesResult.Succeeded)
                    {
                        // Handle error if unable to remove existing roles
                        // Redirect or return appropriate error response
                        return RedirectToAction("AuthorisationList", "Authorisation");
                    }
                }

                // Assign the new role
                var assignRoleResult = await _userManager.AddToRoleAsync(existingUser, rolevm.SelectedRole);

                if (assignRoleResult.Succeeded)
                {
                    // Role assignment succeeded
                    return RedirectToAction("AuthorisationList", "Authorisation");
                }
                else
                {
                    // Handle role assignment failure
                    // Redirect or return appropriate error response
                    return RedirectToAction("AuthorisationList", "Authorisation");
                }
            }
            else
            {
                // User with the provided ID not found
                // Handle this scenario based on your application logic
                return RedirectToAction("AuthorisationList", "Authorisation");
            }
        }
        else
        {
            // Role specified in rolevm.SelectedRole does not exist
            // Handle this scenario based on your application logic
            return RedirectToAction("AuthorisationList", "Authorisation");
        }
    }



}
