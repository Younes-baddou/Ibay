using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class OrderHelper
    {
        public static decimal ShippingFee { get; } = 5;
        public static Dictionary<string, string> PaymentMethods { get; } = new()
        {
        {"Cash","Cash on delivery"},
        {"Paypal","Paypal"},
        {"Credit Card","Credit Card" }

         };
        public static List<string> PaymentStatuses { get; } = new()
         {
        "Pending", "Accepted" , "Canceled"
         };
        public static List<string> OrderStatuses { get; } = new()
          {
        "Created" , "Accepted" , "Canceled" , "Shipped", "Delivered" , "Returned"
          };
        public static Dictionary<int, int> GetProductDictionary(string productionIdentifiers)
        {
            var productDictionary = new Dictionary<int, int>();
            if (productionIdentifiers.Length > 0)
            {
                string[] productIdArray = productionIdentifiers.Split(',');
                foreach (var productId in productIdArray)
                {
                    try
                    {
                        int id = int.Parse(productId);
                        if (productDictionary.ContainsKey(id))
                        {
                            productDictionary[id] += 1;
                        }
                        else
                        {
                            productDictionary.Add(id, 1);
                        }
                    }
                    catch (Exception) { }
                }
            }
            return productDictionary;
        }

    }
    
}
