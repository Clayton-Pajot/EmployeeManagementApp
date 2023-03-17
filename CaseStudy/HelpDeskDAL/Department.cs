using System;
using System.Collections.Generic;

namespace HelpDeskDAL
{
    public partial class Department : HelpDeskEntity
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public string? DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
