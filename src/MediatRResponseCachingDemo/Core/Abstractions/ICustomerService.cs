using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Abstractions
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomerList();
        Customer? GetCustomer(int id);
    }
}
