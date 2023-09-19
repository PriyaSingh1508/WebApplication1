using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using WebApplication1.DataBase;
using WebApplication1.Models;
using WebApplication1.Models.Authentication;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
       

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(Register register)
        {
            var role=register.Role;
            //Check User if exist already
            var userExit = await _userManager.FindByEmailAsync(register.Email);
            if (userExit != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User Already exist" });
            }
            // Add new user to the database
            IdentityUser user = new()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.UserName
                
            };
            // var result=await _userManager.CreateAsync(user,register.Password);
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, register.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                   new Response { Status = "Error", Message = "Failed to Created user into the Database" });

                }
                // Add role to the user
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Success", Message = "Created New User Successfully" });

            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "This Role Doest not exit" });

            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            Login model = new Login();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {

                var result = await _userManager.FindByEmailAsync(model.Email);

                if (result!=null)
                { 
                    return RedirectToAction("Index","Home");
                }
                else
                {

                    return StatusCode(StatusCodes.Status403Forbidden,
                        new Response { Status = "Error", Message = "Failed to login" });

                }
            }
            return View(model);
        }



    }
}
