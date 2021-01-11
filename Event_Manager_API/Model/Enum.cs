using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Model
{
    public enum ServiceCode
    {
        BadRequest = 400,
        Success = 200,
        Excaption = 500
    }

    public enum Status
    {
        DONE = 1,
        DOING = 2,
        COMING = 3,
        MISSED = 4
    }
}
