using ECommerce.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Search.Interfaces
{
    public interface ICustomerService
    {
        Task<(bool IsSucess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSucess, Customer Customer, string ErrorMessage)> GetCustomerAsync(int customerId);
    }
}
