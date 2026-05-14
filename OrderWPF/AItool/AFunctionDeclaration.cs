using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public abstract class AFunctionDeclaration
    {
        // public override string name => "functinoName"
        public abstract string name { get; }
        public abstract string description { get; }
        public abstract AParameter parameters { get; }
        protected string GetToolName(Type toolType)
        {
            return toolType.FullName.Replace(".", "_");
        }

    }
}
