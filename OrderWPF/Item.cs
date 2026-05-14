using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int Total => Price*Count;
        public Item(string name, int price, int count)
        {
            Name = name;
            Price = price;
            Count = count;
        }
        public Item()
        {

        }
    }
}
