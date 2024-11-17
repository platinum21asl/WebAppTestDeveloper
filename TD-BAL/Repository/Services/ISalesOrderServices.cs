using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD_BAL.Models;
using TD_BAL.Models.Dto.Req;
using TD_BAL.Models.Dto.Res;

namespace TD_BAL.Repository.Services
{
    public interface ISalesOrderServices
    {
        Task<IEnumerable<ResGetAllSalesOrderDto>> GetAll(string? OrderDate = null, string? Keyword = null);
        long CreateOrder(ReqOrderDto reqOrderDto);
        Task<bool> UpdateOrder(ReqOrderDto reqOrderDto, long id);
        bool DeleteOrder(long id);
        Task<ResGetAllSalesOrderDto> ReadOrderById(long id);
        void SaveUpdateOrderAndItems(OrderViewModel orderViewModel, long id);
    }
}
