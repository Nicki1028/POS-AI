using OrderWPF.AItool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AIStrategy
{
    public class DiscountStrategy : AFunctionCall
    {
        public override object FunctionCall(object parameters)
        {          
            return parameters;
        }
    }
}
