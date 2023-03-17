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
    public class ProblemDAO
    {
        IRepository<Problem> repository;
        public ProblemDAO()
        {
            repository = new HelpDeskRepository<Problem>();
        }

        public async Task<Problem> GetByDescription(string description)
        {
            Problem problem = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                problem = await _db.Problems.FirstOrDefaultAsync(prob => prob.Description == description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return problem;
        }

        public async Task<Problem> GetById(int id)
        {
            Problem problem = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                problem = await _db.Problems.FirstOrDefaultAsync(prob => prob.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return problem;
        }

        public async Task<List<Problem>> GetAll()
        {
            List<Problem> problems = new List<Problem>();
            try
            {
                HelpDeskContext db = new HelpDeskContext();
                problems = await repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return problems;
        }

    }
}
