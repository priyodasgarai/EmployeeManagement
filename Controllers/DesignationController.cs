using CoreApiResponse;
using Employee.Dtos.Designation;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : BaseController
    {
        private readonly IDesignationRepository _desigRepo;
        private readonly ILogger<DesignationController> _logger;
        private readonly IDepartmentRepository _deptRepo;
        public DesignationController(ILogger<DesignationController> logger, IDepartmentRepository deptRepo, IDesignationRepository desigRepo)
        {
                _deptRepo = deptRepo;
                _desigRepo = desigRepo;
                _logger = logger;
        }
        [HttpGet]
        [Route("all-Designation")]
        public async Task<IActionResult> GetAllDesignation()
        {
            try
            {
                var Designations = await _desigRepo.GetDesignationAsync();

                if (Designations != null)
                {
                    return CustomResult("Data loaded successfully", Designations, HttpStatusCode.OK);
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
        [HttpGet("{designationId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int designationId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);
                var Designation = await _desigRepo.GetDesignationByIdAsync(designationId);
                if (Designation == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                return CustomResult("Data loaded successfully", Designation, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDesignationRequestDto DesigDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResult("One or more validation errors occurred", ModelState, HttpStatusCode.BadRequest);

                var department = await _deptRepo.GetDepartmentByIdAsync(DesigDto.departmentId);
                if (department == null)
                {
                    return CustomResult("Department name not found", HttpStatusCode.NotFound);
                }

                var DesignationDetails = await _desigRepo.GetDesignationByNameAsync(DesigDto.departmentId,DesigDto.designationName);

                if (DesignationDetails != null)
                    return CustomResult("Designation name allready exit", HttpStatusCode.BadRequest);
                var DesignationModel = new Designation
                {
                    designationName = DesigDto.designationName,
                    departmentId = DesigDto.departmentId
                };
                var result = await _desigRepo.AddDesignationAsync(DesignationModel);
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
        [Route("{designationId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int designationId)
        {
            try
            {
                var Designation = await _desigRepo.GetDesignationByIdAsync(designationId);
                if (Designation == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var DesignationModel = await _desigRepo.DeleteDesignationAsync(designationId);

                if (DesignationModel == false)
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
        [Route("{designationId:int}")]
        public async Task<IActionResult> Update([FromRoute] int designationId, [FromBody] CreateDesignationRequestDto desigRepo)
        {
            try
            {

                var Designation = await _desigRepo.GetDesignationByIdAsync(designationId);
                if (Designation == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                var department = await _deptRepo.GetDepartmentByIdAsync(desigRepo.departmentId);
                if (department == null)
                {
                    return CustomResult("Department name not found", HttpStatusCode.NotFound);
                }

                var DesignationDetails = await _desigRepo.GetDesignationByNameAsync(desigRepo.departmentId, desigRepo.designationName);

                if (DesignationDetails != null)
                    return CustomResult("Designation name allready exit", HttpStatusCode.BadRequest);

                var DesignationModel = new Designation
                {
                    designationId = designationId,
                    designationName = desigRepo.designationName,
                    departmentId = desigRepo.departmentId
                };
                var updatedDesignation = await _desigRepo.UpdateDesignationAsync(DesignationModel);
                if (updatedDesignation == false)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                return CustomResult("Data Updated successfully", updatedDesignation, HttpStatusCode.OK);
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
