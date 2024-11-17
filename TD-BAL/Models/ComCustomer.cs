using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models
{
    [Table("COM_CUSTOMER")]
    public class ComCustomer
    {
        [Key]
        [Required]
        [Column("COM_CUSTOMER_ID")]
        public int Id { get; set; }
        [Required]
        [Column("CUSTOMER_NAME")]
        public string? Name { get; set; }
    }
}
