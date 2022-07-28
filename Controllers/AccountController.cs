
using System.Security.Claims;
using HotelMS.Models;
using HotelMS.ViewModel;
using HotelMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HotelMS.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegistrationVM register)
    {

        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                Name = register.Name,
                UserName = register.Email,
                Email = register.Email
            };
            try
            {

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("login", "account");
                }
                foreach (var error in result.Errors)
                {
                    if (error.Description.EndsWith("is already taken."))
                    {
                        ModelState.AddModelError("", "This email is already registered !");
                        return View(register);
                    }
                    else
                    {
                        ModelState.AddModelError("", error.Description);
                        return View(register);
                    }
                }
            }
            catch (DbUpdateException e)
            {
                switch (e.InnerException)
                {
                    case PostgresException postgres:
                        {
                            if (postgres.SqlState == "23505")
                            {
                                ModelState.AddModelError("", "This email is already registered !");
                                break;
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    default:
                        ModelState.AddModelError("", "Problem registering user please try again later");
                        break;
                }
                return View(register);

                //Note to implement Logging errors to a logger
                // if (e.)


            }
        }
        return View(register);
    }

    //Logout Controller
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    //Login Controller

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginVM login, string? returnUrl)
    {

        if (ModelState.IsValid)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "This Email Address is not registred");
                }
                else
                {
                    // Console.WriteLine($"------------{user.Id}");
                    // user.SecurityStamp = Guid.NewGuid().ToString();

                    ClaimsStore.Claims.Add(new Claim(ClaimTypes.Name, user?.Name));
                    ClaimsStore.Claims.Add(new Claim(ClaimTypes.NameIdentifier, user?.Id));
                    //To supress this error System. InvalidOperationException: User security stamp cannot be null.
                    await _userManager.AddClaimsAsync(user, ClaimsStore.Claims);
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, login.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    ModelState.AddModelError("", "Email or Password Entered Are Incorrect");
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "An error occured while trying to sign in");
                Console.WriteLine(e);
            }
        }
        return View(login);
    }
    //[AcceptVerbs("Get", "Post")]
    //[AllowAnonymous]
    // public IActionResult IsEmailTaken(string email)
    // {
    //     var user = _userManager.FindByEmailAsync(email);
    //     if (user == null)
    //     {
    //         return Json(true);
    //     }
    //     else
    //     {
    //         return Json($"Email {email} already in use ");
    //     }

    // }

}