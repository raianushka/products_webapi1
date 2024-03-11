using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV.Models
{
    public class Item
    {

        public int item_id { get; set; }

        public int category_id { get; set; }

        public string item_code { get; set; }

        public string item_name { get; set; }

        public decimal itemPrice { get; set; }

        public decimal itemDiscountInper { get; set; }



    }
}
