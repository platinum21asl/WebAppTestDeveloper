using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD_BAL.Models.Dto.Res;

namespace TD_BAL.Models
{
    public class PagedSalesOrderViewModel
    {
        public IEnumerable<ResGetAllSalesOrderDto> SalesOrders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
