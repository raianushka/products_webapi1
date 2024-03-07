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
    public class CategoryController : ApiController
    {
            // GET api/values
            public List<Category> Get()
            {
                return DataProvider.Categories.ToList();
            }

            // GET api/values/5
            public Category Get(int id)
            {
                return DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();
            }

            // POST api/values
            public bool Post([FromBody] Category value)
            {
                DataProvider.Categories.Add(value);
                return true;
            }

            // PUT api/values/5
            public bool Put(int id, [FromBody] Category value)
            {
                var cat = DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();
                if (cat != null)
                {
                    cat.category_name = value.category_name;
                    return true;
                }
                return false;
            }

            // DELETE api/values/5
            public bool Delete(int id)
            {
                var cat = DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();
                if (cat != null)
                {
                DataProvider.Items.RemoveAll(i => i.category_id == id);

                DataProvider.Categories.Remove(cat);
                    return true;                                      
                }
                return false;

            }
        
    
}
}
