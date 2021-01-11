using Event_Manager_API.Database;
using Event_Manager_API.DL.Interface;
using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.DL.Class
{
    public class JobDL:DatabaseConnector, IJobDL
    {
        DatabaseConnector _dbConnector;
        
        public JobDL()
        {
            _dbConnector = new DatabaseConnector();
        }

        public int InsertJob(Job job)
        {
            return _dbConnector.Insert<Job>(job);
        }
        public int UpdateJob(Job job)
        {
            return _dbConnector.Update<Job>(job);
        }
        public int DeleteJob(string jobId)
        {
            return _dbConnector.DeleteById<Job>(jobId);
        }

        public IEnumerable<Job> GetJobs(string userAccount)
        {
            string sql = $"Select * from Job where UserId = '{userAccount}'";
            return _dbConnector.GetManyDataByCommand<Job>(sql);
        }
        public Job GetJobById(string id)
        {
            string sql = $"Select * from Job where JobId = '{id}'";
            return (Job)_dbConnector.GetOneDataByCommand<Job>(sql);
        }

        public bool CheckDuplicateUpdate<T>(string nameProperty, T valueProperty, string id)
        {
            return _dbConnector.CheckExistUpdate<Job, T>(nameProperty, valueProperty, id);
        }
    }
}
