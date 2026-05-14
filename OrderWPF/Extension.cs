using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace OrderWPF
{
    public static class Extension 
    {
        public static void CreateMenu(this StackPanel stackPanel, string foods, RoutedEventHandler checkboxCheckChange, RoutedPropertyChangedEventHandler<object> valueChange)
        {
            
                // 勾選
                CheckBox checkBox = new CheckBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Content = foods
                };
                checkBox.Checked += checkboxCheckChange;
                checkBox.Unchecked += checkboxCheckChange;

                // 數量
                IntegerUpDown count = new IntegerUpDown
                {
                    Width = 70,
                    Height = 20,
                    Value = 0,
                    VerticalAlignment = VerticalAlignment.Center
                };
                count.ValueChanged += valueChange;
                count.Minimum = 0;
                checkBox.Tag = count;
                count.Tag = checkBox;      

                // 一列用 Grid
                Grid row = new Grid
                {
                    Height = 30,
                    Margin = new Thickness(1)
                };
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // CheckBox
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // 文字
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 數量
               
                Grid.SetColumn(checkBox, 0);           
                Grid.SetColumn(count, 2);

                row.Children.Add(checkBox);            
                row.Children.Add(count);

                stackPanel.Children.Add(row);
            
        }
        //public static void Orderdetail(this StackPanel panel, string item, string price, string count, string total, string note)
        //{

        //    Label itemlabel = new Label();
        //    itemlabel.Content = item;
        //    Label pricelabel = new Label();
        //    pricelabel.Content = price;
        //    Label countlabel = new Label();
        //    countlabel.Content = count;
        //    Label totallabel = new Label();
        //    totallabel.Content = total;
        //    Label notelabel = new Label();
        //    notelabel.Content = note;

        //    Grid row = new Grid
        //    {
        //        Height = 30,
        //        Margin = new Thickness(1)
        //    };
        //    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) }); 
        //    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) }); 
        //    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
        //    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
        //    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        //    Grid.SetColumn(itemlabel, 0);
        //    Grid.SetColumn(pricelabel, 1);
        //    Grid.SetColumn(countlabel, 2);
        //    Grid.SetColumn(totallabel, 3);
        //    Grid.SetColumn(notelabel, 4);

        //    row.Children.Add(itemlabel);
        //    row.Children.Add(pricelabel);
        //    row.Children.Add(countlabel);
        //    row.Children.Add(totallabel);
        //    row.Children.Add(notelabel);
                      
        //    panel.Children.Add(row);
        //}
    }
}
