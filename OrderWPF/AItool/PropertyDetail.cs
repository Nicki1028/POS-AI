using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public class PropertyDetail
    {
        public string type { get; set; }
        public string[] @enum { get; set; }

        public AIRequestModel.Items items { get; set; }
        public string description { get; set; }
    }
}
