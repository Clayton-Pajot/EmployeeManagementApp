using System;
using HelpDeskDAL;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskViewModels
{
    public class DepartmentViewModel
    {
        readonly private DepartmentDAO _dao;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Timer { get; set; }

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();

            try
            {

                List<Department> allDepartments = await _dao.GetAll();
                foreach (Department depts in allDepartments)
                {
                    DepartmentViewModel deptsVm = new DepartmentViewModel
                    {

                        Name = depts.DepartmentName,

                        Id = depts.Id,

                        Timer = Convert.ToBase64String(depts.Timer)
                    };
                    allVms.Add(deptsVm);

                }

            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Name = "not foud";
            }
            catch (Exception ex)
            {
                Name = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }
    }
}
