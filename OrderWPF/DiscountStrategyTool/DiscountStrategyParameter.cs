using OrderWPF.AItool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AIStrategy
{
    public class DiscountStrategyParameter : AParameter
    {
        public override string type => "object";
        public override object properties => new
        {
            discountName = new PropertyDetail()
            {
                type = "string",
                description = "優惠的名稱"
            },

            discountType = new PropertyDetail()
            {
                type = "string",
                description = "優惠的策略"
            },

            reason = new PropertyDetail()
            {
                type = "string",
                description = "選擇此優惠的理由"
            }

        };
        public override string[] required => new[] { "discountName", "discountType", "reason" };
    }
}
