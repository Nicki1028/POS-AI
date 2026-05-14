using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OrderWPF.Models.MenuModel;

namespace OrderWPF.DiscountStrategies
{
    public class CuponDiscountPercentage : CuponStrategy
    {
        public MenuModel.Discount _discount {  get; set; }

        public List<Item> _items { get; set; }
        public CuponDiscountPercentage(List<Item> items, MenuModel.Discount discount) : base(items, discount)
        {
            _discount = discount;
            _items = items;
        }

        public override void ApplyCupon()
        {
            var conditions = _discount.Conditions
                .SelectMany((x, index) =>
                             x.Name.Split('|').Select(y => new
                             {
                                 ConditionID = index,
                                 Name = y,
                                 Qty = x.Count,
                                 MinPrice = x.MinPrice
                             })
                            )
                .ToList();

            var matchPriceItems = 0;
            if (String.IsNullOrEmpty(conditions[0].Name))
            {
                matchPriceItems = _items.Sum(item => item.Total);
            }
            if (matchPriceItems > conditions[0].MinPrice) 
            {
                var awarditem = _discount.Awards
                           .Select(item => new Item
                           {
                               Name = "(折扣)" + _discount.Type,
                               Price = (int)-(matchPriceItems * (1 - item.DiscountOff)),
                               Count = 1
                           })
                           .ToList();
                _items.AddRange(awarditem);
            }

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
                        .OrderBy(x => x.Price);


            var matchCountItems = matchCondtionItems
                                   .GroupBy(x => new { x.ConditionID, x.ConditionRequire })
                                   .Select(item =>
                                   {
                                       return new
                                       {
                                           ConditionID = item.Key.ConditionID,
                                           MatchCount = item.Sum(x => x.BuyQty) / item.Key.ConditionRequire
                                       };
                                   })
                                   .Where(x => x.MatchCount > 0)
                                   .ToList();

            
            if (matchCountItems.Count != _discount.Conditions.Length)
            {
                return;
            }

            int matchCount = matchCountItems.Min(x => x.MatchCount);
            var data = matchCondtionItems.GroupBy(x => x.ConditionID).Select(x => x.SelectMany(i => Enumerable.Repeat(new { i.Price, i.ConditionRequire }, i.BuyQty)).ToList());

            int combinedtotal = 0;

            if (matchCondtionItems.Count() > 0)
            {
                foreach (var items in data)
                {
                    combinedtotal += items.Take(matchCount * items[0].ConditionRequire).Sum(x => x.Price);
                }

                var awarditem = _discount.Awards
                            .Select(item => new Item
                            {
                                Name = "(折扣)" + _discount.Type,
                                Price = (int)-(combinedtotal * (1 - item.DiscountOff)),
                                Count = 1 
                            })
                            .ToList();
                _items.AddRange(awarditem);
            }

        }
             
    }
}
