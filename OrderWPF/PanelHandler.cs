using OrderWPF.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public static class PanelHandler
    {
        public static event EventHandler<RenderModel> MenuPanelChanged;      
        public static void NotifyPanel(RenderModel renderModel)
        {
            MenuPanelChanged.Invoke(null, renderModel);
        }       
    }
}
