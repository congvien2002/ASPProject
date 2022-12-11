using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectASP.Models
{
    [Table("category")]
    public class Category
    {
        [Key]
        [DisplayName("Mã Thể Loại")]
        public int CategoryID { get; set; }
        [DisplayName("Tên Thể Loại")]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
