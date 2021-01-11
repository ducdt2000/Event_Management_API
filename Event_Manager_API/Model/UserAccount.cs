using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Model
{
    public class UserAccount
    {
        [Required("Mã tài khoản thành viên")]
        [MaxLength("Mã tài khoản thành viên", 20)]
        [MinLength("Mã tài khoản thành viên", 6)]
        [Duplicate("Mã tài khoản thành viên")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [MaxLength("Mật khẩu thành viên", 20)]
        [MinLength("Mật khẩu thành viên", 6)]
        public string UserPassword { get; set; }
    }
}
