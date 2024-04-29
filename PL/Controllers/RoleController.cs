using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager ,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
      
        public async Task<IActionResult> Index(string SearchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();

            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name
                                   .ToLower()
                                   .Contains(SearchInput.ToLower()))
                                   .Select(R => new RoleViewModel()
                                   {
                                       Id = R.Id,
                                       RoleName = R.Name
                                   }).ToListAsync();


            }


            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {

            if(ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName,
                };
                var Result = await _roleManager.CreateAsync(role);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest(); //400

            var roleFromdb = await _roleManager.FindByIdAsync(id);

            if (roleFromdb == null)
                return NotFound();

            var role = new RoleViewModel()
            {
                Id = roleFromdb.Id,
                RoleName = roleFromdb.Name
            };

            return View(viewname, role);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            //ViewData["Departments"] = _departmentRepositry.GetAll(); //All Departments

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var roleFromdb = await _roleManager.FindByIdAsync(id);

                if (roleFromdb == null)
                    return NotFound();

                roleFromdb.Name = model.RoleName;

                await _roleManager.UpdateAsync(roleFromdb);

                return RedirectToAction(nameof(Index));

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
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var roleFromdb = await _roleManager.FindByIdAsync(id);

                if (roleFromdb == null)
                    return NotFound();

                roleFromdb.Name = model.RoleName;

                await _roleManager.DeleteAsync(roleFromdb);

                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return NotFound();

            ViewData["RoleId"] = RoleId;
            var UsersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await _userManager.IsInRoleAsync(user,role.Name))
                    UserInRole.IsSelected = true;
                else 
                    UserInRole.IsSelected = false;

                UsersInRole.Add(UserInRole);
            }

            return View(UsersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId,List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);
                    if (AppUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(AppUser,role.Name))
                        {


                            await _userManager.AddToRoleAsync(AppUser,role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        }
                    }
                    
                }
                return RedirectToAction(nameof(Edit), new {id = RoleId});

            }
            return View(users);
        }

    }
}
