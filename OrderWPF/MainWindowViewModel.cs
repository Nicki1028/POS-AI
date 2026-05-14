using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using static OrderWPF.Models.MenuModel;
using Menu = OrderWPF.Models.MenuModel.Menu;
using OrderWPF.Models;
using OrderWPF.Components;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrderWPF
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        private string _reason;
        public string reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                OnPropertyChanged();
            }
        }

        private int _totalPay;
        public int TotalPay 
        {
            get { return _totalPay; }
            set 
            {
                _totalPay = value;
                OnPropertyChanged();
            } 
        }

        public bool AIchecked { get; set; }
        public ObservableCollection<Item> OrderItems { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
        public ObservableCollection<MenuModel.Discount> Discounts { get; set; }
        public MenuModel.Discount SelectedCupon {  get; set; }
        
        private MenuPanel _menuPanel;
        public MenuPanel MenuPanel
        {
            get { return _menuPanel; }
            set { _menuPanel = value; 
                  OnPropertyChanged(nameof(MenuPanel));    
            }
        }    
        public ICommand CheckChangeCommand { get; set; }
        public ICommand ValueChangeCommand { get; set; }
        public ICommand SelectCuponCommand { get; set; }
        public ICommand IsAICheckedCommand { get; set; }
        public ICommand ComputeTotalCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindowViewModel() 
        {
            PanelHandler.MenuPanelChanged += PanelHandler_MenuPanelChanged;

            Menus = new ObservableCollection<Menu>(AppData.Menus);
            Discounts = new ObservableCollection<MenuModel.Discount>(AppData.Discounts);
            SelectedCupon = Discounts[0];
            this.CheckChangeCommand = new RelayCommand<Food>(data =>
            {
                data.Quality = data.IsChecked == true ? 1 : 0;
            });

            this.ValueChangeCommand = new RelayCommand<Food>(async data =>
            {
                data.IsChecked = data.Quality > 0 ? true : false;
                Item item = new Item(data.Name, data.Price, data.Quality);
                OrderRequestModel OrderRequest = new OrderRequestModel(SelectedCupon, item, AIchecked);

                await Order.Updateitem(OrderRequest);
            });

            this.SelectCuponCommand = new RelayCommand(async () =>
            {
                OrderRequestModel OrderRequest = new OrderRequestModel(SelectedCupon);
                await Order.ChangeCuponDiscount(OrderRequest);
            });

            this.IsAICheckedCommand = new RelayCommand(async () =>
            {
                if (AIchecked == true)
                {
                    OrderRequestModel OrderRequest = new OrderRequestModel(AIchecked);
                    await Order.ChangeCuponDiscount(OrderRequest);
                }
                
            });
            
        }

        private void PanelHandler_MenuPanelChanged(object sender, RenderModel e)
        {
            this.MenuPanel = new MenuPanel(e.renderItem);
            this.reason = e.stragtegyReason;
            this.TotalPay = e.TotalPay;
        }

        //MenuPanel+reason重新包裝一個類別傳入到PanelHandler
       
        protected void OnPropertyChanged([CallerMemberName] string name = null) //自動取得「是誰呼叫目前的屬性或方法」
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
