using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.Helper;
using PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
        

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager )
        {
			_userManager = userManager;
          
        }


		public async Task<IActionResult> Index(string SearchInput)
		{
			var users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(SearchInput))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				
			}
			else
			{
			   users = await _userManager.Users.Where(U => U.Email
								  .ToLower()
								  .Contains(SearchInput.ToLower()))
								  .Select(U => new UserViewModel()
								  {
									  Id = U.Id,
									  FirstName = U.FirstName,
									  LastName = U.LastName,
									  Email = U.Email,
									  Roles = _userManager.GetRolesAsync(U).Result
								  }).ToListAsync();

								  				
			}

			
			return View(users);
		}
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest(); //400

			var UserFromdb = await _userManager.FindByIdAsync(id);

			if (UserFromdb == null)
				return NotFound();

			var user = new UserViewModel()
			{
				Id = UserFromdb.Id,
				FirstName = UserFromdb.FirstName,
				LastName = UserFromdb.LastName,
				Email = UserFromdb.Email,
				Roles = _userManager.GetRolesAsync(UserFromdb).Result
			};
           
            return View(viewname, user);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            //ViewData["Departments"] = _departmentRepositry.GetAll(); //All Departments

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if(ModelState.IsValid)
            {

                var UserFromdb = await _userManager.FindByIdAsync(id);

                if (UserFromdb == null)
                    return NotFound();

                UserFromdb.FirstName = model.FirstName;
                UserFromdb.LastName = model.LastName;
                UserFromdb.Email = model.Email;
                var Result = await _userManager.UpdateAsync(UserFromdb);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);       
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var UserFromdb = await _userManager.FindByIdAsync(id);

                if (UserFromdb == null)
                    return NotFound();

                UserFromdb.FirstName = model.FirstName;
                UserFromdb.LastName = model.LastName;
                UserFromdb.Email = model.Email;
                var Result = await _userManager.DeleteAsync(UserFromdb);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }

    }
}
