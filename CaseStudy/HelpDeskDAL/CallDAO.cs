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
    public class CallDAO
    {
        readonly IRepository<Call> repo;

        public CallDAO()
        {
            repo = new HelpDeskRepository<Call>();
        }

        public async Task<List<Call>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<Call> GetCallById(int id)
        {
            return await repo.GetOne(call => call.Id == id);
        }

        public async Task<int> Add(Call newCall)
        {
            return (await repo.Add(newCall)).Id;
        }

        public async Task<UpdateStatus> Update(Call call)
        {
            return await repo.Update(call);
        }

        public async Task<int> Delete(int id)
        {
            return await repo.Delete(id);
        }
    }
}
