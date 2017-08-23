using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsEnum
    {
        /// <summary>
        /// 변경여부
        /// </summary>
        public enum eChange
        {
            Normal, //기본
            New,    //신규
            Modify, //수정
            Delete  //삭제
        }

        /// <summary>
        /// 프로그램 현재상황
        /// </summary>
        public enum eProcess
        { 
            eSelect = 0,    //조회
            eSave = 1,      //저장
            eDelete = 2,    //삭제
            eNormal = 3,
            eDownload = 4
        }
    }
}
