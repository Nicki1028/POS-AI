using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.Components
{
    public class MenuPanelViewModel
    {
        public ObservableCollection<Item> OrderItems { get; set; }       
        public MenuPanelViewModel(List<Item> items) 
        {
            OrderItems = new ObservableCollection<Item>(items);           
        }
    }
}
