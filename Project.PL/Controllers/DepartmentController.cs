using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interfaces;
using Project.DAL.Models;
using Project.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.PL.Controllers
{
    // Clas has Two Relationships
    // Inhertance : DepartmentController is  a Controller.
    // Compostion : DepartmentController has a IDepartmentRepository
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IDepartmentRepository _DepartmentRepo;


        public DepartmentController( IMapper mapper, IUnitOfWork unitOfWork/*,IDepartmentRepository department*/) // Ask CLR to Create Object form class Implement IDepartmentRepository
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_DepartmentRepo = department;
        }

        // /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAll();

            var mappedDept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>> (departments);
            return View(mappedDept);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                var depatMapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Add(depatMapped);
                var Count = await _unitOfWork.Complete();
                if(Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            
            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);

            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

            if(department is null)
                return NotFound();

            return View(viewName, mappedDept);
        }

        // /Department/Edit/1
        // /Department/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            ///if(!id.HasValue)
            ///    return BadRequest();
            ///var department = _DepartmentRepo.Get(id.Value);
            ///if (department is  null)
            ///    return NotFound();
            ///return View(department);

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,DepartmentViewModel departmentVM) 
        {
            if(departmentVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var depatMapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(depatMapped);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        // /Department/Delete/1
        // /Department/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id , DepartmentViewModel departmentVM)
        {
            try
            {
                var depatMapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(depatMapped);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }
        }
    }
}
