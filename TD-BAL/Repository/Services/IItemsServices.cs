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
    public interface IItemsServices
    {
        Task<IEnumerable<ResGetAllItemsDto>> GetAll(long? Id = null);
        bool CreateItem(ReqItemDto reqItemDto);
        Task<bool> UpdateItem(ReqItemDto reqItemDto, int id);
        bool DeleteItem(long id);
        Task<ResGetAllItemsDto> ReadItemById(int id);
        bool CreateCustomer(ReqCustomerDto reqCustomerDto);
        bool DeleteCustomer(int id);
        Task<IEnumerable<ResGetAllCustomerDto>> GetAllCustomer();
    }
}
