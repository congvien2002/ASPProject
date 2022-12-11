using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    [Table("account")]
    public class Account
    {
        [Key]
        [DisplayName("Mã tài khoản")]
        public int AccountID { get; set; }
        [DisplayName("Tên người dùng")]
        [Required(ErrorMessage ="Tên người dùng không để trống")]
        [MinLength(6,ErrorMessage ="Tên tối thiểu 6 kí tự")]
        [MaxLength(20,ErrorMessage ="Tên tối đa 20 kí tự")]
        public string AccountName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage ="Email không để trống ")]
        public string Email { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage ="Số điện thoại bắt buộc nhập")]
        public string Phone { get; set; }
        [DisplayName("Mật Khẩu")]
        [MinLength(8,ErrorMessage ="Mật khẩu tối thiểu 8 kí tự ")]
        public string Password { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime Birthday { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage ="Địa chỉ bắt buộc nhập")]
        public string Address { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime DayCreated { get; set; }
        [DisplayName("Quyền")]
        public bool Role { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
