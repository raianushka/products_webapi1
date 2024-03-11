using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INV.Models;

namespace INV.DataProvider
{
    public static class DataProvider
    {
        public static List<Category> Categories { get; set; } = new List<Category>{
        new Category { category_id = 1, category_name = "Category 1" },
        new Category { category_id = 2, category_name = "Category 2" },
        new Category { category_id = 3, category_name = "Category 3" }
    };


        public static List<Item> Items { get; set; } = new List<Item> {
        new Item { item_id = 1, category_id = 1, item_code = "item1" , item_name = "ItemName1", itemPrice = 10.0m, itemDiscountInper = 5.0m },
        new Item { item_id = 2, category_id = 2, item_code = "item2" , item_name = "ItemName2", itemPrice = 20.0m, itemDiscountInper = 10.0m },
        new Item { item_id = 3, category_id = 3, item_code = "item3" , item_name = "ItemName3", itemPrice = 30.0m, itemDiscountInper = 15.0m }
        };
        public static List<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}