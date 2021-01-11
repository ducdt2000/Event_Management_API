using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Model
{
    public class Job
    {
        public Guid JobId { get; set; }

        public string JobIdContains
        {
            get
            {
                return JobId.ToString();
            }
        }

        [Required("Tên công việc")]
        public string JobName { get; set; }

        [Required("Ngày diễn ra")]
        public DateTime JobDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public Status JobStatus { get; set; }
        public string UserId { get; set; }
    }
}
