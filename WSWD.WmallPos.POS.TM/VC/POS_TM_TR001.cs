using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using WSWD.WmallPos.POS.FX.Win.Forms;
using WSWD.WmallPos.FX.NetComm;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;
using WSWD.WmallPos.FX.Tasks.Trans;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PP;
using WSWD.WmallPos.FX.Shared.NetComm.Response;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.NetComm.Tasks.PD;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PD;
using WSWD.WmallPos.FX.Shared.NetComm;

namespace WSWD.WmallPos.POS.TM.VC
{
    public partial class POS_TM_TR001 : FormBase
    {
        public POS_TM_TR001()
        {
            InitializeComponent();
            this.Text = "SOCKET통신테스트";

            this.btnPQ01.Click += new EventHandler(btnPQ01_Click);
            this.btnPP02RespData.Click += new EventHandler(btnPP02RespData_Click);
            this.btnPQ06.Click += new EventHandler(btnPQ06_Click);
            this.btnPU01.Click += new EventHandler(btnPU01_Click);
        }

        void task_Errored(string errorMessage, Exception lastException)
        {
            MessageBox.Show(errorMessage);
        }

        void task_TaskCompleted(TaskResponseData responseData)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
                {
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = responseData.DataRecords.ToDataRecords<PQ06RespData>().ToDataTable();
                }
            });
        }

        void task_ProgressChanged(string progressMessage, int percentage)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                label1.Text = progressMessage;
                progressBar1.Value = percentage;
            });
        }

        private void btnPQ06_Click(object sender, EventArgs e)
        {
            var task = new PQ06DataTask(string.Empty);

            task.ProgressChanged += new WSWD.WmallPos.FX.NetComm.Tasks.ProgressedChangedHandler(task_ProgressChanged);
            task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(task_TaskCompleted);
            task.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(task_Errored);
            task.ExecuteTask();
        }

        private void btnPU01_Click(object sender, EventArgs e)
        {
            //var pu01Task = new TransUploadTask();
            // task.ExecuteTask(null, null);
            // pu01Task.ExecuteTest();
        }

        private void btnPQ01_Click(object sender, EventArgs e)
        {
            var pq01 = new PQ01DataTask();
            pq01.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq01_TaskCompleted);
            pq01.ExecuteTask();
        }

        void pq01_TaskCompleted(TaskResponseData responseData)
        {
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ01RespData>()[0];
                MessageBox.Show(data.SystemTime);
            }
        }

        private void btnPP02RespData_Click(object sender, EventArgs e)
        {
            PP02RespData res = new PP02RespData()
            {
                CustCount = 9.ToString().ToString(3, WSWD.WmallPos.FX.Shared.NetComm.Types.TypeProperties.Number)
            };


            res.CustList = new PP02RespDataSub[9];
            for (int i = 0; i < 9; i++)
            {
                res.CustList[i] = new PP02RespDataSub()
                {
                    CustCardNo = "12121212121",
                    CustName = "12121하난"
                };
            }

            string serialText = res.ToString();
            txtTraceLog.AppendText(serialText);

            var ret = (PP02RespData)SerializeClassBase.Parse(typeof(PP02RespData), serialText);

            MessageBox.Show(ret.CustCount);
            MessageBox.Show(ret.CustList.Length.ToString());
        }

        private void btnPD04_Click(object sender, EventArgs e)
        {
            var pd04Task = new PD04DataTask();
            pd04Task.ProgressChanged += new WSWD.WmallPos.FX.NetComm.Tasks.ProgressedChangedHandler(pd04Task_ProgressChanged);
            pd04Task.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pd04Task_TaskCompleted);
            pd04Task.ExecuteTask();
        }

        void pd04Task_TaskCompleted(TaskResponseData responseData)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.Success)
                {
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = responseData.DataRecords.ToDataRecords<PD04RespData>().ToDataTable();
                    MessageBox.Show(responseData.DataRecords.Length.ToString());
                }
            });
        }

        void pd04Task_ProgressChanged(string progressMessage, int percentage)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                label1.Text = progressMessage;
                progressBar1.Value = percentage;
            });
        }
    }
}
