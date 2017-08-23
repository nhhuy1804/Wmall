using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsINI
    {
        private static clsUtilsINI m_instance = new clsUtilsINI();
        public static clsUtilsINI Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// INI 파일 읽기
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// ini 파일 읽기(컨트롤 셋팅)
        /// </summary>
        /// <param name="strSection">섹션</param>
        /// <param name="strKey">키</param>
        /// <param name="strLocalPath">로컬파일경로</param>
        /// <param name="dt">ini파일내용 임시저장할 데이터테이블</param>
        /// <returns></returns>
        public string GetConfigValue(string strSection, string strKey, string strLocalPath, DataTable dt)
        {
            StringBuilder temp = new StringBuilder(255);

            try
            {
                int iRet = GetPrivateProfileString(strSection, strKey, "", temp, 255, strLocalPath);
            }
            catch (Exception ex)
            {
                temp = new StringBuilder(255);
            }

            return temp.ToString().Trim();
        }

        /// <summary>
        /// INI 파일 쓰기
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// ini 파일 쓰기
        /// </summary>
        /// <param name="strSection">섹션</param>
        /// <param name="strKey">키</param>
        /// <param name="strLocalPath">파일경로</param>
        /// <returns></returns>
        public bool SetConfigValue(string strSection, string strKey, string strValue, string strLocalPath)
        {
            try
            {
                WritePrivateProfileString(strSection, strKey, strValue, strLocalPath);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
