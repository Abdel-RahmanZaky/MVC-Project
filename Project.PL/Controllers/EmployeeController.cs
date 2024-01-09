using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interfaces;
using Project.DAL.Models;
using Project.PL.Helpers;
using Project.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmployeeRepository _employeeRepo;
        //private readonly IDepartmentRepository _departmentRepos; 



        public EmployeeController(IMapper mapper , IUnitOfWork unitOfWork
            /*,IEmployeeRepository employee*//* IDepartmentRepository departmentRepos*/) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_employeeRepo = employee;
            //_departmentRepos = departmentRepos;
        }

        // /Employee/Index
        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(searchInput))
                 employees = await _unitOfWork.EmployeeRepository.GetAll();
            else
                employees =  _unitOfWork.EmployeeRepository.SearchByName(searchInput.ToLower());

            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmp);
        }

        [HttpGet]
        // Employee/Create
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepos.GetAll();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                // Manual Mapping
                ///var mappedEmp = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Address = employeeVM.Address,
                ///    Email = employeeVM.Email,
                ///    PhoneNumber = employeeVM.PhoneNumber,
                ///    IsActive = employeeVM.IsActive,
                ///    Salary = employeeVM.Salary,
                ///    Age = employeeVM.Age,
                ///    HireDate = employeeVM.HireDate
                ///};

                //Employee employee = (Employee)employeeVM;

                employeeVM.ImageName = DocumentSettings.UpLoadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map< EmployeeViewModel, Employee> (employeeVM);

                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                var Count = await _unitOfWork.Complete();
                if (Count > 0)
                {
                    TempData["Message"] = "Department is Created Succssfuly :)";
                }
                else
                {
                    TempData["Messgae"] = "An Error Has Occured, Department Not Created :(";
                }
                    return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            return View(viewName, mappedEmp);
        }

        // /Employee/Edit/1
        // /Employee/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ///if(!id.HasValue)
            ///    return BadRequest();
            ///var Employee = _EmployeeRepo.Get(id.Value);
            ///if (Employee is  null)
            ///    return NotFound();
            ///return View(Employee);
            //ViewData["Departments"] = _departmentRepos.GetAll();

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (employeeVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    var count = await _unitOfWork.Complete();
                   

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        // /Employee/Delete/1
        // /Employee/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }
}
