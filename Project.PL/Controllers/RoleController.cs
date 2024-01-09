using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.DAL.Models;
using Project.PL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Project.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Roles/Index
        public async Task<IActionResult> Index(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                var users = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
                return View(users);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);

                if(role is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };

                    return View(new List<RoleViewModel>() { mappedRole });
                }
                return View(Enumerable.Empty<RoleViewModel>()); 
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

        // Roles/Details/Guid
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();


            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(viewName, mappedRole);
        }

        // /Roles/Edit/Guid
        // /Roles/Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (updatedRole.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = updatedRole.RoleName;
                   

                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(updatedRole);
        }

        // /Roles/Delete/Guid
        // /Role/Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(role);
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
