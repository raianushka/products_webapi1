using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV.Models;
using INV.DataProvider;

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

        /* public bool Post([FromBody] Invoice value)
         {
             DataProvider.Invoices.Add(value);
             return true;
         }*/

        /*public bool Post(int invoiceId, int invoice_no, int invoice_itemId, int itemQty)
        {
            var item = DataProvider.Items.FirstOrDefault(x => x.item_id == invoice_itemId);
            if (item == null)
            {
                return false; // Item with the given invoice_itemId does not exist
            }

            var category = DataProvider.Categories.FirstOrDefault(x => x.category_id == item.category_id);
            if (category == null)
            {
                return false;
            }

            var newItem = new Itemslist
            {
                invoice_itemId = invoice_itemId,
                itemCode = item.item_code,
                itemName = item.item_name,
                itemQty = itemQty,
                itemUnitPrice = item.itemPrice,
                itemDiscount = item.itemDiscountInper,
                itemAmount = itemQty * item.itemPrice,
                itemAmountPaid = (itemQty * item.itemPrice) - (itemQty * item.itemPrice * item.itemDiscountInper) / 100,
                category_id = category.category_id,
                category_name = category.category_name
            };

            var invoice = new Invoice();
            invoice.invoice_no = invoice_no;

            if (newItem.invoice_itemId == item.item_id)
            {
                invoice.ItemsList.Add(newItem);
                DataProvider.Invoices.Add(invoice);
                return true;
            }

            return false; // invoice_itemId does not match item_id
        }*/

        public bool Post(int invoice_no, List<ItemDetails> itemDetailsList)
        {
            var invoice = new Invoice();
            invoice.invoice_no = invoice_no;

            foreach (var itemDetails in itemDetailsList)
            {
                var item = DataProvider.Items.FirstOrDefault(x => x.item_id == itemDetails.itemId);
                if (item == null)
                {
                    return false; // Item with the given itemId does not exist
                }

                var category = DataProvider.Categories.FirstOrDefault(x => x.category_id == item.category_id);
                if (category == null)
                {
                    return false; // Category for the item does not exist
                }

                var newItem = new Itemslist
                {
                    invoice_itemId = item.item_id,
                    itemCode = item.item_code,
                    itemName = item.item_name,
                    itemQty = itemDetails.itemQty,
                    itemUnitPrice = item.itemPrice,
                    itemDiscount = item.itemDiscountInper,
                    itemAmount = itemDetails.itemQty * item.itemPrice,
                    itemAmountPaid = (itemDetails.itemQty * item.itemPrice) - (itemDetails.itemQty * item.itemPrice * item.itemDiscountInper) / 100,
                    category_id = category.category_id,
                    category_name = category.category_name
                };

                invoice.ItemsList.Add(newItem);
            }

            DataProvider.Invoices.Add(invoice);
            return true;
        }

        public class ItemDetails
        {
            public int itemId { get; set; }
            public int itemQty { get; set; }
        }






        public bool Put(int id, [FromBody] Invoice value)
        {
            var invoiceupdate = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();

            if (invoiceupdate != null)
            {
                invoiceupdate.invoice_no = value.invoice_no;
                return true;
            }

            return false;
        }
      
        public bool Delete(int id)
        {
            var invoice_val = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();
            if (invoice_val != null)
            {
                DataProvider.Invoices.Remove(invoice_val);
                return true;
            }
            return false;
        }

 



    }
}
