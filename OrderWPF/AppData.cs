using Newtonsoft.Json;
using OrderWPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public class AppData
    {
       public  static string MenuData;
       public  static OrderWPF.Models.MenuModel.Menu[] Menus;
       public  static OrderWPF.Models.MenuModel.Discount[] Discounts;
        static AppData()
        {
            string menuPath = ConfigurationManager.AppSettings["MenuPath"];

            string fullPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                menuPath);
            MenuData = File.ReadAllText(fullPath);
            MenuModel model = JsonConvert.DeserializeObject<MenuModel>(MenuData);
            Menus = model.Menus;
            Discounts = model.Discounts;
        }

    }
}
