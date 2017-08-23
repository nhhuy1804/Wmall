using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.FX.Shared;

namespace WSWD.WmallPos.POS.FX.Win.Forms
{
    public partial class InputDialogBase : KeyInputForm, IChildFormView
    {
        public InputDialogBase()
        {
            InitializeComponent();
            this.Load += new EventHandler(InputDialogBase_Load);
            this.FormClosed += new FormClosedEventHandler(InputDialogBase_FormClosed);
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
        }

        void InputDialogBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Load -= new EventHandler(InputDialogBase_Load);
            this.FormClosed -= new FormClosedEventHandler(InputDialogBase_FormClosed);
        }

        void InputDialogBase_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (this.Owner == null)
                {
                    this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
                    this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
                }
            }
        }

        #region Properties

        /// <summary>
        /// MainFrame
        /// </summary>
        public IChildFormManager ChildManager { get; set; }

        #endregion

        #region 기본설정

        /// <summary>
        /// 팝업타이틀
        /// </summary>
        public override string Text
        {
            get
            {
                return lblPopupTitle != null ? lblPopupTitle.Text : base.Text;
            }
            set
            {
                base.Text = string.Empty;
                if (lblPopupTitle != null)
                {
                    lblPopupTitle.Text = value;
                }
            }
        }

        #endregion
    }
}
