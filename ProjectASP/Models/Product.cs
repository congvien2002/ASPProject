using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [DisplayName("Mã Sản Phẩm")]
        public int ProductID { get; set; }
        [DisplayName("Tên Sản Phẩm")]
        [Required(ErrorMessage ="Tên sản phẩm không để trống")]
        [MaxLength(30,ErrorMessage ="Tên sản phẩm không quá 30 kí tự")]
        [MinLength(6,ErrorMessage ="Tên sản phẩm tối thiểu 6 kí tự ")]
        public string ProductName { get; set; }
        [DisplayName("Ảnh Minh Họa")]
        public string ProductImage { get; set; }
        [DisplayName("Giá Niêm Yết")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage ="Giá không được để trống ")]
        public float Price { get; set; }
        [DisplayName("Giá Khuyến Mại")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public float SalePrice { get; set; }
        [DisplayName("Ngày Sản Xuất")]
        [Required(ErrorMessage ="Ngày sản xuất bắt buộc nhập")]
        public DateTime DayMaking { get; set; }
        [DisplayName("Nơi Sản Xuất")]
        [Required(ErrorMessage ="Nơi sản xuất bắt buộc nhập")]
        public string MadeIn { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
        [DisplayName("Thể loại")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
