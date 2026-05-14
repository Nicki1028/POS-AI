using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public class OrderRequestModel
    {
        //前端資料的勾選都要裝進這model來傳下去
        public MenuModel.Discount DiscountType { get; set; }
        public List<Item> Orders { get; set; }
        public Item OrderItem { get; set; }
        public bool EnableAISuggestion { get; set; }
        public OrderRequestModel(MenuModel.Discount DiscountType, Item OrderItem, bool EnableAISuggestion)
        {
            this.EnableAISuggestion = EnableAISuggestion;
            this.DiscountType = DiscountType;
            this.OrderItem = OrderItem;
        }
        public OrderRequestModel(bool EnableAISuggestion)
        {
            this.EnableAISuggestion = EnableAISuggestion;
        }
        public OrderRequestModel(MenuModel.Discount DiscountType)
        {
            EnableAISuggestion = false;
            this.DiscountType = DiscountType;
        }
    }
}
