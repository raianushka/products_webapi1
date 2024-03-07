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
    public class ItemController : ApiController
    {
        // GET api/values
        public List<Item> Get()
        {
            return DataProvider.Items.ToList();
        }

        // GET api/values/5
        public Item Get(int id)
        {
            return DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();
        }

        // POST api/values
        public bool Post([FromBody] Item value)
        {
            if (!CategoryExists(value.category_id))
            {
                return false;
            }
            DataProvider.Items.Add(value);
            return true;
        }

        // PUT api/values/5
        public bool Put(int id, [FromBody] Item value)
        {
            if (!CategoryExists(value.category_id))
            {
                return false;
            }
            var item = DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();
            if (item != null)
            {
                item.item_name = value.item_name;
                item.category_id = value.category_id;
                item.item_name = value.item_name;
                item.item_code = value.item_code;
                item.itemPrice = value.itemPrice;
                item.itemDiscountInper = value.itemDiscountInper;
                return true;
            }
            return false;
        }

        // DELETE api/values/5
        public bool Delete(int id)
        {
            var item = DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();
            if (item != null)
            {
                DataProvider.Items.Remove(item);
                return true;
            }
            return false;
        }

        private bool CategoryExists(int categoryId)
        {
            return DataProvider.Categories.Any(c => c.category_id == categoryId);
        }
    }
}
