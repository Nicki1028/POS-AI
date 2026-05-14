using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public abstract class AParameter
    {
        //根據跟AI對話，AI所擷取我要的結果，成為參數
        public abstract string type { get; }
        public abstract object properties { get; }
        public abstract string[] required { get; }
    }
}
