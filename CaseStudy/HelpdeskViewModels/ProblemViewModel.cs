using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private ProblemDAO _dao;

        public string Description { get; set; }
        public int Id { get; set; }
        public string Timer { get; set; }

        public async Task GetByDescription()
        {
            try
            {
                Problem prob = await _dao.GetByDescription(Description);
                Description = prob.Description;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";

            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }
        /*public async Task<ProblemViewModel> GetByDescription()
        {
            ProblemViewModel probVM = null;
            try
            {
                Problem prob = await _dao.GetByDescription(Description);
                Description = prob.Description;
                probVM.Description = prob.Description;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";

            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return probVM;
        }*/

        public async Task <List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> allProbVms = new List<ProblemViewModel>();
            try
            {
                List<Problem> allProbs = await _dao.GetAll();
                foreach (Problem prob in allProbs)
                {
                    ProblemViewModel probVM = new ProblemViewModel();
                    probVM.Description = prob.Description;
                    allProbVms.Add(probVM);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allProbVms;
        }


    }

    
}
