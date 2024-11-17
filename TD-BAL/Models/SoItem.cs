using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models
{
    [Table("SO_ITEM")]
    public class SoItem
    {

        [Key]
        [Required]
        [Column("SO_ITEM_ID")]
        public long SOItemId { get; set; }     
        [Required]
        [Column("SO_ORDER_ID")]
        public long SOOrderId { get; set; }    
        [Required]
        [Column("ITEM_NAME")]
        public string? ItemName { get; set; }
        [Required]
        [Column("QUANTITY")]
        public int Quantity { get; set; }
        [Required]
        [Column("PRICE")]
        public double Price { get; set; }    
        public virtual SoOrder? SOOrder { get; set; }
    }
}
