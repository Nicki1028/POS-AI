using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.DiscountStrategies
{
    public abstract class CuponStrategy
    {
        public List<Item> Items { get; set; }
        public MenuModel.Discount Discount { get; set; }
        public CuponStrategy(List<Item> items, MenuModel.Discount discount)
        {
            this.Items = items;
            this.Discount = discount;
        }

        public abstract void ApplyCupon();
    }
}
