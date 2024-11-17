using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models
{
    [Table("SO_ORDER")]
    public class SoOrder
    {
        [Key]
        [Required]
        [Column("SO_ORDER_ID")]
        public long SOOrderId { get; set; }
        [Required]
        [Column("ORDER_NO")]
        public string? OrderNo { get; set; }
        [Required]
        [Column("ORDER_DATE")]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column("COM_CUSTOMER_ID")]
        public int ComCustomerId { get; set; }
        [Required]
        [Column("ADDRESS")]
        public string? Address { get; set; }     
        public virtual ICollection<SoItem>? SOItems { get; set; }
    }
}
