using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD_BAL.Helpers;
using TD_BAL.Models;
using TD_BAL.Models.Context;
using TD_BAL.Models.Dto.Req;
using TD_BAL.Models.Dto.Res;

namespace TD_BAL.Repository.Services.Impl
{
    public class SalesOrderServicesImpl : ISalesOrderServices
    {
        private readonly ApplicationDbContext _dbContext;

        public SalesOrderServicesImpl(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public long CreateOrder(ReqOrderDto reqOrderDto)
        {
            try
            {
             // Generate random OrderId

                var newObj = new SoOrder
                {
                    OrderNo = reqOrderDto.OrderNumber,
                    OrderDate = Convert.ToDateTime(reqOrderDto.OrderDate),
                    ComCustomerId = Convert.ToInt32(reqOrderDto.CustomerId),
                    Address = reqOrderDto.Address
                };

                _dbContext.Add(newObj);
                _dbContext.SaveChanges();

                return newObj.SOOrderId;  // Return the SOOrderId that was generated
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool DeleteOrder(long id)
        {
            try
            {
                var oldObj = _dbContext.SoOrders
                      .Where(x => x.SOOrderId == id)
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

        public async Task<IEnumerable<ResGetAllSalesOrderDto>> GetAll(string? OrderDate = null, string? Keyword = null)
        {
            try
            {
                var query = _dbContext.SoOrders
                    .Join(
                        _dbContext.ComCustomers,
                        order => order.ComCustomerId,
                        customer => customer.Id,
                        (order, customer) => new
                        {
                            Order = order,
                            CustomerName = customer.Name
                        }
                    );

                if (!string.IsNullOrEmpty(OrderDate) && DateTime.TryParse(OrderDate, out var parsedDate))
                {
                    query = query.Where(x => x.Order.OrderDate.Date == parsedDate.Date);
                }

                if (!string.IsNullOrEmpty(Keyword))
                {
                    query = query.Where(x =>
                        x.Order.OrderNo.Contains(Keyword) ||
                        x.CustomerName.Contains(Keyword)
                    );
                }

                var items = await query
                    .OrderByDescending(x => x.Order.OrderDate) 
                    .Select(x => new ResGetAllSalesOrderDto
                    {
                        Id = x.Order.SOOrderId,
                        SalesOrder = x.Order.OrderNo,
                        OrderDate = Convert.ToString(x.Order.OrderDate), 
                        NameCustomer = x.CustomerName
                    })
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResGetAllSalesOrderDto> ReadOrderById(long id)
        {
            try
            {
                var item = await _dbContext.SoOrders
                      .Where(order => order.SOOrderId == id)
                      .Join(
                          _dbContext.ComCustomers,
                          order => order.ComCustomerId,
                          customer => customer.Id,
                          (order, customer) => new ResGetAllSalesOrderDto
                          {
                              Id = order.SOOrderId,
                              SalesOrder = order.OrderNo,
                              Address = order.Address,
                              OrderDate = Convert.ToString(order.OrderDate),
                              NameCustomer = customer.Name,
                              IdCustomer = customer.Id
                          }
                      )
                      .OrderByDescending(x => x.OrderDate)
                      .FirstOrDefaultAsync();

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveUpdateOrderAndItems(OrderViewModel orderViewModel, long id)
        {
            var order = _dbContext.SoOrders.FirstOrDefault(o => o.SOOrderId == id);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            order.OrderNo = orderViewModel.ReqOrderDto.OrderNumber;
            order.OrderDate = DateTime.Parse(orderViewModel.ReqOrderDto.OrderDate);
            order.ComCustomerId = orderViewModel.ReqOrderDto.CustomerId;
            _dbContext.SoOrders.Update(order);

            // Update atau insert items
            if (orderViewModel.ReqItemDtos != null && orderViewModel.ReqItemDtos.Any())
            {
                foreach (var itemDto in orderViewModel.ReqItemDtos)
                {
                    var existingItem = _dbContext.SoItems.FirstOrDefault(x => x.SOOrderId == id && x.ItemName == itemDto.Name);

                    if (existingItem != null)
                    {
                        existingItem.Quantity = itemDto.Qty;
                        existingItem.Price = itemDto.Price ?? 0;
                        _dbContext.SoItems.Update(existingItem);
                    }
                    else
                    {
                        var newItem = new SoItem
                        {
                            ItemName = itemDto.Name,
                            Quantity = itemDto.Qty,
                            Price = itemDto.Price ?? 0,
                            SOOrderId = id
                        };
                        _dbContext.SoItems.Add(newItem);
                    }
                }
            }

            // Simpan perubahan
            _dbContext.SaveChanges();
        }

        public async Task<bool> UpdateOrder(ReqOrderDto reqOrderDto, long id)
        {
            try
            {
                var oldObj = await _dbContext.SoOrders
                              .FirstOrDefaultAsync(x => x.SOOrderId == id);
                if (oldObj != null)
                {
                    oldObj.OrderNo = reqOrderDto.OrderNumber;
                    oldObj.OrderDate = Convert.ToDateTime(reqOrderDto.OrderDate);
                    oldObj.ComCustomerId = reqOrderDto.CustomerId;
                    oldObj.Address = reqOrderDto.Address;

                    _dbContext.Update(oldObj);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order: {ex.Message}");
            }
        }

    }
}
