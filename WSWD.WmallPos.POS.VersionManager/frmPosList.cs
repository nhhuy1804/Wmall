using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.VersionManager
{
    public partial class frmPosList : Form
    {
        public frmPosList(string PosNo)
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("colPosNo");

            if (PosNo.Length > 0)
            {
                string[] arrPosNo = PosNo.Split(',');

                foreach (string item in arrPosNo)
                {
                    if (item.Trim().Length > 0)
                    {
                        dt.Rows.Add(item.Trim());    
                    }
                }
            }

            grdPosNo.DataSource = dt;
        }
    }
}
