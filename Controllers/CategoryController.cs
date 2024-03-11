using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV.Models;
using INV.DataProvider;
using Newtonsoft.Json;

namespace Products.Controllers
{
    public class CategoryController : ApiController
    {
        // GET api/values
         public ResultClassName Get(bool isZip)
         {
            var result = new ResultClassName();
            try
            {
                var category = DataProvider.Categories.ToList();

                if (category.Any())
                {
                    result.Data = category;

                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Category>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Category Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category not found";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = "Error getting category: { ex.Message}";
            }

            return result;

        }

            // GET api/values/5
         public ResultClassName Get(int id, bool isZip)
         {
            var result = new ResultClassName();
            try
            {
                var category = DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();

                if (category != null)
                {
                    result.Data = category;

                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Category>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Category Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category not found";
                }


            }
            catch (Exception ex)
            {
                    result.Result.Flag = false;
                    result.Result.Message = "Error getting category: { ex.Message}";
            }

            return result;
        }

        // POST api/values
        public ResultClassName Post([FromBody] Category value, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                if (DataProvider.Categories.Any(x => x.category_id == value.category_id))
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category with the same ID already exists.";
                }
                else
                {
                    DataProvider.Categories.Add(value);
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Category>())));
                        result.Result.IsZip = true;
                    }
                    else
                    {
                        result.Data = value.category_id;
                    }
                    result.Result.Flag = true;
                    result.Result.Message = "Category added successfully.";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error adding category: {ex.Message}";
            }

            return result;

        }

        // PUT api/values/5
        public ResultClassName Put(int id, [FromBody] Category value, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var cat = DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();

                if (cat != null)
                {
                    cat.category_name = value.category_name;
                    //If category name is changed then update the name in invoice as well 
                    foreach (var invoice in DataProvider.Invoices)
                    {
                        foreach (var item in invoice.ItemsList)
                        {
                            if (item.category_id == id)
                            {
                                item.category_name = value.category_name;
                            }
                        }
                    }
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Category>())));
                        result.Result.IsZip = true;
                    }
                    else
                    {
                        result.Data = value.category_id;
                    }
                    result.Result.Flag = true;
                    result.Result.Message = "Category updated successfully.";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category not updated";
                }
                
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error updating category: {ex.Message}";
            }

            return result;
        }

            

        // DELETE api/values/5
        public ResultClassName Delete(int id)
        {
            var result = new ResultClassName();
            try
            {
                var cat = DataProvider.Categories.Where(x => x.category_id == id).FirstOrDefault();
                if (cat != null)
                {
                DataProvider.Items.RemoveAll(i => i.category_id == id);

                DataProvider.Categories.Remove(cat);
                result.Result.Flag = true;
                result.Result.Message = "Category deleted successfully.";
                result.Data = id;
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category not found.";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error deleting category: {ex.Message}";
            }

            return result;


        }
    }
        
    
}

