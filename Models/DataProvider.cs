using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Models
{
    public static class DataProvider
    {
        public static List<Category> Categories{get; set;} = new List<Category>();
        public static List<Item> Items { get; set; } = new List<Item>();
        public static List<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}