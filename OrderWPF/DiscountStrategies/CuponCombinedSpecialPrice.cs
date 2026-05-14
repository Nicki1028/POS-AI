using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace OrderWPF.DiscountStrategies
{
    public class CuponCombinedSpecialPrice : CuponStrategy
    {
        public MenuModel.Discount _discount { get; set; }
        public List<Item> _items { get; set; }
        public CuponCombinedSpecialPrice(List<Item> items, MenuModel.Discount discount) : base(items, discount)
        {
            _discount = discount;
            _items = items;
        }
        

        public override void ApplyCupon()
        {
            //[菜飯x2(60)雞排飯x(80)3,雞腿飯x1(100)] => count: 6
            //[炸豆腐x1(45), 香腸x1(55)] => count: 2
            //6 vs 2 => 2
            //[菜販, 菜販, 雞排飯, 雞排飯, 雞排飯, 雞腿飯] => take 2 => sum (price) => 120
            //[炸豆腐, 香腸] =>100


            var conditions = _discount.Conditions
                .SelectMany((x, index) =>
                                x.Name.Split('|').Select(item => new
                                {
                                    ConditionID = index,
                                    Name = item,
                                    Qty = x.Count
                                })
                           )
                .ToList();

            var matchCondtionItems = _items
                        .Select(item =>
                        {
                            var condition = conditions.FirstOrDefault(con => con.Name.Equals(item.Name));
                            if (condition == null)
                                return null;
                            return new
                            {
                                ConditionID = condition.ConditionID,
                                Name = condition.Name,
                                Price = item.Price,
                                BuyQty = item.Count,
                                ConditionRequire = condition.Qty,
                            };
                        })
                        .Where(x => x != null)
                        .OrderBy(x => x.Price)
                        .GroupBy(x => x.ConditionID);

           
            if (matchCondtionItems.Count() == 0)
            {
                return;
            }

            var matchCountItems = matchCondtionItems
                .Select(item =>
                {
                    int Qty =conditions.First(x => x.ConditionID == item.Key).Qty;
                    return new
                    {
                        ConditionID = item.Key,
                        MatchCount = item.Sum(x => x.BuyQty) / Qty,
                        RequiredQty = Qty
                    };
                })
               .Where(x => x.MatchCount > 0);

            if (matchCountItems.Count() != _discount.Conditions.Length)
            {
                return;
            }

            var matches = matchCountItems.Select(x => new { x.MatchCount, x.RequiredQty });
            int matchCount = matches.Min(x=>x.MatchCount);

            var data = matchCondtionItems.Select(x => x.SelectMany(i => Enumerable.Repeat(new { i.Price ,i.ConditionRequire}, i.BuyQty)).ToList());

            int combinedtotal = 0;

            if (matchCondtionItems.Count() > 1)
            {              
                foreach(var items in data)
                {
                    combinedtotal += items.Take(matchCount * items[0].ConditionRequire).Sum(x=>x.Price);
                }

                var awarditem = _discount.Awards.Select(item =>
                {
                    return new Item
                    {
                        Name = "(優惠)" + _discount.Type,
                        Price = -(combinedtotal - (Convert.ToInt32(item.SetPrice)*matchCount)) ,
                        Count = 1,
                    };

                });
                _items.AddRange(awarditem);
            }

        }
    }
}
