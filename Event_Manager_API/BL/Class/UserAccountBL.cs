using Event_Manager_API.BL.Interface;
using Event_Manager_API.DL.Interface;
using Event_Manager_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using System.Text.RegularExpressions;
using Event_Manager_API.Properties;

namespace Event_Manager_API.BL.Class
{
    public class UserAccountBL: IUserAccountBL
    {
        IUserAccountDL _userAccountDL;
        ServiceResult _serviceResult;

        public UserAccountBL(IUserAccountDL userAccountDL)
        {
            _userAccountDL = userAccountDL;
            _serviceResult = new ServiceResult();
            _serviceResult.Code = ServiceCode.Success;
        }

        public ServiceResult InsertUserAccount(UserAccount userAccount)
        {
            ValidateObject(userAccount);
            if(_serviceResult.Code == ServiceCode.BadRequest)
            {
                return _serviceResult;
            }
            _serviceResult.Data = _userAccountDL.InsertUserAccount(userAccount);
            _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
            return _serviceResult;
        }

        public ServiceResult Login(string id, string password)
        {
            UserAccount newUserAccount = new UserAccount()
            {
                UserId = id,
                UserPassword = password,
                UserName = ""
            };

            ValidateObjectLogin(newUserAccount);
            
            if(_serviceResult.Code == ServiceCode.BadRequest)
            {
                return _serviceResult;
            }
            var check = _userAccountDL.CheckLogin(id, password);
            if(check == false)
            {
                _serviceResult.Messenger.Add(Resource.Msg_UnknowAccount);
                _serviceResult.Code = ServiceCode.BadRequest;
                return _serviceResult;
            }
            _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
            return _serviceResult;
        }

        public ServiceResult UpdateUserAccount(UserAccount userAccount)
        {
            throw new NotImplementedException();
        }
        public ServiceResult DeleteUserAccount(UserAccount userAccount)
        {
            throw new NotImplementedException();
        }

        //private method

        private void ValidateObjectLogin(UserAccount userAccount)
        {
            var properties = typeof(UserAccount).GetProperties();
            //Kiểm tra các điều kiện đúng đắn về mặt dữ liệu
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(userAccount);
                var propertyName = property.Name;

                //trim dữ liệu
                if (property.PropertyType.Name == "String")
                {
                    if (propertyValue == null) propertyValue = "";
                    propertyValue = propertyValue.ToString().Trim();
                }

                //Kiểm tra bắt buộc nhập
                if (property.IsDefined(typeof(Required), true) && (propertyValue == null || propertyValue.ToString() == string.Empty))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(Required), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var errorMessenger = (requiredAttribute as Required).Messenger;
                        _serviceResult.Messenger.Add(errorMessenger);
                    }
                    _serviceResult.Code = ServiceCode.BadRequest;
                }
                //Kiểm tra độ dài
                //1.Kiểm tra độ dài lớn nhất
                if (property.IsDefined(typeof(MaxLength), true))
                {
                    var maxLengthAttribute = property.GetCustomAttributes(typeof(MaxLength), true).FirstOrDefault();
                    if (maxLengthAttribute != null)
                    {
                        var errorMessenger = (maxLengthAttribute as MaxLength).Messenger;
                        var lengthOfPropertyValue = propertyValue.ToString().Length;
                        if (lengthOfPropertyValue > (maxLengthAttribute as MaxLength).LengthMax)
                        {
                            _serviceResult.Messenger.Add(errorMessenger);
                            _serviceResult.Code = ServiceCode.BadRequest;
                        }
                    }
                }

                //2.Kiểm tra độ dài nhỏ nhất
                if (property.IsDefined(typeof(MinLength), true))
                {
                    var minLengthAttribute = property.GetCustomAttributes(typeof(MinLength), true).FirstOrDefault();
                    if (minLengthAttribute != null)
                    {
                        var errorMessenger = (minLengthAttribute as MinLength).Messenger;
                        var lengthOfPropertyValue = propertyValue.ToString().Length;
                        if (lengthOfPropertyValue < (minLengthAttribute as MinLength).LengthMin)
                        {
                            _serviceResult.Messenger.Add(errorMessenger);
                            _serviceResult.Code = ServiceCode.BadRequest;
                        }
                    }
                }
            }
        }

        private void ValidateObject(UserAccount userAccount)
        {
            var properties = typeof(UserAccount).GetProperties();
            //Kiểm tra các điều kiện đúng đắn về mặt dữ liệu
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(userAccount);
                var propertyName = property.Name;

                //trim dữ liệu
                if (propertyName == "string")
                {
                    propertyValue = propertyValue.ToString().Trim();
                }

                //Kiểm tra bắt buộc nhập
                if (property.IsDefined(typeof(Required), true) && (propertyValue == null || propertyValue.ToString() == string.Empty))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(Required), true).FirstOrDefault();
                    if (requiredAttribute != null)
                    {
                        var errorMessenger = (requiredAttribute as Required).Messenger;
                        _serviceResult.Messenger.Add(errorMessenger);
                    }
                    _serviceResult.Code = ServiceCode.BadRequest;
                }

                //Kiểm tra trùng lặp
                if (property.IsDefined(typeof(Duplicate), true))
                {
                    var duplicateAttribute = property.GetCustomAttributes(typeof(Duplicate), true).FirstOrDefault();
                    if (duplicateAttribute != null)
                    {
                        var errorMessenger = (duplicateAttribute as Duplicate).Messenger;
                        var checkDuplicate = _userAccountDL.CheckDuplicate<UserAccount>(propertyName, propertyValue.ToString());
                        if (checkDuplicate)
                        {
                            _serviceResult.Messenger.Add(errorMessenger);
                            _serviceResult.Code = ServiceCode.BadRequest;
                        }
                    }
                }

                //Kiểm tra độ dài
                //1.Kiểm tra độ dài lớn nhất
                if (property.IsDefined(typeof(MaxLength), true))
                {
                    var maxLengthAttribute = property.GetCustomAttributes(typeof(MaxLength), true).FirstOrDefault();
                    if (maxLengthAttribute != null)
                    {
                        var errorMessenger = (maxLengthAttribute as MaxLength).Messenger;
                        var lengthOfPropertyValue = propertyValue.ToString().Length;
                        if (lengthOfPropertyValue > (maxLengthAttribute as MaxLength).LengthMax)
                        {
                            _serviceResult.Messenger.Add(errorMessenger);
                            _serviceResult.Code = ServiceCode.BadRequest;
                        }
                    }
                }

                //2.Kiểm tra độ dài nhỏ nhất
                if (property.IsDefined(typeof(MinLength), true))
                {
                    var minLengthAttribute = property.GetCustomAttributes(typeof(MinLength), true).FirstOrDefault();
                    if (minLengthAttribute != null)
                    {
                        var errorMessenger = (minLengthAttribute as MinLength).Messenger;
                        var lengthOfPropertyValue = propertyValue.ToString().Length;
                        if (lengthOfPropertyValue < (minLengthAttribute as MinLength).LengthMin)
                        {
                            _serviceResult.Messenger.Add(errorMessenger);
                            _serviceResult.Code = ServiceCode.BadRequest;
                        }
                    }
                }
            }
        }

    }
}
