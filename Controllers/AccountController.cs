using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniMenti.Data;
using UniMenti.Models;
using UniMenti.ViewModels;

namespace UniMenti.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<AppUser> signInManager;
    private readonly UserManager<AppUser> userManager;
    //private readonly RoleManager<IdentityUser> roleManager;
    private readonly AppDbContext _context;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext context)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this._context = context;
        //this.roleManager = roleManager;


    }
    [HttpGet]
    public IActionResult Login()
    {

        return View();
    }
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // If returnUrl is local to the application
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        // If we got this far, something failed, redisplay form
        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();

    }



    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            AppUser user = new()
            {
                name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                address = model.Address
            };

            

            var result = await userManager.CreateAsync(user, model.Password!);
            var result2 = await userManager.AddToRoleAsync(user, "User");


            if (result.Succeeded && result2.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }


    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    //[HttpGet]
    //public async Task<IActionResult> Edit(Guid id)
    //{
    //    var student = await _context.Users.FindAsync(id);

    //    return View(student);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Edit(RegisterVM viewModel) can put AppUser it has Id
    //{
    //    var student = await _context.Users.FindAsync(viewModel.Id);
    //    if (student is not null)
    //    {
    //        student.Name = viewModel.Name;
    //        student.Email = viewModel.Email;
    //        student.Phone = viewModel.Phone;
    //        student.Subscribed = viewModel.Subscribed;

    //        await _context.SaveChangesAsync();
    //    }

    //    return RedirectToAction("Login", "Students");
    //}

    [Authorize(Roles = "admin, tutor")]

    public async Task<IActionResult> UserList(string sortOrder, string searchString)
    {
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["UsernameSortParam"] = sortOrder == "Username" ? "username_desc" : "Username";
        ViewData["EmailSortParam"] = sortOrder == "Email" ? "email_desc" : "Email";
        ViewData["AddressSortParam"] = sortOrder == "Address" ? "address_desc" : "Address";
        var users = await userManager.GetUsersInRoleAsync("student");
        //var users = _context.Users.ToList(); // Replace with your logic to get users from the database

        // Apply filtering
        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u => u.UserName.Contains(searchString)
                                   || u.name.Contains(searchString)
                                   || u.Email.Contains(searchString)
                                   || u.address.Contains(searchString)).ToList();
        }

        // Apply sorting
        switch (sortOrder)
        {
            case "name_desc":
                users = users.OrderByDescending(u => u.name).ToList();
                break;
            case "Username":
                users = users.OrderBy(u => u.UserName).ToList();
                break;
            case "username_desc":
                users = users.OrderByDescending(u => u.UserName).ToList();
                break;
            case "Email":
                users = users.OrderBy(u => u.Email).ToList();
                break;
            case "email_desc":
                users = users.OrderByDescending(u => u.Email).ToList();
                break;
            case "Address":
                users = users.OrderBy(u => u.address).ToList();
                break;
            case "address_desc":
                users = users.OrderByDescending(u => u.address).ToList();
                break;
            default:
                users = users.OrderBy(u => u.name).ToList();
                break;
        }

        var viewModel = new UserListVM
        {
            Users = (List<AppUser>)users,
            SearchString = searchString
        };

        return View(viewModel);
    }





    [Authorize(Roles = "admin, student")]

    public async Task<IActionResult> TutorList(string sortOrder, string searchString)
    {
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["UsernameSortParam"] = sortOrder == "Username" ? "username_desc" : "Username";
        ViewData["EmailSortParam"] = sortOrder == "Email" ? "email_desc" : "Email";
        ViewData["AddressSortParam"] = sortOrder == "Address" ? "address_desc" : "Address";
        var users = await userManager.GetUsersInRoleAsync("tutor");
        //var users = _context.Users.ToList(); // Replace with your logic to get users from the database

        // Apply filtering
        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u => u.UserName.Contains(searchString)
                                   || u.name.Contains(searchString)
                                   || u.Email.Contains(searchString)
                                   || u.address.Contains(searchString)).ToList();
        }

        // Apply sorting
        switch (sortOrder)
        {
            case "name_desc":
                users = users.OrderByDescending(u => u.name).ToList();
                break;
            case "Username":
                users = users.OrderBy(u => u.UserName).ToList();
                break;
            case "username_desc":
                users = users.OrderByDescending(u => u.UserName).ToList();
                break;
            case "Email":
                users = users.OrderBy(u => u.Email).ToList();
                break;
            case "email_desc":
                users = users.OrderByDescending(u => u.Email).ToList();
                break;
            case "Address":
                users = users.OrderBy(u => u.address).ToList();
                break;
            case "address_desc":
                users = users.OrderByDescending(u => u.address).ToList();
                break;
            default:
                users = users.OrderBy(u => u.name).ToList();
                break;
        }

        var viewModel = new UserListVM
        {
            Users = (List<AppUser>)users,
            SearchString = searchString
        };

        return View(viewModel);
    }







    //[HttpPost]
    //public async Task<IActionResult> Delete(int userId)
    //{
    //    var user = await userManager.FindByIdAsync(userId.ToString());

    //    if (user == null)
    //    {
    //        // User not found, handle the error (e.g., show an error message or redirect)
    //        return View();
    //    }

    //    var result = await userManager.DeleteAsync(user);

    //    if (result.Succeeded)
    //    {
    //        var saved = await _context.SaveChangesAsync();
    //        return View();
    //    }
    //    else
    //    {
    //        // Handle deletion failure, if needed
    //        // For example, you might add ModelState errors
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError(string.Empty, error.Description);
    //        }

    //        return View();
    //    }
    //}




}

