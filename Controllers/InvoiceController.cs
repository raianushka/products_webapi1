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
    public class InvoiceController : ApiController
    {
        public ResultClassName Get(bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var invoices = DataProvider.Invoices.ToList();

                if (invoices.Any())
                {
                    result.Data = invoices;
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Invoice>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Invoice Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Invoice details not found";
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
                var invoices = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();

                if (invoices != null)
                {
                    result.Data = invoices;
                    if (isZip)
                    {
                        result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Invoice>())));
                        result.Result.IsZip = true;
                    }
                    result.Result.Message = "Invoice Details";
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Invoice details not found";
                }


            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = "Error getting item: {ex.Message}";
            }

            return result;
        }

        public ResultClassName Post(int invoice_no, List<ItemDetails> itemDetailsList, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var invoice = new Invoice();
                invoice.invoice_no = invoice_no;
                if (DataProvider.Invoices.Any(x => x.invoice_no == invoice_no))
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Invoice with the same number already exists.";
                    return result;
                }

                foreach (var itemDetails in itemDetailsList)
                {
                    var item = DataProvider.Items.FirstOrDefault(x => x.item_id == itemDetails.itemId);
                    if (item == null)
                    {
                        result.Result.Flag = false;
                        result.Result.Message = "Item with the given ID does not exist.";
                        return result;
                    }


                    var category = DataProvider.Categories.FirstOrDefault(x => x.category_id == item.category_id);
                    if (invoice.ItemsList.Any(i => i.invoice_itemId == item.item_id))
                    {
                        result.Result.Flag = false;
                        result.Result.Message = "Item with the same ID already exists in the invoice.";
                        return result;
                    }

                    var newItem = new Itemslist
                    {
                        invoice_itemId = item.item_id,
                        itemCode = item.item_code,
                        itemName = item.item_name,
                        itemQty = itemDetails.itemQty,
                        itemUnitPrice = item.itemPrice,
                        itemDiscount = item.itemDiscountInper,
                        itemAmount = itemDetails.itemQty * item.itemPrice,
                        itemAmountPaid = (itemDetails.itemQty * item.itemPrice) - (itemDetails.itemQty * item.itemPrice * item.itemDiscountInper) / 100,
                        category_id = category.category_id,
                        category_name = category.category_name
                    };

                    invoice.ItemsList.Add(newItem);
                }

                DataProvider.Invoices.Add(invoice);
                result.Result.Flag = true;
                result.Result.Message = "Invoice added successfully.";
                if (isZip)
                {
                    result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Invoice>())));
                    result.Result.IsZip = true;
                }
                else
                {
                    result.Data = invoice.invoiceId;
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error adding invoice: {ex.Message}";
            }
            return result;

        }

        public ResultClassName Put(int invoiceId, int itemId, int itemQty, bool isZip)
        {
            var result = new ResultClassName();
            try
            {
                var invoice = DataProvider.Invoices.FirstOrDefault(x => x.invoiceId == invoiceId);
                if (invoice == null)
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Invoice not found.";
                    return result;
                }

                var existingItem = invoice.ItemsList.FirstOrDefault(i => i.invoice_itemId == itemId);
                if (existingItem != null)
                {
                    existingItem.itemQty = itemQty;
                    existingItem.itemAmount = itemQty * existingItem.itemUnitPrice;
                    existingItem.itemAmountPaid = existingItem.itemAmount - (existingItem.itemAmount * existingItem.itemDiscount) / 100;
                }
                else
                {
                    // Create a new item
                    var item = DataProvider.Items.FirstOrDefault(x => x.item_id == itemId);
                    if (item == null)
                    {
                        result.Result.Flag = false;
                        result.Result.Message = "Item not found.";
                        return result;
                    }

                    var category = DataProvider.Categories.Where(x => x.category_id == item.category_id).FirstOrDefault();

                    var newItem = new Itemslist
                    {
                        invoice_itemId = item.item_id,
                        itemCode = item.item_code,
                        itemName = item.item_name,
                        itemQty = itemQty,
                        itemUnitPrice = item.itemPrice,
                        itemDiscount = item.itemDiscountInper,
                        itemAmount = itemQty * item.itemPrice,
                        itemAmountPaid = (itemQty * item.itemPrice) - (itemQty * item.itemPrice * item.itemDiscountInper) / 100,
                        category_id = category.category_id,
                        category_name = category.category_name
                    };
                    invoice.ItemsList.Add(newItem);
                }
                result.Result.Flag = true;
                result.Result.Message = "Item updated/added successfully.";
                if (isZip)
                {
                    result.Data = Convert.ToBase64String(CompressionUtility.Zip(JsonConvert.SerializeObject(result?.Data ?? new List<Invoice>())));
                    result.Result.IsZip = true;
                }
                else
                {
                    result.Data = invoice.invoiceId;
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error updating/adding item: {ex.Message}";
            }
            return result;
        }

        public ResultClassName Delete(int id)
        {
            var result = new ResultClassName();
            try
            {
                var invoice_val = DataProvider.Invoices.Where(x => x.invoiceId == id).FirstOrDefault();
                if (invoice_val != null)
                {
                    DataProvider.Invoices.Remove(invoice_val);
                    result.Result.Flag = true;
                    result.Result.Message = "Invoice deleted successfully.";
                    result.Data = id;
                }
                else
                {
                    result.Result.Flag = false;
                    result.Result.Message = "Invoice not found.";
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error deleting invoice: {ex.Message}";
            }

            return result;

        }

        [HttpDelete]
        [Route("api/invoice/{invoiceId}/item/{itemId}")]
        public ResultClassName Delete(int invoiceId, int itemId)
        {
            var result = new ResultClassName();
            try
            {
                var invoice = DataProvider.Invoices.FirstOrDefault(x => x.invoiceId == invoiceId);
                if (invoice != null)
                {
                    var itemToRemove = invoice.ItemsList.FirstOrDefault(i => i.invoice_itemId == itemId);
                    if (itemToRemove != null)
                    {
                        invoice.ItemsList.Remove(itemToRemove);
                        result.Result.Flag = true;
                        result.Result.Message = "Invoice deleted successfully.";
                        result.Data = invoiceId;
                    }
                    else
                    {
                        result.Result.Flag = false;
                        result.Result.Message = "Invoice not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result.Flag = false;
                result.Result.Message = $"Error deleting invoice: {ex.Message}";
            }

            return result;

        }
    }
}
