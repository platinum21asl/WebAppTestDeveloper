using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models.Dto.Res
{
    public class ResGetAllSalesOrderDto
    {
        public long Id { get; set; }
        public string? SalesOrder { get; set; }
        public string? OrderDate { get; set; }
        public string? NameCustomer { get; set; }
        public string? Address { get; set; }
        public long? IdCustomer { get; set; }
    }
}
