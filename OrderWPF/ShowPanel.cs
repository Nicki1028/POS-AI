using OrderWPF.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    public static class ShowPanel
    {
        public static void RenderOrder(RenderModel renderModel)
        {
            PanelHandler.NotifyPanel(renderModel);
        }

        
    }
}
