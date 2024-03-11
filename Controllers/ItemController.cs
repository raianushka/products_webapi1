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
    public class ItemController : ApiController
    {
        // GET api/values
        public ResultClassName Get(bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var items = DataProvider.Items.ToList();

                if (items.Any())
                {
                    result.Data = items;
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Item>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Item Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Item details not found";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = "Error getting item: {ex.Message}";
            }

            return result;
        }

        // GET api/values/5
        public ResultClassName Get(int id, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var items = DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();

                if (items != null)
                {
                    result.Data = items;
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Item>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Item Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Item details not found";
                }


            }
            catch (Exception ex)
            {
                    result.Result.Flag = false;
                    result.Result.Message = "Error getting item: {ex.Message}";
            }

            return result;

        }

        // POST api/values
        public ResultClassName Post([FromBody] Item value, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                if (!CategoryExists(value.category_id))
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category does not exist.";
                    return result;
                }
                if (DataProvider.Items.Any(x => x.item_id == value.item_id && x.category_id == value.category_id))
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Item with the same ID and category already exists.";
                    return result;
                }
                DataProvider.Items.Add(value);
                result.Result.Flag = true;
                result.Result.Message = "Item added successfully.";
                if (isZip)
                {
                    result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Item>())));
                    result.Result.IsZip = true;
                }
                else
                {
                    result.Data = value.item_id; ;
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error adding item: {ex.Message}";
            }

            return result;
        }

        // PUT api/values/5
        public ResultClassName Put(int id, [FromBody] Item value, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                if (!CategoryExists(value.category_id))
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Category does not exist.";
                    return result;
                }
                var item = DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();
                if (item != null)
                {
                    // Update item details
                    item.item_name = value.item_name;
                    item.category_id = value.category_id;
                    item.item_code = value.item_code;
                    item.itemPrice = value.itemPrice;
                    item.itemDiscountInper = value.itemDiscountInper;

                    // Update items in invoices
                    foreach (var invoice in DataProvider.Invoices)
                    {
                        var invoiceItem = invoice.ItemsList.FirstOrDefault(i => i.invoice_itemId == id);
                        if (invoiceItem != null)
                        {
                            invoiceItem.itemName = value.item_name;
                            invoiceItem.itemCode = value.item_code;
                            invoiceItem.itemUnitPrice = value.itemPrice;
                            invoiceItem.itemDiscount = value.itemDiscountInper;
                            invoiceItem.itemAmount = value.itemPrice * invoiceItem.itemQty;
                            invoiceItem.itemAmountPaid = (value.itemPrice * invoiceItem.itemQty) - ((value.itemPrice * invoiceItem.itemQty) * value.itemDiscountInper) / 100;
                        }
                    }
                    result.Result.Flag = true;
                    result.Result.Message = "Item updated successfully.";
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Item>())));
                        result.Result.IsZip = true;
                    }
                    else
                    {
                        result.Data = item.item_id;
                    }
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Item not found.";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error updating item: {ex.Message}";
            }

            return result;

        }

        // DELETE api/values/5
        public ResultClassName Delete(int id)
        {
            var result = new ResultClassName();
            try
            {
                var item = DataProvider.Items.Where(x => x.item_id == id).FirstOrDefault();
                if (item != null)
                {
                    DataProvider.Items.Remove(item);
                    result.Result.Flag = true;
                    result.Result.Message = "Item deleted successfully.";
                    result.Data = id;
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Item not found.";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error deleting item: {ex.Message}";
            }

            return result;

        }

        private bool CategoryExists(int categoryId)
        {
            return DataProvider.Categories.Any(c => c.category_id == categoryId);
        }
    }
}
