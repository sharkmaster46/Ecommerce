using ECommerce.Search.Interfaces;
using ECommerce.Search.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            var productResult = await productService.GetProductsAsync();
            var customerResult = await customerService.GetCustomerAsync(customerId);
            if(orderResult.IsSuccess)
            {
                foreach(Order order in orderResult.Orders)
                {
                    foreach(OrderItem item in order.Items)
                    {
                        item.ProductName = productResult.IsSuccess?
                            productResult.Products.FirstOrDefault(x => x.Id == item.ProductId)?.Name : "Product name is not available";
                    }
                }
                return (true, new
                {
                    Customer = customerResult.IsSucess? customerResult.Customer : new Customer() { Name = "Customer details are not available" },
                    Orders = orderResult.Orders
                });
            }
            return (false, null);
        }
    }
}
