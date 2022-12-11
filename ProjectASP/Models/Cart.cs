using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    [Table("cart")]
    public class Cart
    {
        [Key]
        [DisplayName("Giỏ hàng")]
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int AccountID { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Account Account { get; set; }
    }
}
