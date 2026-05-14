using OrderWPF.AItool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AIStrategy
{
    public class DiscountStrategyDeclaration : AFunctionDeclaration
    {
        public override string name => typeof(DiscountStrategy).FullName;

        public override string description => "AI判斷最佳折扣策略";

        public override AParameter parameters => new DiscountStrategyParameter();
    }
}
