using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Products.Models;

namespace Products.Controllers
{
    public class InvoiceController : ApiController
    {
        // GET api/values
        public List<Invoice> Get()
        {
            return DataProvider.Invoices.ToList();
        }

        // GET api/values/5
        public Invoice Get(int id)
        {
            return DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();
        }

        // POST api/values
        public bool Post([FromBody] Invoice value)
        {
            DataProvider.Invoices.Add(value);
            return true;
        }

        // PUT api/values/5
        public bool Put(int id, [FromBody] Invoice value)
        {
            var cat = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();
            if (cat != null)
            {
                //cat.itemAmount= value.itemAmount;
                return true;
            }
            return false;
        }

        // DELETE api/values/5
        public bool Delete(int id)
        {
            var cat = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();
            if (cat != null)
            {
                DataProvider.Invoices.Remove(cat);
                return true;
            }
            return false;
        }


    }
}
