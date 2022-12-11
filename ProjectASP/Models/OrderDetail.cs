using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    [Table("order_detail")]
    public class OrderDetail
    {
        [Key]
        [DisplayName("Mã đơn hàng")]
        public int OrderID { get; set; }
        [DisplayName("Mã khách hàng")]
        public int AccountID { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int ProductID { get; set; }
        [DisplayName("Số sản phẩm")]
        public int Quantity { get; set; }
        [DisplayName("Kiểu thanh toán")]
        [Required(ErrorMessage ="Kiểu thanh toán là bắt buộc")]
        public int TypeOrder { get; set; }
        [DisplayName("Địa chỉ giao hàng")]
        [Required(ErrorMessage ="Địa chỉ giao phải nhập")]
        public string Address { get; set; }
        //[DisplayName("Tên người nhận")]
        //[Required(ErrorMessage = "Tên người nhận phải nhập")]
        //public string Taker { get; set; }
        //[DisplayName("SĐT người nhận")]
        //[Required(ErrorMessage = "SĐT người nhận phải nhập")]
        //public string TakerPhone { get; set; }
        [DisplayName("Trạng thái đơn hàng")]
        public int Status { get; set; }

        public Account Account { get; set; }
        public Product Product { get; set; }
    }
}
