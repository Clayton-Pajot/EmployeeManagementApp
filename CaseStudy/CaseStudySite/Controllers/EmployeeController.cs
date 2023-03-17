using HelpdeskViewModels;
using HelpDeskDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Reflection;

namespace CaseStudySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                EmployeeViewModel viewModel = new EmployeeViewModel
                {
                    Email = email
                };
                await viewModel.GetByEmail();
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(EmployeeViewModel viewmodel)
        {
            UpdateStatus retVal = UpdateStatus.Failed;
            try
            {
                retVal = await viewmodel.Update(); 
                return retVal switch
                {
                    UpdateStatus.Ok => Ok(new { msg = "Employee " + viewmodel.Lastname + " updated!" }),
                    UpdateStatus.Failed => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                    UpdateStatus.Stale => Ok(new { msg = "Data is stale for " + viewmodel.Lastname + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(EmployeeViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Employee " + viewmodel.Lastname + " Added!" })
                    : Ok(new { msg = "Employee " + viewmodel.Lastname + " not Added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = await viewmodel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel { Id = id };
                return await viewmodel.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted" })
                    : Ok(new { msg = "Employee " + id + " not deleted" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }//end of delete
    }
}
