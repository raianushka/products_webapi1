using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV.Models
{
    public class Invoice
    {
        private static int _nextInvoiceId = 1;
        public int invoiceId { get; }

        public int invoice_no { get; set; }

        public List<Itemslist> ItemsList { get; set; }

        public Invoice()
        {
           invoiceId = _nextInvoiceId++;
           ItemsList = new List<Itemslist>();
        }
    }
    public class Itemslist
    {
        public int invoice_itemId { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public int itemQty { get; set; }
        public decimal itemUnitPrice { get; set; }
        public decimal itemDiscount { get; set; }
        public decimal itemAmount { get; set; }
        public decimal itemAmountPaid { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }

    }
    public class ItemDetails
    {
        public int itemId { get; set; }
        public int itemQty { get; set; }
    }



}