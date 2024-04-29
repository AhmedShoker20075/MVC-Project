using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;
using PL.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
	public class EmployeeController : Controller
    {
        //private IEmployeeRepositry _employeeRepositry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepositry _departmentRepositry;

        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper /*IEmployeeRepositry employeeRepositry,*/ /*,IDepartmentRepositry departmentRepositry*/)
        {
            //_employeeRepositry = employeeRepositry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_departmentRepositry = departmentRepositry;
        }

        public async Task<IActionResult> Index( string SearchInput)
        {
            var employees=Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                //employees = _employeeRepositry.GetAll();
                employees = await _unitOfWork.EmployeeRepositry.GetAll();
            }
            else
            {
                //employees = _employeeRepositry.GetByName(SearchInput.ToLower());
                employees = await _unitOfWork.EmployeeRepositry.GetByName(SearchInput.ToLower());
            }

            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            //view's Dictionary => Transfer Data From Action To View (one way) (key,value)

            //1.ViewData: Property Inherited From Controller Class, Dictionary

            ViewData["Message"] = "Hello ViewData";

            //2.ViewBag: Property Inherited From Controller Class, dynamic

            ViewBag.Message = "Hello ViewBag";

            //1.TempData: Property Inherited From Controller Class , Like ViewData
            //Transfer Information From Request To Another

            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _unitOfWork.DepartmentRepositry.GetAll(); //All Departments
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    HireDate = model.HireDate,
                //    IsActive = model.IsActive,
                //    Salary = model.Salary,
                //    Address = model.Address,
                //    Phone = model.Phone,
                //    DateOfCreate = model.DateOfCreate,  
                //    DepartmentId = model.DepartmentId,
                //    Department = model.Department,
                //    IsDeleted = model.IsDeleted,
                //};

                var employee = _mapper.Map<Employee>(model);

                //int count = _employeeRepositry.Add(employee);
                _unitOfWork.EmployeeRepositry.Add(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added !!";
                }
                else
                {
                    TempData["Message"] = "Employee Added !!";
                }
                return RedirectToAction("Index");

            }
            return View();
        }

        public async Task<IActionResult> Details(int? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest(); //400

            //var employee = _employeeRepositry.Get(id.Value);
            var employee = await _unitOfWork.EmployeeRepositry.Get(id.Value);

            //Manual Mapping

            //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Salary = employee.Salary,
            //    Address = employee.Address,
            //    Phone = employee.Phone,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    HireDate = employee.HireDate,   
            //    DateOfCreate = employee.DateOfCreate,
            //    Department = employee.Department,
            //    DepartmentId = employee.DepartmentId,
            //};

            var result = _mapper.Map<EmployeeViewModel>(employee);

            if (employee == null)
                return NotFound(); //404
            return View(viewname, result);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepositry.GetAll(); //All Departments

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (model.Image is not null)
            {
                if (model.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
            }

            
            //1.Add
            //2.Update
            //3.Delete
            //_unitOfWork.Complete();

            //Employee employee = new Employee()
            //{
            //    Id =model.Id,
            //    Name = model.Name,
            //    Age = model.Age,
            //    Salary = model.Salary,
            //    Address = model.Address,
            //    Phone = model.Phone,
            //    Email = model.Email,
            //    IsActive = model.IsActive,
            //    IsDeleted = model.IsDeleted,    
            //    HireDate = model.HireDate,
            //    DateOfCreate = model.DateOfCreate,
            //    Department = model.Department,  
            //    DepartmentId = model.DepartmentId,
            //};

            var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid) //server side validation
            {
                //var count = _employeeRepositry.Update(employee);
                 _unitOfWork.EmployeeRepositry.Update(employee);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid) //server side validation
            {
                //var count = _employeeRepositry.Delete(employee);
                _unitOfWork.EmployeeRepositry.Delete(employee);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
