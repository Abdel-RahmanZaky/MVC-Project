using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Models;
using Project.PL.Helpers;
using Project.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
			, IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string email)
		{

			if (string.IsNullOrWhiteSpace(email))
			{
				var users = await _userManager.Users.Select( U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(email);

				if(user is not null)
                {
                    var mappedUser = new UserViewModel()
                    {
                        Id = user.Id,
                        FName = user.FName,
                        LName = user.LName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(user).Result
                    };

                    return View(new List<UserViewModel>() { mappedUser });
                }
                return View(Enumerable.Empty<UserViewModel>());
			}			
		}

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

			if(user is null)
				return NotFound();


          var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);

            return View(viewName, mappedUser);
        }

        // /User/Edit/Guid
        // /User/Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
        {
            if (updatedUser.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    user.FName = updatedUser.FName;
                    user.LName = updatedUser.LName;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                    //user.Email = updatedUser.Email;
                    //user.SecurityStamp = Guid.NewGuid().ToString();

                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(updatedUser);
        }

        // /Usser/Delete/Guid
        // /User/Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
