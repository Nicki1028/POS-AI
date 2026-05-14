using OrderWPF.DiscountStrategies;
using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderWPF
{
    public class Discount
    {
        public static async Task CuponUse(OrderRequestModel orderRequestModel) 
        {
            RenderModel data = await new StrategyContext(orderRequestModel).GetCuponStrategyAsync();          
            ShowPanel.RenderOrder(data);
        }
    }
}
