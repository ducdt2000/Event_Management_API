using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.BL.Interface
{
    public interface IJobBL
    {
        ServiceResult InsertJob(Job job);
        ServiceResult GetJobs(string userId);
        ServiceResult GetJob(string jobId);
        ServiceResult UpdateJob(Job job);
        ServiceResult DeleteJob(string jobId);
    }
}
