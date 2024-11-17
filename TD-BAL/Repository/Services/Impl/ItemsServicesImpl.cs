using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD_BAL.Models.Context;
using TD_BAL.Models.Dto.Res;
using TD_BAL.Helpers;
using TD_BAL.Models.Dto.Req;
using TD_BAL.Models;
using System.Diagnostics;

namespace TD_BAL.Repository.Services.Impl
{
  
    public class ItemsServicesImpl : IItemsServices
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemsServicesImpl(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public bool CreateCustomer(ReqCustomerDto reqCustomerDto)
        {
            try
            {

                var newObj = new ComCustomer
                {
                    Name = reqCustomerDto.Name,
                };

                _dbContext.Add(newObj);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CreateItem(ReqItemDto reqItemDto)
        {
            try
            {
                //Random random = new Random();
                //int randomInt = random.Next(1, 999);

                var newObj = new SoItem
                {
                    SOOrderId = reqItemDto.OrderId,
                    ItemName = reqItemDto.Name,
                    Quantity = reqItemDto.Qty,
                    Price = Convert.ToDouble(reqItemDto.Price)
                };

                _dbContext.Add(newObj);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCustomer(int id)
        {
            try
            {
                var oldObj = _dbContext.ComCustomers
                      .Where(x => x.Id == id)
                      .FirstOrDefault();

                if (oldObj != null)
                {

                    _dbContext.Remove(oldObj);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteItem(long id)
        {
            try
            {
                var oldObj = _dbContext.SoItems
                      .Where(x => x.SOOrderId == id)
                      .ToList();
                _dbContext.RemoveRange(oldObj);
                _dbContext.SaveChanges();
                return true;


            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ResGetAllItemsDto>> GetAll(long? Id = null)
        {
            try
            {
                var items = await _dbContext.SoItems
                   .Where(x => x.SOOrderId == Id)
                  .Select(item => new ResGetAllItemsDto
                  {
                      Id = item.SOItemId,
                      Name = item.ItemName,
                      Qty = item.Quantity,
                      Price = item.Price,
                
                  })
                  .OrderByDescending(x => x.Name)
                  .ToListAsync();

                return items;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ResGetAllCustomerDto>> GetAllCustomer()
        {
            try
            {
                var items = await _dbContext.ComCustomers
                  .Select(item => new ResGetAllCustomerDto
                  {
                      Id = Convert.ToInt64(item.Id),
                      Name = item.Name,
                  
                  })
                  .OrderByDescending(x => x.Name)
                  .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResGetAllItemsDto> ReadItemById(int id)
        {
            try
            {
                var items = await _dbContext.SoItems
                    .Where(x => x.SOItemId == id)
                    .Select(item => new ResGetAllItemsDto
                    {
                        Id = item.SOItemId,
                        Name = item.ItemName,
                        Qty = item.Quantity,
                        Price = item.Price,
                    })
                  .OrderByDescending(x => x.Name)
                  .FirstOrDefaultAsync();
                
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateItem(ReqItemDto reqItemDto, int id)
        {
            try
            {
                var oldObj = await _dbContext.SoItems
                              .FirstOrDefaultAsync(x => x.SOItemId == id);
                if (oldObj != null)
                {
                    oldObj.ItemName = reqItemDto.Name;
                    oldObj.Quantity = reqItemDto.Qty;
                    oldObj.Price = Convert.ToDouble(reqItemDto.Price);

                    _dbContext.Update(oldObj);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating item: {ex.Message}");
            }
        }

    }
}
