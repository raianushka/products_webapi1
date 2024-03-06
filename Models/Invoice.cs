using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Models
{
    public class Invoice
    {
        private static int _nextInvoiceId = 1;
        public int invoiceId { get;}

        public int invoice_itemId { get; set; }

        public int invoice_no { get; set; }

        public List<Itemslist> Items { get; set; }

        
        public Invoice()
        {
            invoiceId = _nextInvoiceId++;
            Items = new List<Itemslist>();
        }



    }
    public class Itemslist
        {
        public int itemCode { get; set; }
        public int itemQty { get; set; }
        public int itemUnitPrice { get; set; }
        public int itemDiscount { get; set; }
        public int itemAmount { get; set; }
        public int itemAmountPaid { get; set; }
    }
       
    
}