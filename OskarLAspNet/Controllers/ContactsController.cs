using Microsoft.AspNetCore.Mvc;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactFormService _contactFormService;

        public ContactsController(ContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        public IActionResult Index()
        {

            return View();
        }

        #region Should have been named Thank You-page
        public async Task<IActionResult> SubmitContactForm(ContactFormVM viewModel)
        {
            if (ModelState.IsValid)
            {
                await _contactFormService.AddAsync(viewModel);
                

                return RedirectToAction("SubmitContactForm");
            }
            return View(viewModel);
        }
        #endregion
    }
}
