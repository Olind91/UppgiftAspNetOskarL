using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OskarLAspNet.Contexts;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.Identity;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Controllers
{

    public class AdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ProductService _productService;
        private readonly ContactFormService _contactFormService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthService _authService;

        public AdminController(DataContext dataContext, ProductService productService, ContactFormService contactFormService, UserManager<AppUser> userManager, AuthService authService)
        {
            _dataContext = dataContext;
            _productService = productService;
            _contactFormService = contactFormService;
            _userManager = userManager;
            _authService = authService;
        }

        #region Success messages when creating tags or categorys
        //Skickar meddelande om att t.ex. ny tag eller kategori har skapats vid redirect
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            if (TempData.TryGetValue("SuccessMessage", out var successMessage))
            {
                ViewBag.SuccessMessage = successMessage.ToString();
            }
            return View();
        }
        #endregion

        #region Get all users for the admin
        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsers()
        {
            List<AppUser> userList = _dataContext.Users.ToList();
            return View(userList);
            
        }
        #endregion

        #region Show all products on adminpage
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminProducts()
        {

            var products = await _productService.GetAllAsync();
            return View(products);

        }
        #endregion

        #region Show registered comments as admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ShowAllComments()
        {

            var comments = await _contactFormService.GetAllAsync();
            return View(comments);

        }
        #endregion


        #region Create user as admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser(UserRegisterVM viewModel)
        {
            if (ModelState.IsValid)
            {
                //Kollar om user finns, true/false
                if (await _authService.UserAlreadyExistsAsync(x => x.Email == viewModel.Email))
                    ModelState.AddModelError("", "A user with the same email already exists");

                //registrerar och omdirigerar
                if (await _authService.RegisterUserAsync(viewModel))
                    return RedirectToAction("index", "login");


            }
            ModelState.Clear();
            return View(viewModel);
        }
        #endregion




        #region Change user-roles as admin
        //BYTA ROLLER PÅ USERS
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            //Hämtar user via ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                
                return RedirectToAction("GetAllUsers");
            }

            //Hämtar role
            var currentRoles = await _userManager.GetRolesAsync(user);

            //Tar bort
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Sätt ny role
            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction("GetAllUsers");
        }
        #endregion

    }
}
