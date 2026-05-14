using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderWPF
{
    public static class Order
    {
        public static List<Item> itemlist = new List<Item>();            
        public static async Task Updateitem(OrderRequestModel model)
        {            
            var findItem = itemlist.FirstOrDefault(x => x.Name == model.OrderItem.Name);
            if (findItem == null && model.OrderItem.Count > 0)
            {
                itemlist.Add(model.OrderItem);
            }
            else
            {
                if (model.OrderItem.Count > 0)
                {
                    findItem.Count = model.OrderItem.Count;
                }
                else
                {
                    itemlist.Remove(findItem);
                }                  
            }
            model.Orders = itemlist;    
            await Discount.CuponUse(model);
        }

        public static async Task ChangeCuponDiscount(OrderRequestModel model)
        {
            model.Orders = itemlist;
            await Discount.CuponUse(model);
        }

        public static void ClearItem()
        {
            itemlist.Clear();
        }

    }
}
