using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models.Dto.Req
{
    public class ReqItemDto
    {
        public string? Name { get; set; }
        public int Qty { get; set; }
        public double? Price { get; set; }
        public long OrderId { get; set; }
    }
}
