using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.BL.Interface
{
    public interface IUserAccountBL
    {
        ServiceResult InsertUserAccount(UserAccount userAccount);
        ServiceResult UpdateUserAccount(UserAccount userAccount);
        ServiceResult DeleteUserAccount(UserAccount userAccount);
        ServiceResult Login(string id, string password);
    }
}
