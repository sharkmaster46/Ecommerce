using AutoMapper;
using ECommerce.Orders.DB;
using ECommerce.Orders.Interfaces;
using ECommerce.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Orders.Any())
            {
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 1, OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 35 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 2, OrderId = 1, ProductId = 4, Quantity = 1, UnitPrice = 100 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 15 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 4, OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 35 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 5, OrderId = 2, ProductId = 4, Quantity = 1, UnitPrice = 100 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 6, OrderId = 3, ProductId = 4, Quantity = 1, UnitPrice = 100 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 7, OrderId = 4, ProductId = 3, Quantity = 1, UnitPrice = 35 });
                dbContext.OrderItems.Add(new DB.OrderItem() { Id = 8, OrderId = 4, ProductId = 4, Quantity = 1, UnitPrice = 100 });
                dbContext.SaveChanges();
                dbContext.Orders.Add(new DB.Order() { Id = 1, CustomerId = 1, OrderDate = DateTime.Now, Total = 135});
                dbContext.Orders.Add(new DB.Order() { Id = 2, CustomerId = 2, OrderDate = DateTime.Now, Total = 150 });
                dbContext.Orders.Add(new DB.Order() { Id = 3, CustomerId = 3, OrderDate = DateTime.Now, Total = 100 });
                dbContext.Orders.Add(new DB.Order() { Id = 4, CustomerId = 4, OrderDate = DateTime.Now, Total = 135 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int CustomerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == CustomerId).ToListAsync();
                if(orders != null)
                {
                    foreach(var order in orders)
                    {
                        order.Items = await dbContext.OrderItems.Where(i => i.OrderId == order.Id).ToListAsync();
                    }  
                    var result = mapper.Map<IEnumerable<DB.Order>,IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

    }
}
