using CoreApiResponse;
using Employee.Dtos.Department;
using Employee.Interfaces;
using Employee.Models;
using Employee.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentRepository _deptRepo;
        public DepartmentController(IDepartmentRepository deptRepo,ILogger<DepartmentController> logger)
        {
            _deptRepo = deptRepo;
            _logger = logger;
                
        }
        [HttpGet]
        [Route("all-department")]
        public async Task<IActionResult> GetPaginateDepartment(string? searchTerm, string? sortColumn, string? sortDirection, int page = 1, int limit = 10)
        {
            try
            {
                var Departments = await _deptRepo.GetDepartmentAsync(page, limit, searchTerm, sortColumn, sortDirection);
              
                if (Departments.CountData.TotalRecords > 0)
                {
                    return CustomResult("Data loaded successfully", Departments, HttpStatusCode.OK);
                }
                else
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpGet("{departmentId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int departmentId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);
                var department = await _deptRepo.GetDepartmentByIdAsync(departmentId);
                if (department == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                return CustomResult("Data loaded successfully", department, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequestDto deptDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);

              var  departmentDetails = await _deptRepo.GetDepartmentByNameAsync(deptDto.departmentName);

                if (departmentDetails != null)
                    return CustomResult("Department name allready exit",  HttpStatusCode.BadRequest);
                var departmentModel = new Models.Department
                {
                    departmentName = deptDto.departmentName,

                    isActive = deptDto.isActive
                };
                var result = await _deptRepo.AddDepartmentAsync(departmentModel);
                if (!result)
                    return CustomResult("Could not save data", HttpStatusCode.BadRequest);
                //  var result = CreatedAtAction(nameof(GetById), new { id = brandModel.Id }, brandModel.branddto());
                return CustomResult("Data added successfully", result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpDelete]
        [Route("{departmentId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int departmentId)
        {
            try
            {
                var department = await _deptRepo.GetDepartmentByIdAsync(departmentId);
                if (department == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var departmentModel = await _deptRepo.DeleteDepartmentAsync(departmentId);

                if (departmentModel == false)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                return CustomResult("Data delete successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
       

        [HttpPut]
        [Route("{departmentId:int}")]
        public async Task<IActionResult> Update([FromRoute] int departmentId, [FromBody] UpdateDepartmentRequestDto deptDto)
        {
            try
            {

                var department = await _deptRepo.GetDepartmentByIdAsync(departmentId);
                if (department == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var departmentModel = new Department
                {
                    departmentId = departmentId,
                    departmentName = deptDto.departmentName,
                    isActive = deptDto.isActive
                };
                var updateddepartment = await _deptRepo.UpdateDepartmentAsync(departmentModel);
                if (updateddepartment == false)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                return CustomResult("Data Updated successfully", updateddepartment, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

    }
}
