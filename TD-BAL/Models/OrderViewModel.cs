using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD_BAL.Models.Dto.Req;

namespace TD_BAL.Models
{
    public class OrderViewModel
    {
        public ReqOrderDto? ReqOrderDto { get; set; } 
        public List<ReqItemDto>? ReqItemDtos { get; set; }  
    }

}
