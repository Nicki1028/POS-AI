using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public class RenderModel
    {
        public List<Item> renderItem {  get; set; }

        public string stragtegyReason {  get; set; }

        public int TotalPay {  get; set; }

        public RenderModel(List<Item> items, string reason) 
        {
            renderItem = items;
            stragtegyReason = reason;
            TotalPay = items.Sum(x => x.Total);
        }
    }
}
