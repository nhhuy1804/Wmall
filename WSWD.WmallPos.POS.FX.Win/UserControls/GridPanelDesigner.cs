using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;

namespace WSWD.WmallPos.POS.FX.Win.UserControls
{
    public class GridPanelDesigner : ControlDesigner
    {
        public override void Initialize(IComponent c)
        {
            base.Initialize(c);
            GridPanel ctl = (GridPanel)c;
            //EnableDesignMode(ctl.ContainerZone, "ContainerZone");
        }
    }
}
