using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OrderWPF.Models.MenuModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace OrderWPF.DiscountStrategies
{
    public class CuponBuyItemsGive : CuponStrategy
    {
        public MenuModel.Discount _discount { get; set; }
        public List<Item> _items { get; set; }

       
        public CuponBuyItemsGive(List<Item> items, MenuModel.Discount discount) : base(items, discount)
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
                                    .OrderByDescending(x => x.Price);

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

            var awarditem = _discount.Awards.Select(item =>
            {
                if (item.AwardType == "Min")
                {
                    return new Item
                    {
                        Name = "(贈送)" + matchCondtionItems.Last().Name,
                        Price = 0,
                        Count = 1 * matchCountItems.Min(x => x.MatchCount),
                    };
                }
                if (item.AwardType == "Max")
                {
                    return new Item
                    {
                        Name = "(贈送)" + matchCondtionItems.First().Name,
                        Price = 0,
                        Count = 1 * matchCountItems.Min(x => x.MatchCount),
                    };
                }
                return new Item
                {
                    Name = "(贈送)" + item.Name,
                    Price = 0,
                    Count = (int)item.Count * matchCountItems.Min(x => x.MatchCount)
                };
            });
            _items.AddRange(awarditem);

            //foreach (var award in _discount.Awards)
            //{
            //    _items.Add(new Item
            //    {
            //        Name = "(贈送)" + award.Name,
            //        Price = 0,
            //        Count = (int)award.Count * temp.Min(x => x.MatchCount)
            //    });
            //}

            //foreach (var cond in _discount.Conditions)
            //{

            //    var targetNames = cond.Name.Split('|');

            //    int conditionCount = _items
            //        .Where(item => targetNames.Contains(item.Name))
            //        .Sum(item => item.Count);

            //    conditionCount = conditionCount / cond.Count;

            //    if (conditionCount >= 1)
            //    {
            //        conditionCounts.Add(conditionCount);
            //    }
            //}

            //if (conditionCounts.Count > 0 && conditionCounts.Min() != 0 && conditionCounts.Count == _discount.Conditions.Length)
            //{
            //    int minCount = conditionCounts.Min();
            //    foreach (var award in _discount.Awards)
            //    {
            //        _items.Add(new Item
            //        {
            //            Name = "(贈送)" + award.Name,
            //            Price = 0,
            //            Count = (int)award.Count * minCount
            //        });

            //        //    }
            //        //}
            //    }
            //}
        }
    }

}
