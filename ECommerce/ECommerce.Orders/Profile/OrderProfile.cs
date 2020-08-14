using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Orders.Profile
{
    public class OrderProfile : AutoMapper.Profile
    {
        public OrderProfile()
        {
            CreateMap<DB.Order, Models.Order>();
            CreateMap<DB.OrderItem, Models.OrderItem>();
        }
    }
}
