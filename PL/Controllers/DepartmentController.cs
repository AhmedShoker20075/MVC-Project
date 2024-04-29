using Microsoft.AspNetCore.Mvc;
using BLL.Repositries;
using BLL.Interfaces;
using DAL.Models;
using AutoMapper;
using System.Collections;
using PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private IDepartmentRepositry _departmentRepositry;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork/*IDepartmentRepositry departmentRepositry*/,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_departmentRepositry = departmentRepositry;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index( string SearchInput)
        {
            var department = Enumerable.Empty<Department>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                //department = _departmentRepositry.GetAll();
                department = await _unitOfWork.DepartmentRepositry.GetAll();
            }
            else
            {
                //department = _departmentRepositry.GetByName(SearchInput.ToLower());
                department = await _unitOfWork.DepartmentRepositry.GetByName(SearchInput.ToLower());
            }

            var result = _mapper.Map<IEnumerable<DepartmentViewModel>>(department);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);

                //int count = _departmentRepositry.Add(department);
                 _unitOfWork.DepartmentRepositry.Add(department);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
               
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id , string viewname ="Details")
        {
            if (id is null)
                return BadRequest(); //400
            
            //var department = _departmentRepositry.Get(id.Value);
            var department = await _unitOfWork.DepartmentRepositry.Get(id.Value);

            var result = _mapper.Map<DepartmentViewModel>(department);

            if (department == null)
                return NotFound(); //404
            return View(viewname, result);
            
          
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id , DepartmentViewModel department) 
        {
            if(id != department.Id)
                return BadRequest();

            var result = _mapper.Map<Department>(department);

            if (ModelState.IsValid) //server side validation
            {
                //var count = _departmentRepositry.Update(result);
                _unitOfWork.DepartmentRepositry.Update(result);
                int count = await _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, DepartmentViewModel department)
        {
            if (id != department.Id)
                return BadRequest();

            var result = _mapper.Map<Department>(department);

            if (ModelState.IsValid) //server side validation
            {
                //var count = _departmentRepositry.Delete(result);
                _unitOfWork.DepartmentRepositry.Delete(result);
                int count = await _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);
        }

    }
}
