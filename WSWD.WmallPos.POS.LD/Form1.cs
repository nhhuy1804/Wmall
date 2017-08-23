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
    public partial class Form1 : Form
    {
        List<Button> tableList = new List<Button>();
        private bool isDragging = false;
        Point move;
        private bool isEditting = false;

        public Form1()
        {
            InitializeComponent();
            //for (int i = 0; i < 10; i++)
            //{
            //    Button table = new Button
            //    {
            //        Name = "pictureBox" + i,
            //        Text = "" + (i + 1),
            //        Size = new Size(200, 100),
            //        Location = new Point(i * 50, i * 50),
            //        Font = new Font("Times New Roman", 30.0f, FontStyle.Bold),
            //        BackColor = System.Drawing.SystemColors.ButtonHighlight
            //    };
            //    tableList.Add(table);
            //}

            //foreach (Button p in tableList)
            //{
            //    p.MouseDown += new MouseEventHandler(c_MouseDown);
            //    p.MouseMove += new MouseEventHandler(c_MouseMove);
            //    p.MouseUp += new MouseEventHandler(c_MouseUp);
            //    p.MouseClick += new MouseEventHandler(c_MouseClick);
            //    pnl.Controls.Add(p);
            //    pnl.Refresh();
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormLoad();
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
        }

        void FormLoad()
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
            if (isEditting)
            {
                Control c = sender as Control;
                isDragging = true;
                move = e.Location;
            }
        }
        void c_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isEditting)
            {
                Test t = new Test();
                t.Show();
            }
            else
            {
                Control c = sender as Control;
                isDragging = true;
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
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            isEditting = false;

            for (int i = 0; i < Convert.ToInt32(txt.Text); i++)
            {
                Button table = new Button
                {
                    Name = "pictureBox" + i,
                    Text = "" + (i + 1),
                    Size = new Size(200, 100),
                    Location = new Point(i * 50, i * 50),
                    Font = new Font("Microsoft Sans Serif", 30.0f, FontStyle.Bold),
                    BackColor = System.Drawing.SystemColors.ButtonHighlight
                };
                tableList.Add(table);
            }

            foreach (Button p in tableList)
            {
                p.MouseDown += new MouseEventHandler(c_MouseDown);
                p.MouseMove += new MouseEventHandler(c_MouseMove);
                p.MouseUp += new MouseEventHandler(c_MouseUp);
                p.MouseClick += new MouseEventHandler(c_MouseClick);
                pnl.Controls.Add(p);
                pnl.Refresh();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isEditting = true;
        }
    }
}
