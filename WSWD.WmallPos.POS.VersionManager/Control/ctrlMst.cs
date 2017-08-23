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
    public partial class ctrlMst : UserControl
    {
        public ctrlMst()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 텍스트내용 삭제
        /// </summary>
        public void Clear()
        {
            txtMst.Clear();
        }
    }
}
