using CoreApiResponse;
using Employee.Dtos.Employee;
using Employee.Extensions;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Employee.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _empRepo;
        private readonly IDesignationRepository _desigRepo;
        private readonly UserManager<AppUser> _userManager;
        public EmployeeController(ILogger<EmployeeController> logger,IEmployeeRepository empRepo,
            IDesignationRepository desigRepo, UserManager<AppUser> userManager)
        {
                _desigRepo = desigRepo;
                _logger = logger;
                _empRepo = empRepo;
                _userManager = userManager;
        }


        [HttpGet]
        [Route("all-employee")]
        public async Task<IActionResult> getPaginateEmployee(string? searchTerm, string? sortColumn, string? sortDirection, int page = 1, int limit = 10)
        {
            try
            {
                var Employees = await _empRepo.GetEmployeeAsync(page, limit, searchTerm, sortColumn, sortDirection);

                if (Employees.CountData.TotalRecords > 0)
                {
                    return CustomResult("Data loaded successfully", Employees, HttpStatusCode.OK);
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

        [HttpGet("{employeeId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int employeeId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);
                var Employee = await _empRepo.GetEmployeeByIdAsync(employeeId);
                if (Employee == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                return CustomResult("Data loaded successfully", Employee, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpDelete]
        [Route("{employeeId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int employeeId)
        {
            try
            {
                var Employee = await _empRepo.GetEmployeeByIdAsync(employeeId);
                if (Employee == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var EmployeeModel = await _empRepo.DeleteEmployeeAsync(employeeId);

                if (EmployeeModel == false)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequestDto empDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);
                var username = User.GetUsername();
                var appUser = await _userManager.FindByNameAsync(username);
                if (appUser == null)
                    return CustomResult("User not found", HttpStatusCode.BadRequest);
            //    return CustomResult("User Id found", appUser.Id, HttpStatusCode.OK);
                var Designation = await _desigRepo.GetDesignationByIdAsync(empDto.designationId);
                if (Designation == null)
                {
                    return CustomResult("Designation not found", HttpStatusCode.NotFound);
                }
                var employeeDetails = await _empRepo.GetEmployeeByUserAsync(appUser.Id);
                if (employeeDetails != null)
                    return CustomResult("employee allready exit", HttpStatusCode.BadRequest);
                var employeeModel = new EmployeeModel {
                    AppUserId = appUser.Id,
                    designationId = empDto.designationId,
                    contactNo = empDto.contactNo,
                    city = empDto.city,
                    state = empDto.state,
                    address = empDto.address,
                    altContactNo = empDto.altContactNo,
                    pincode = empDto.pincode,
                    employeeRole = empDto.employeeRole
                };
               // return CustomResult("emp data found", employeeModel, HttpStatusCode.OK);
                var result = await _empRepo.AddEmployeeAsync(employeeModel);
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
        [HttpPut]
        [Route("{employeeId:int}")]
        public async Task<IActionResult> Update([FromRoute] int employeeId, [FromBody] CreateEmployeeRequestDto empDto)
        {
            try
            {

                var Employee = await _empRepo.GetEmployeeByIdAsync(employeeId);
                if (Employee == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var EmployeeModel = new EmployeeModel
                {
                    employeeId = employeeId,
                    designationId = empDto.designationId,
                    contactNo = empDto.contactNo,
                    city = empDto.city,
                    state = empDto.state,
                    address = empDto.address,
                    altContactNo = empDto.altContactNo,
                    pincode = empDto.pincode,
                    employeeRole = empDto.employeeRole
                };
                var updatedEmployee = await _empRepo.UpdateEmployeeAsync(EmployeeModel);
                if (updatedEmployee == false)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                return CustomResult("Data Updated successfully", updatedEmployee, HttpStatusCode.OK);
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
