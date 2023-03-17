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
    public class CallViewModel
    {
        private CallDAO _dao;

        public int Id { get; set; }
        public int EmpId { get; set; }
        public int ProblemId { get; set; }
        public string EmpName { get; set; }
        public string ProblemDescription { get; set; }
        public string TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }


        public CallViewModel()
        {
            _dao = new CallDAO();
        }
        
        public async Task GetByDescription()
        {
           /* try
            {
                Call call = await _dao.GetCallById
            }*/
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Call call = new Call
                {
                    EmployeeId = EmpId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateTime.Now,
                    DateClosed = null,
                    OpenStatus = true,
                    Notes = ""
                };
                Id = await _dao.Add(call);


                call.Timer = Convert.FromBase64String(Timer);//This was added to attempt to bypass the POST error
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }//end of Add
        

        public async Task<UpdateStatus> Update()
        {
            UpdateStatus opstat = UpdateStatus.Failed;
            try
            {
                Call call = new Call
                {
                    EmployeeId = EmpId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes
                };
                Id = await _dao.Add(call);

                opstat = await _dao.Update(call);
                call.Timer = Convert.FromBase64String(Timer);//This was added to attempt to bypass the POST error
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return opstat;
        }//end of Update


        public async Task GetById()
        {
            try
            {
                Call call = await _dao.GetCallById(Id);
                Id = call.Id;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Id = -1;

            }
            catch (Exception ex)
            {
                Id = -1;
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }
    }
}
