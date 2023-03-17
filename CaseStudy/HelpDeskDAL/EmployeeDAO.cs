using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL
{
    

    public class EmployeeDAO
    {
        //----------------------------  REPOSITORY --------------------------------------------------------
        readonly IRepository<Employee> repository;

        public EmployeeDAO()
        {
            {
                repository = new HelpDeskRepository<Employee>();
            }
        }

        //----------------------------GET BY LAST NAME--------------------------------------------------------
        public async Task<Employee> GetByLastname(string name)
        {
            Employee selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                selectedEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.LastName == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee;
        }//end of getbylastname

        //----------------------------GET BY Email--------------------------------------------------------
        public async Task<Employee> GetByEmail(string email)
        {
            Employee selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                selectedEmployee = await repository.GetOne(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee;
        }//end of getbyEmail

        //---------------------------- GET BY ID --------------------------------------------------------
        public async Task<Employee> GetByID(int id)
        {
            Employee selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                selectedEmployee = await repository.GetOne(emp => emp.Id == id); //table is still called Employees
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee;
        }//end of getbyID

        //---------------------------- GETALL --------------------------------------------------------
        public async Task<List<Employee>> GetAll()
        {
            List<Employee> allEmployees = new List<Employee>();
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                allEmployees = await repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allEmployees;
        }//end of getALL

        //---------------------------- ADD Employee --------------------------------------------------------
        public async Task<int> Add(Employee newEmployee)
        {
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                await repository.Add(newEmployee);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return newEmployee.Id;
        }//end of ADD

        //---------------------------- UPDATE Employee --------------------------------------------------------
        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            UpdateStatus opstat = UpdateStatus.Failed;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                Employee currentEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == updatedEmployee.Id);

                _db.Entry(currentEmployee).OriginalValues["Timer"] = updatedEmployee.Timer;
                _db.Entry(currentEmployee).CurrentValues.SetValues(updatedEmployee);
                if (await _db.SaveChangesAsync() == 1)
                {
                    opstat = UpdateStatus.Ok;
                }
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                opstat = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return opstat;
        }//end of UPDATE

        //---------------------------- DELETE Employee --------------------------------------------------------
        public async Task<int> Delete(int id)
        {
            int employeesDeleted = -1;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                Employee currentEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
                _db.Employees.Remove(currentEmployee);
                employeesDeleted = await repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return employeesDeleted;
        }//end of DELETE
    }
}
