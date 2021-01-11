using Event_Manager_API.BL.Interface;
using Event_Manager_API.DL.Interface;
using Event_Manager_API.Model;
using Event_Manager_API.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.BL.Class
{
    public class JobBL :IJobBL
    {
        IJobDL _jobDL;
        IUserAccountDL _userAccountDL;

        ServiceResult _serviceResult;
        public JobBL(IJobDL jobDL, IUserAccountDL userAccountDL)
        {
            _jobDL = jobDL;
            _userAccountDL = userAccountDL;
            _serviceResult = new ServiceResult();
            _serviceResult.Code = ServiceCode.Success;
        }


        public ServiceResult DeleteJob(string jobId)
        {
            var job = _jobDL.GetJobById(jobId);
            if(job == null)
            {
                _serviceResult.Code = ServiceCode.BadRequest;
                _serviceResult.Messenger.Add(Resource.Msg_UnknowJob);
                return _serviceResult;
            }

            var result = _jobDL.DeleteJob(jobId);
            if(result <= 0)
            {
                _serviceResult.Code = ServiceCode.BadRequest;
                _serviceResult.Messenger.Add(Resource.Msg_UnknowJob);
                return _serviceResult;
            }
            _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
            return _serviceResult;
        }

        public ServiceResult GetJobs(string userId)
        {
            var checkExist = _userAccountDL.CheckDuplicate<UserAccount>("userId", userId);

            if (!checkExist)
            {
                _serviceResult.Code = ServiceCode.BadRequest;
                _serviceResult.Messenger = new List<string>() { Resource.Msg_UnknowAccount };
                return _serviceResult;
            }

            _serviceResult.Data = _jobDL.GetJobs(userId);
            _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
            return _serviceResult;
        }

        public ServiceResult GetJob(string jobId)
        {
            var job = _jobDL.GetJobById(jobId);
            if (job == null)
            {
                _serviceResult.Code = ServiceCode.BadRequest;
                _serviceResult.Messenger.Add(Resource.Msg_UnknowJob);
                return _serviceResult;
            }
            _serviceResult.Data = job;
            _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
            return _serviceResult;
        }

        public ServiceResult InsertJob(Job job)
        {
            job.JobId = new Guid();
            ValidateObject(job);

            if(_serviceResult.Code == ServiceCode.BadRequest)
            {
                return _serviceResult;
            }
            var check = _jobDL.InsertJob(job);
            if(check > 0)
            {
                _serviceResult.Messenger = new List<string>() { Resource.Msg_Success };
                return _serviceResult;
            }
            _serviceResult.Messenger.Add(Resource.Msg_AddJobFail);
            _serviceResult.Code = ServiceCode.BadRequest;
            return _serviceResult;
        }

        public ServiceResult UpdateJob(Job job)
        {
            ValidateObjectUpdate(job);
            if (_serviceResult.Code == ServiceCode.BadRequest) return _serviceResult;

            _serviceResult.Messenger.Add(Resource.Msg_Success);
            _serviceResult.Data = _jobDL.UpdateJob(job);
            return _serviceResult;
        }

        //private method
        private void ValidateObjectLogin(Job job)
        {
            var properties = typeof(Job).GetProperties();
            //Kiểm tra các điều kiện đúng đắn về mặt dữ liệu
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(job);
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
        private void ValidateObject(Job job)
        {
            var properties = typeof(Job).GetProperties();
            //Kiểm tra các điều kiện đúng đắn về mặt dữ liệu
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(job);
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
        private void ValidateObjectUpdate(Job job)
        {
            {
                var properties = typeof(Job).GetProperties();
                //Kiểm tra các điều kiện đúng đắn về mặt dữ liệu
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(job);
                    var propertyName = property.Name;

                    //trim dữ liệu
                    if (property.PropertyType.Name == "String")
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
                            var checkDuplicate = _jobDL.CheckDuplicateUpdate(propertyName, propertyValue, job.JobIdContains);
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
}
