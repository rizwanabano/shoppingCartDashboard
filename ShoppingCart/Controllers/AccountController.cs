using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
        public class AccountController : Controller
        {
                private UserManager<AppUser> _userManager;
                private SignInManager<AppUser> _signInManager;
                

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
                {
                        _userManager = userManager;
                        _signInManager = signInManager;
                }

                public IActionResult Create() => View();

                [HttpPost]
                public async Task<IActionResult> Create(User user)
                {
                        if (ModelState.IsValid)
                        {
                                AppUser newUser = new AppUser { UserName = user.UserName, Email = user.Email };
                                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                                if (result.Succeeded)
                                {
                                        return Redirect("/admin/products");
                                }

                                foreach (IdentityError error in result.Errors)
                                {
                                        ModelState.AddModelError("", error.Description);
                                }

                        }

                        return View(user);
                }


        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new AppUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        

        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return Redirect(model.ReturnUrl ?? "/");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }
        public async Task<RedirectResult> Logout(string returnUrl = "/")
                {
                        await _signInManager.SignOutAsync();

                        return Redirect(returnUrl);
                }

        }
}
