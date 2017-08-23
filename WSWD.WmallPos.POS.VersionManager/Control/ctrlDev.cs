using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.VersionManager.Utils;

namespace WSWD.WmallPos.POS.VersionManager.Control
{
    public partial class ctrlDev : UserControl
    {
        private DataTable _dtDev;
        public DataTable dtDev
        {
            get { return this._dtDev; }
            set { this._dtDev = value; }
        }

        public ctrlDev()
        {
            InitializeComponent();

            InitControl();

            InitEvent();
        }

        private void InitControl()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void InitEvent()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
