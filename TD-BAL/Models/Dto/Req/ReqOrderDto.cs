using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models.Dto.Req
{
    public class ReqOrderDto
    {
        public int? Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string? Address { get; set; }
    }
}
