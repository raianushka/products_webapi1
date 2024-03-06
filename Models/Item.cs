using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Models
{
    public class Item
    {
        internal int Discount;
        internal int Amount;
        internal int AmountPaid;

        public int item_id { get; set; }

        public int category_id { get; set; }

        public int item_code { get; set; }

        public string item_name { get; set; }

        public int itemPrice { get; set; }

        public int itemDiscountInper { get; set; }

    }
}