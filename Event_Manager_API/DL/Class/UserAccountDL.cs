using Event_Manager_API.Database;
using Event_Manager_API.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Event_Manager_API.Model;
namespace Event_Manager_API.DL.Class
{
    public class UserAccountDL: DatabaseConnector, IUserAccountDL
    {
        DatabaseConnector _dbConnector;

        public UserAccountDL() 
        { 
            _dbConnector = new DatabaseConnector();
        }


        public bool CheckDuplicate<T>(string nameProperty, string valueProperty)
        {
            return _dbConnector.CheckExist<T>(nameProperty, valueProperty);
        }

        public bool CheckLogin(string userId, string userPassword)
        {
            return _dbConnector.CheckLogin<UserAccount>(userId, userPassword);
        }

        public int DeleteUserAccount(string userId)
        {
            return _dbConnector.DeleteById<UserAccount>(userId);
        }

        public int InsertUserAccount(UserAccount userAccount)
        {
            return _dbConnector.Insert<UserAccount>(userAccount);
        }

        public int UpdateUserAccount(UserAccount userAccount)
        {
            return _dbConnector.Update<UserAccount>(userAccount);
        }
    }
}
