using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.UserControls;
using WSWD.WmallPos.POS.FX.Win.Controls;
using System.Windows.Forms;

namespace WSWD.WmallPos.POS.FX.Win.Utils
{
    public class POSCommon
    {
        private static POSCommon m_instance = null;
        public static POSCommon Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new POSCommon();
                }

                return m_instance;
            }
        }

        /// <summary>
        /// InputControl Comma 제거후 리턴
        /// </summary>
        /// <param name="incControl">InputControl</param>
        /// <returns>InputControl Value</returns>
        public decimal GetInputControl(InputControl incControl)
        {
            decimal dReturn = 0;

            if (incControl.Text.ToString().Replace(",","").Length > 0)
            {
                decimal.TryParse(incControl.Text.ToString().Replace(",", ""), out dReturn);
            }

            return dReturn;
        }

        /// <summary>
        /// InputControl Comma 제거후 리턴
        /// </summary>
        /// <param name="incControl">InputControl</param>
        /// <returns>InputControl Value</returns>
        public string GetStringInputControl(InputControl incControl)
        {
            string strReturn = string.Empty;

            if (incControl.Text.ToString().Replace(",", "").Length > 0 && incControl.Text.ToString().Replace(",", "") != "0")
            {
                strReturn = incControl.Text.ToString().Replace(",", "");
            }

            return strReturn;
        }

        public decimal GetInputText(InputText incText)
        {
            decimal dReturn = 0;

            if (incText.Text.ToString().Replace(",", "").Length > 0)
            {
                decimal.TryParse(incText.Text.ToString().Replace(",", ""), out dReturn);
            }

            return dReturn;
        }

        public int GetInputTextInt(InputText incText)
        {
            int iReturn = 0;

            if (incText.Text.ToString().Replace(",", "").Length > 0)
            {
                int.TryParse(incText.Text.ToString().Replace(",", ""), out iReturn);
            }

            return iReturn;
        }

        public decimal GetLabel(Label lbl)
        {
            decimal dReturn = 0;

            if (lbl.Text.ToString().Replace(",", "").Length > 0)
            {
                decimal.TryParse(lbl.Text.ToString().Replace(",", ""), out dReturn);
            }

            return dReturn;
        }

        public int GetLabelInt(Label lbl)
        {
            int iReturn = 0;

            if (lbl.Text.ToString().Replace(",", "").Length > 0)
            {
                int.TryParse(lbl.Text.ToString().Replace(",", ""), out iReturn);
            }

            return iReturn;
        }

        public Int64 GetLabelInt64(Label lbl)
        {
            Int64 iReturn = 0;

            if (lbl.Text.ToString().Replace(",", "").Length > 0)
            {
                Int64.TryParse(lbl.Text.ToString().Replace(",", ""), out iReturn);
            }

            return iReturn;
        }

        public decimal GetCLabel(CLabel lbl)
        {
            decimal dReturn = 0;

            if (lbl.Text.ToString().Replace(",", "").Length > 0)
            {
                decimal.TryParse(lbl.Text.ToString().Replace(",", ""), out dReturn);
            }

            return dReturn;
        }
    }
}
