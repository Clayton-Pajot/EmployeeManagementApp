using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskDAL;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private EmployeeDAO _dao;

        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Id { get; set; }
        public string StaffPicture64 { get; set; }
        public string ErrorMessage { get; set; }

        //constructor

        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        public async Task GetByEmail()
        {
            try
            {
                Employee emp = await _dao.GetByEmail(Email);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Email = emp.Email;
                Phoneno = emp.PhoneNo;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                DepartmentName = emp.Department.DepartmentName;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "not found";

            }
            catch (Exception ex)
            {
                Email = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }//end of GetByLastname

        //find employee using ID-----------------------------------------------------
        public async Task GetById()
        {
            try
            {
                Employee emp = await _dao.GetByID(Id);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Email = emp.Email;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Id = 0;

            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message);
                throw;
            }

        }//end of GetById

        //find All Employees -----------------------------------------------------
        /*public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();

            try
            {

                List<Employee> allEmployees = await _dao.GetAll();
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        Timer = Convert.ToBase64String(emp.Timer)
                    };
                    allVms.Add(empVm);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }*/
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employee> allEmployees = await _dao.GetAll();
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.Firstname = emp.FirstName;
                    empVm.Lastname = emp.LastName;
                    empVm.Email = emp.Email;
                    empVm.Phoneno = emp.PhoneNo;
                    empVm.Id = emp.Id;
                    empVm.DepartmentId = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.Timer = Convert.ToBase64String(emp.Timer);

                    if (emp.StaffPicture != null)
                    {
                        empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }
                    
                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allVms;

        }//end of GetAll

        //Add Employees -----------------------------------------------------
        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new Employee
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId
                };
                Id = await _dao.Add(emp);

                if(StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }

                emp.Timer = Convert.FromBase64String(Timer);//This was added to attempt to bypass the POST error
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }//end of Add

        //Update Employees -----------------------------------------------------
        public async Task<UpdateStatus> Update()
        {
            UpdateStatus opstat = UpdateStatus.Failed;
            try
            {
                Employee emp = new Employee
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = Id,
                    DepartmentId = DepartmentId
                };

                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                /*

                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }*/

                emp.Timer = Convert.FromBase64String(Timer);
                opstat = await _dao.Update(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return opstat;
        }//end of Update

        //Delete Employees -----------------------------------------------------
        public async Task<int> Delete()
        {
            int employeeDeleted = -1;
            try
            {
                employeeDeleted = await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return employeeDeleted;
        }//end of Delete
    }
}