using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.ViewModels
{
    public class CustomersViewModel
    {
        public Customer NewCustomer { get; set; }
        public IEnumerable<Customer> Customers { get; set; }


    }
}
