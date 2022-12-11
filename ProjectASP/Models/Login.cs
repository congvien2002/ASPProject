using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    public class Login
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Tài khoản không được để trống !")]
        public string Account { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống !")]
        public string Password { get; set; }
    }
}
