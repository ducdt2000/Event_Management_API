using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.DL.Interface
{
    public interface IJobDL
    {
        public int InsertJob(Job job);
        public int UpdateJob(Job job);
        public int DeleteJob(string jobId);
        public IEnumerable<Job> GetJobs(string userAccount);
        public Job GetJobById(string jobId);
        public bool CheckDuplicateUpdate<T>(string nameProperty, T valueProperty, string id);
    }
}
