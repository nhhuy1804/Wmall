using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.TableLayout.VI;
using WSWD.WmallPos.POS.TableLayout.PT;
using WSWD.WmallPos.POS.TableLayout.PI;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared.Utils;

namespace WSWD.WmallPos.POS.TableLayout.VC
{
    public partial class POS_TL_P001 : FormBase, ITLP001View
    {
        private ITLP001Presenter m_p001Presenter;
        List<System.Windows.Forms.Button> tableList = new List<System.Windows.Forms.Button>();
        List<System.Windows.Forms.CheckBox> chkTable = new List<System.Windows.Forms.CheckBox>();
        private bool isDragging = false;
        Point move;
        private bool isEditting = false;
        private bool isDeleting = false;

        public POS_TL_P001()
        {
            InitializeComponent();
            this.HideMainMenu = true;
        }

        private void POS_TL_P001_Load(object sender, EventArgs e)
        {
            m_p001Presenter = new TLP001Presenter(this);
            m_p001Presenter.GetTable(Convert.ToInt32(cbxFloor.Text));
            WSWD.WmallPos.POS.FX.Win.UserControls.Button line = new WSWD.WmallPos.POS.FX.Win.UserControls.Button
            {
                Name = "btnLine",
                Text = "",
                Size = new Size(10, 580),
                Location = new Point(775, 0),
                Enabled = false
            };
            pnlTable.Controls.Add(line);
        }

        public void SetTableList(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    isEditting = false;
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];

                        System.Windows.Forms.Button table = new System.Windows.Forms.Button
                        {
                            Name = "" + dr["Id"],
                            Text = "" + dr["Index"],
                            Size = new Size(150, 80),
                            Location = new Point(Convert.ToInt32(dr["X"]), Convert.ToInt32(dr["Y"])),
                            Font = new Font("Microsoft Sans Serif", 30.0f, FontStyle.Bold),
                            BackColor = System.Drawing.SystemColors.ButtonHighlight
                        };
                        tableList.Add(table);
                    }

                    foreach (System.Windows.Forms.Button p in tableList)
                    {
                        p.MouseDown += new MouseEventHandler(c_MouseDown);
                        p.MouseMove += new MouseEventHandler(c_MouseMove);
                        p.MouseUp += new MouseEventHandler(c_MouseUp);
                        p.MouseClick += new MouseEventHandler(c_MouseClick);
                        pnlTable.Controls.Add(p);
                        pnlTable.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        void c_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEditting)
            {
                Control c = sender as Control;
                isDragging = true;
                move = e.Location;
                RestristMouseMove(sender);
            }
        }
        
        void RestristMouseMove(object sender)
        {
            Control c = sender as Control;
            Size size = new Size(pnlTable.Size.Width - c.Size.Width, pnlTable.Size.Height - c.Size.Height);
            Point point = new Point(pnlTable.Location.X + move.X, pnlTable.Location.Y + move.Y + 50);
            Cursor.Clip = new Rectangle(point, size);
        }

        void c_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isEditting && !isDeleting)
            {
                //Show sales
            }
            if (isEditting)
            {
                Control c = sender as Control;
                isDragging = true;
            }
            if (isDeleting)
            {
                Control c = sender as Control;
                foreach (Control p in pnlTable.Controls)
                {
                    if (p is System.Windows.Forms.CheckBox)
                    {
                        if (p.Name == "c" + c.Name)
                        {
                            if (((System.Windows.Forms.CheckBox)p).Checked)
                            {
                                ((System.Windows.Forms.CheckBox)p).Checked = false;
                            }
                            else
                            {
                                ((System.Windows.Forms.CheckBox)p).Checked = true;
                            }
                        }
                    }
                }
            }
        }

        void c_MouseMove(object sender, MouseEventArgs e)
        {
            if (isEditting)
            {
                if (isDragging == true)
                {
                    Control c = sender as Control;
                    for (int i = 0; i < tableList.Count(); i++)
                    {
                        if (c.Equals(tableList[i]))
                        {
                            tableList[i].Left += e.X - move.X;
                            tableList[i].Top += e.Y - move.Y;
                        }
                    }
                }
            }
        }
        
        void c_MouseUp(object sender, MouseEventArgs e)
        {
            if (isEditting)
            {
                isDragging = false;
                Cursor.Clip = new Rectangle();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isDeleting = false;
            isEditting = true;
            btnDelete.Enabled = btnAdd.Enabled = btnEdit.Enabled = false;
            btnCancel.Enabled = btnSave.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int maxIndex = m_p001Presenter.MaxIndexTable(Convert.ToInt32(cbxFloor.Text));
            try
            {
                for (int i = 0; i<Convert.ToInt32(cbxAddTable.Text); i++)
                {
                    int y = rnd.Next(30, 470);
                    m_p001Presenter.InsertTable(maxIndex + i + 1, 800, y, Convert.ToInt32(cbxFloor.Text));
                }
                isDeleting = isEditting = false;
                ReloadPanel();
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isDeleting = isEditting = false;
            ReloadPanel();
        }

        void ReloadPanel()
        {
            pnlTable.Controls.Clear();
            tableList.Clear();
            m_p001Presenter.GetTable(Convert.ToInt32(cbxFloor.Text));

            WSWD.WmallPos.POS.FX.Win.UserControls.Button line = new WSWD.WmallPos.POS.FX.Win.UserControls.Button
            {
                Name = "btnLine",
                Text = "",
                Size = new Size(10, 580),
                Location = new Point(775, 0),
                Enabled = false
            };
            pnlTable.Controls.Add(line);
            btnAdd.Enabled = btnDelete.Enabled = btnEdit.Enabled = true;
            btnCancel.Enabled = btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isEditting)
            {
                foreach (Control c in pnlTable.Controls)
                {
                    if (c.Name == "btnLine")
                    {

                    }
                    else
                    {
                        m_p001Presenter.UpdateTable(Convert.ToInt32(c.Name), c.Location.X, c.Location.Y);
                    }
                }
            }
            if (isDeleting)
            {
                chkTable.Clear();
                foreach (Control c in pnlTable.Controls)
                {
                    if(c is System.Windows.Forms.CheckBox)
                    {
                        if (((System.Windows.Forms.CheckBox)c).Checked)
                        {
                            m_p001Presenter.DeleteTable(Convert.ToInt32(c.Name.Substring(1)));
                        }
                    }
                }
            }
            
            isDeleting = isEditting = false;
            ReloadPanel();
        }

        private void cbxFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadPanel();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            isEditting = false;
            isDeleting = true;
            btnAdd.Enabled = btnEdit.Enabled = btnDelete.Enabled = false;
            btnCancel.Enabled = btnSave.Enabled = true;
            foreach (Control c in pnlTable.Controls)
            {
                if (c.Name == "btnLine")
                {

                }
                else
                {
                    System.Windows.Forms.CheckBox chk = new System.Windows.Forms.CheckBox
                    {
                        Name = "c" + c.Name,
                        Size = new Size(10, 10),
                        Location = new Point(c.Location.X - 10, c.Location.Y)
                    };
                    pnlTable.Controls.Add(chk);
                }
            }
        }
    }
}
