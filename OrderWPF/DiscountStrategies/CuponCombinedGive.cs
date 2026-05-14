using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static OrderWPF.Models.MenuModel;

namespace OrderWPF.DiscountStrategies
{
    public class CuponCombinedGive : CuponStrategy
    {
        public MenuModel.Discount _discount { get; set; }
        public List<Item> _items { get; set; }
        public CuponCombinedGive(List<Item> items, MenuModel.Discount discount) : base(items, discount)
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

            int awardnumber = matchCountItems.Min(x => x.MatchCount);

            _items.AddRange(
                 Enumerable.Repeat(1, awardnumber)
                .SelectMany(x => _discount.Awards
                           .Select(item => new Item
                           {
                               Name = "(贈送)" + item.Name.Split('|').OrderBy(i => new Random(Guid.NewGuid().GetHashCode()).Next()).First(),
                               Price = 0,
                               Count = (int)item.Count
                           }).ToList())
                .GroupBy(x => x.Name).Select(x => new Item
                {
                    Name = x.Key,
                    Price = 0,
                    Count = x.Sum(y => y.Count)
                }));
            //List<Item> awarditems = new List<Item>();

            //for (int i = 0; i < awardnumber; i++)
            //{
            //    var awarditem = _discount.Awards
            //               .Select(item => new Item
            //               {
            //                   Name = "(贈送)" + item.Name.Split('|').OrderBy(x => new Random(Guid.NewGuid().GetHashCode()).Next()).First(),
            //                   Price = 0,
            //                   Count = (int)item.Count
            //               });
            //    awarditems.AddRange(awarditem);          
            //}

            //_items.AddRange(awarditems.GroupBy(x => x.Name).Select(x => new Item
            //{
            //    Name = x.Key,
            //    Price = 0,
            //    Count = x.Sum(y => y.Count)
            //}).ToList());

        }
    }
}
