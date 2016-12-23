using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.Routing;
using queryExecutor.Models;

namespace queryExecutor.Controllers
{
    public class CustomersController : ODataController
    {
        private static List<Customer> CustomerList = new List<Customer>
        {
            new Customer {
                Id = 11, Name = "Lowest", Gender = Gender.Female, BirthTime = new DateTime(2001, 1, 1),
                
            },
            new Customer {
                Id = 33, Name = "Highest", Gender = Gender.Male, BirthTime = new DateTime(2002, 2, 2),
                
            },
            new Customer { Id = 22, Name = "Middle", Gender = Gender.Female, BirthTime = new DateTime(2003, 3, 3) },
            new Customer { Id = 3, Name = "NewLow", Gender = Gender.Male, BirthTime = new DateTime(2004, 4, 4) },
        };

        [EnableQuery(AllowedArithmeticOperators = AllowedArithmeticOperators.Add)]
        public IQueryable<Customer> Get()
        {
            return CustomerList.AsQueryable();
        }
        
        public IQueryable<Customer> Get([FromODataUri] long key)
        {
            return CustomerList.AsQueryable();
        }
    }
}