using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Model
{
    public class ServiceResult
    {
        public object Data { get; set; }

        public List<string> Messenger { get; set; } = new List<string>();

        public ServiceCode Code { get; set; }

    }
}
