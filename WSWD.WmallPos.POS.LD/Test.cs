using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.LD
{
    public partial class Test : Form
    {
        List<PictureBox> pictureBoxList = new List<PictureBox>();
        private bool isDragging = false;
        Point move;

        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            TestFormLoad();
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
        }

        void TestFormLoad()
        {
            if (Screen.AllScreens[1].WorkingArea.Width > 1024)
            {
                this.Left = 100;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        void c_MouseDown(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            isDragging = true;
            move = e.Location;
        }

        void c_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDragging == true)
            {
                Control c = sender as Control;
                for (int i = 0; i < pictureBoxList.Count(); i++)
                {
                    if (c.Equals(pictureBoxList[i]))
                    {
                        pictureBoxList[i].Left += e.X - move.X;
                        pictureBoxList[i].Top += e.Y - move.Y;
                    }
                }
            }
        }

        void c_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        void c_Click(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Int32.Parse(txtCout.Text); i++)
            {

                PictureBox picture = new PictureBox
                {
                    Name = "pictureBox" + i,
                    Size = new Size(200, 100),
                    Location = new Point(i * 50, i * 50),
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = System.Drawing.SystemColors.ButtonHighlight
                };
                pictureBoxList.Add(picture);
            }

            foreach (PictureBox p in pictureBoxList)
            {
                p.MouseDown += new MouseEventHandler(c_MouseDown);
                p.MouseMove += new MouseEventHandler(c_MouseMove);
                p.MouseUp += new MouseEventHandler(c_MouseUp);
                pnlTableGroup.Controls.Add(p);
                pnlTableGroup.Refresh();
            }
            btnEnter.Enabled = false;
        }
    }
}
