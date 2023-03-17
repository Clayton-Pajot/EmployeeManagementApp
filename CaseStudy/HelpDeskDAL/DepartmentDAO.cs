using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;


namespace HelpDeskDAL
{
    public class DepartmentDAO
    {
        readonly IRepository<Department> repository;

        public DepartmentDAO()
        {
            repository = new HelpDeskRepository<Department>();
        }
        public async Task<List<Department>> GetAll()
        {



            List<Department> allDepartments = new List<Department>();
            try
            {
                allDepartments = await repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }

            return allDepartments;
        }
    }
}
