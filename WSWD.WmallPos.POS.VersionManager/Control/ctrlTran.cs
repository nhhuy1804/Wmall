using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.VersionManager.Control
{
    public partial class ctrlTran : UserControl
    {
        public ctrlTran()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 내용 삭제
        /// </summary>
        public void Clear()
        {
            txtTran.Clear();
        }
    }
}
