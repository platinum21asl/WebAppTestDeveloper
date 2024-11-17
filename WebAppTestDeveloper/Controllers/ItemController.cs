using ClosedXML.Excel;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using TD_BAL.Models;
using TD_BAL.Models.Dto.Req;
using TD_BAL.Models.Dto.Res;
using TD_BAL.Repository.Services;


namespace WebAppTestDeveloper.Controllers
{
    [Controller]
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;

        private readonly IItemsServices _itemsServices;
        private readonly ISalesOrderServices _salesOrderServices;

        public ItemController(ILogger<ItemController> logger, IItemsServices itemsServices, ISalesOrderServices salesOrderServices)
        {
            _logger = logger;
            _itemsServices = itemsServices;
            _salesOrderServices = salesOrderServices;
        }
        public async Task<IActionResult> Index(int page = 1, string? orderDate = null, string? keyword = null )
        {
            try
            {
                const int pageSize = 5; 
                var allItems = await _salesOrderServices.GetAll(orderDate, keyword);

                var totalCount = allItems.Count();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Paging data
                var pagedItems = allItems
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Membuat model untuk dikirim ke view
                var viewModel = new PagedSalesOrderViewModel
                {
                    SalesOrders = pagedItems,
                    CurrentPage = page,
                    TotalPages = totalPages
                };
                ViewBag.OrderDate = orderDate;
                ViewBag.Keyword = keyword;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
               // Get All Data Customer
               var listCustomer = await _itemsServices.GetAllCustomer();
                ViewBag.Customers = listCustomer;
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult SaveOrderAndItems([FromBody] OrderViewModel orderViewModel)
        {
            try
            {
 
                long orderId = _salesOrderServices.CreateOrder(orderViewModel.ReqOrderDto);

                if (orderId == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save order");
                }

                foreach (var itemDto in orderViewModel.ReqItemDtos)
                {
                    itemDto.OrderId = orderId;

                
                    var itemSaved = _itemsServices.CreateItem(itemDto);
                    if (!itemSaved)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save item");
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        public async Task<IActionResult> Update(long id)
        {
            try
            {
                var listCustomer = await _itemsServices.GetAllCustomer();
                var oldDataSalesOrder = await _salesOrderServices.ReadOrderById(id);
                var listDataItem = await _itemsServices.GetAll(id);

                var totalItem = listDataItem.Sum(item => item.Qty);
                var totalAmount = listDataItem.Sum(item => item.Qty * item.Price);

                ViewBag.SelectCustomer = oldDataSalesOrder.NameCustomer;
                ViewBag.SelectCustomerId = oldDataSalesOrder.IdCustomer;
                ViewBag.SalesOrder = oldDataSalesOrder;
                ViewBag.Customers = listCustomer;
                ViewBag.ListItem = listDataItem;

                ViewBag.TotalItem = totalItem;
                ViewBag.TotalAmount = totalAmount;

                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SaveUpdateOrderAndItems([FromBody] OrderViewModel orderViewModel, long id)
        {
            try
            {
                if (orderViewModel == null)
                {
                    return BadRequest("Invalid order data.");
                }

                // Panggil service
                _salesOrderServices.SaveUpdateOrderAndItems(orderViewModel, id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        public IActionResult Delete(long id) 
        {
            try
            {
                var isSuccessDelete = _salesOrderServices.DeleteOrder(id);
                if (isSuccessDelete) {
                    _itemsServices.DeleteItem(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();

                }
          
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<IActionResult> ExportToCsv(int page = 1, string? orderDate = null, string? keyword = null)
        {
            try
            {
                const int pageSize = 5;

                var allItems = await _salesOrderServices.GetAll(orderDate, keyword);

                if (!string.IsNullOrEmpty(orderDate))
                {
                    if (DateTime.TryParse(orderDate, out var parsedDate))
                    {
                        allItems = allItems.Where(x => Convert.ToDateTime(x.OrderDate) == parsedDate.Date).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    allItems = allItems.Where(x => x.SalesOrder.Contains(keyword) || x.NameCustomer.Contains(keyword)).ToList();
                }

                var csvContent = new StringWriter();
                var csv = new CsvWriter(csvContent, CultureInfo.InvariantCulture);

                csv.WriteField("Order No");
                csv.WriteField("Order Date");
                csv.WriteField("Customer Name");
                csv.NextRecord();

                foreach (var item in allItems)
                {
                    csv.WriteField(item.SalesOrder);
                    csv.WriteField(item.OrderDate);
                    csv.WriteField(item.NameCustomer);
                    csv.NextRecord();
                }

                var fileName = "SalesOrders.csv";
                return File(System.Text.Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
