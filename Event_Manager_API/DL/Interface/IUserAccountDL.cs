using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.DL.Interface
{
    public interface IUserAccountDL
    {
        public bool CheckDuplicate<T>(string nameProperty, string valueProperty);
        public int InsertUserAccount(UserAccount userAccount);
        public int UpdateUserAccount(UserAccount userAccount);
        public int DeleteUserAccount(string userId);
        public bool CheckLogin(string userId, string userPassword);
    }
}

