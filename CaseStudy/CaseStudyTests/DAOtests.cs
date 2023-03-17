using System;
using Xunit;
using Xunit.Abstractions;
using HelpDeskDAL;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpDeskDAO// CaseEmpdyTests
{
    public class DAOtests
    {

        private readonly ITestOutputHelper output;

        public DAOtests(ITestOutputHelper output)
        {
            this.output =  output;
        }



        [Fact]
        public async Task Call_CompTest()
        {
            CallDAO cdao = new CallDAO();
            EmployeeDAO edao = new EmployeeDAO();
            ProblemDAO pdao = new ProblemDAO();

            Employee bigshot = await edao.GetByEmail("bs@abc.com");
            Employee tech = await edao.GetByEmail("cp@abc.com");

            Problem badDrive = await pdao.GetByDescription("Hard Drive Failure");

            Call call = new Call
            {
                DateOpened = DateTime.Now,
                DateClosed = null,
                OpenStatus = true,
                EmployeeId = bigshot.Id,
                TechId = tech.Id,
                ProblemId = badDrive.Id,
                Notes = "Mr.Bigshot's drive is shot, " + tech.FirstName + " to fix it"
            };

            int newCallId = await cdao.Add(call);
            output.WriteLine("New Call Generated: Id = " + newCallId);
            call = await cdao.GetCallById(newCallId);
            byte[] oldTimer = call.Timer;
            output.WriteLine("New Call Retrieved by " + tech.FirstName);
            call.Notes += "\nOrdered new Drive!";

            if (await cdao.Update(call) == UpdateStatus.Ok)
            {
                output.WriteLine("Call was updated: " + call.Notes);
            }
            else
            {
                output.WriteLine("Call was note updated!");
            }

            call.Timer = oldTimer;
            call.Notes = "Doesn't matter, data stale";

            if(await cdao.Update(call) == UpdateStatus.Stale)
            {
                output.WriteLine("Call was note not updated, as data is stale");
            }

            cdao = new CallDAO();

            call = await cdao.GetCallById(newCallId);
             
            if(await cdao.Delete(newCallId) == 1)
            {
                output.WriteLine("Call was deleted.");
            }
            else
            {
                output.WriteLine("Call was not deleted.");
            }

            Assert.Null(await cdao.GetCallById(newCallId));

        }



        [Fact]
        public async Task Emp_PicTest()
        {
            try
            {
                DALUtil util = new DALUtil();
                Assert.True(await util.AddEmployeePicsToDb());
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

        /*//----------------------------GET BY Lastname Test-------------------------------
        [Fact]
        public async Task GetByLastnameTest()
        {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = await dao.GetByLastname("Pajot");
                Assert.NotNull(selectedEmployee);
            *//*try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = await dao.GetByLastname("Iac");
                Assert.True(selectedEmployee.FirstName == "Brain");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("!!ERROR: Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                
            }*//*

        }

        //----------------------------GET BY Email Test-------------------------------
        [Fact]
        public async Task GetByEmailTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = await dao.GetByEmail("bs@abc.com");
                Assert.NotNull(selectedEmployee.FirstName);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("!!ERROR: Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                
            }
        }


        //----------------------------ADD Test-------------------------------
        [Fact]
        public async Task AddTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = new Employee
                {
                    Title = "Mr.",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "iam@ironman.avn",
                    PhoneNo = "999-888-7777",
                    DepartmentId = 10
                };
                    
                await dao.Add(selectedEmployee);
                Assert.True(selectedEmployee.Id > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("!!ERROR: Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }*/

        [Fact]
        public async Task GetByIDTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employee selectedEmployee = await dao.GetByID(11);
            Assert.NotNull(selectedEmployee);
        }

        [Fact]
        public async Task GetAllTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            List<Employee> allEmployees = await dao.GetAll();
            Assert.NotNull(allEmployees);
        }

        [Fact]
        public async Task AddEmployeeTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employee newEmp = new Employee
            {
                Title = "Mr.",
                FirstName = "James",
                LastName = "Bond",
                PhoneNo = "(007) 007-0077",
                Email = "jb@abc.com",
                DepartmentId = 50
            };

            await dao.Add(newEmp);
            Assert.True(newEmp.Id > 0);
        }

        [Fact]
        public async Task UpdateEmployeeTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employee selectedEmp = await dao.GetByEmail("jb@abc.com");
            if (selectedEmp != null)
            {
                string oldFirstName = selectedEmp.FirstName;
                string newFirstName = oldFirstName == "James" ? "Jimothy" : "James";
                selectedEmp.FirstName = newFirstName;
            }
            Assert.True(await dao.Update(selectedEmp) == UpdateStatus.Ok);  //> 0); OLD CODE
        }

        [Fact]
        public async Task DeleteEmployeeTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employee selectedEmp = await dao.GetByLastname("Bond");
            int numDeleted = -1;
            if (selectedEmp != null)
            {
                numDeleted = await dao.Delete(selectedEmp.Id);
            }
            Assert.True(numDeleted > 0);
        }

        [Fact]
        public async Task Emp_Conc_test()
        {
            EmployeeDAO dao1 = new EmployeeDAO();
            EmployeeDAO dao2 = new EmployeeDAO();
            Employee stuToUpdate1 = await dao1.GetByLastname("Bond");
            Employee stuToUpdate2 = await dao2.GetByLastname("Bond");

            if (stuToUpdate1 != null)
            {
                string oldPhoneNo = stuToUpdate1.PhoneNo; ;
                string newPhoneNo = oldPhoneNo == "(007) 007-0077" ? "(777) 777-7700" : "(007) 007-0077";
                stuToUpdate1.PhoneNo = newPhoneNo;
                if (await dao1.Update(stuToUpdate1) == UpdateStatus.Ok)
                {
                    stuToUpdate2.PhoneNo = "666-666-6666";
                    Assert.True(await dao2.Update(stuToUpdate2) == UpdateStatus.Stale);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }


    }
}