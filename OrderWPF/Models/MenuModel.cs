using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using static OrderWPF.Models.MenuModel;

namespace OrderWPF.Models
{
    public class MenuModel
    {
        public Menu[] Menus { get; set; }
        public Discount[] Discounts { get; set; }

        public class Menu
        {
            public string FoodType { get; set; }
            public Food[] Foods { get; set; }
        }

        
        public class Food : INotifyPropertyChanged
        {
            
            public string Name { get; set; }
            public int Price { get; set; }

            private bool _isChecked;
            public bool IsChecked 
            {
                get => _isChecked;
                set
                {
                    _isChecked = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(Quality));
                }
            }

            private int _quality;
            public int Quality 
            {
                get => _quality;
                set
                {
                    _quality = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsChecked));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string name = null) //自動取得「是誰呼叫目前的屬性或方法」
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

        }

       

        public class Discount
        {
            public string Type { get; set; }
            public string DiscountType { get; set; }
            public Condition[] Conditions { get; set; }
            public Award[] Awards { get; set; }
        }

        public class Condition
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int MinPrice { get; set; }
        }

        public class Award
        {
            public string Name { get; set; }
            public int? Count { get; set; }
            public string AwardType { get; set; }
            public object SetPrice { get; set; }
            public int DiscountPrice { get; set; }
            public float DiscountOff { get; set; }
        }

    }
}
