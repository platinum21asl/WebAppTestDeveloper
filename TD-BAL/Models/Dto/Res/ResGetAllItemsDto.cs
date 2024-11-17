using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Models.Dto.Res
{
    public class ResGetAllItemsDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int? Qty { get; set; }
        public double? Price { get; set; }
        public string? Total { get; set; }
    }
}
