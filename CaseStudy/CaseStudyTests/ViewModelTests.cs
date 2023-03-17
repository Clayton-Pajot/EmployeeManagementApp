using Xunit;
using HelpdeskViewModels;
using HelpDeskDAL;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CaseStudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task CallCompTest()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();

            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;

            evm.Email = "bs@abc.com";
            await evm.GetByEmail();
            cvm.EmpId = evm.Id;

            evm.Email = "cp@abc.com";
            await evm.GetByEmail();
            cvm.TechId = evm.Id;

            pvm.Description = "Memory Upgrade";
            await pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;

            cvm.Notes = "Bigshot has bad RAM, " + cvm.EmpName + " to fix";
            await cvm.Add();

            Debug.WriteLine("New Call generated - ID: " + cvm.Id);
            int id = cvm.Id;
            await cvm.GetById();

            cvm.Notes += "\n" + cvm.EmpName + " ordered new RAM.";

            if(await cvm.Update() == UpdateStatus.Ok)
            {
                Debug.WriteLine("Call was updated.");
            }
            else
            {
                Debug.WriteLine("Call not updated.");
            }

            cvm.Notes = "Another change to comments that shouldn't work.";


        }

        [Fact]
        public async Task Employee_GetByEmail()
        {
            //EmployeeViewModel vm = null;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel();
                vm.Email = "bs@abc.com" ;
                await vm.GetByEmail();
                Assert.NotNull(vm.Email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = null;
            try
            {
                vm = new EmployeeViewModel { Id = 2 };
                await vm.GetById();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel();
                
                employees = await vm.GetAll();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            //Assert.True(vm.Phoneno == "(777) 777-7700");
            Assert.NotNull(employees);
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            //EmployeeViewModel vm = null;
            //int result = -1;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel
                {
                    Title = "Mr.",
                    Firstname = "Tony",
                    Lastname = "Stark",
                    Email = "iam@ironman.avn",
                    Phoneno = "999-888-7777",
                    DepartmentId = 100
                };
                await vm.Add();
                Assert.True(vm.Id>1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ADD TEST Error: " + ex.Message);
            }
            //Assert.True(vm.Phoneno == "(777) 777-7700");
            //Assert.True(result > -1);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            //EmployeeViewModel vm = null;
            UpdateStatus result = UpdateStatus.Failed;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel { Email = "iam@ironman.avn" };
                await vm.GetByEmail();
                vm.Phoneno = vm.Phoneno == "999-888-7777" ? "(111) 111-1111" : "999-888-7777";
                result = await vm.Update();
                Assert.True(result == UpdateStatus.Ok);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            //Assert.True(vm.Phoneno == "(777) 777-7700");
            
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            //EmployeeViewModel vm = null;
            int result = -1;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel { Email = "iam@ironman.avn" };
                await vm.GetByEmail();
                result = await vm.Delete();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            //Assert.True(vm.Phoneno == "(777) 777-7700");
            Assert.True(result > -1);
        }
    }
}
