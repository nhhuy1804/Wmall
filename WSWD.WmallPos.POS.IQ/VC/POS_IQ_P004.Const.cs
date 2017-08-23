//-----------------------------------------------------------------
/*
 * 화면명   : POS_IQ_P004.Consts.cs
 * 화면설명 : 영수증 조회
 * 개발자   : 정광호
 * 개발일자 : 2015.05.
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.IQ.VC
{
    partial class POS_IQ_P004
    {
        static string strMsg01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00248");
        static string strMsg02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00274");
        static string strMsg03 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00275");
        static string strMsg04 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00276");
        static string strMsg05 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00277");
        static string strMsg06 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00278");
        static string strMsg07 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00279");
        static string strMsg08 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00280");
        static string strMsg09 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00281");
        static string strMsg10 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00282");
        static string strMsg11 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00283");
        static string strMsg12 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00284");
        static string strMsg13 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00285");
        static string strMsg14 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00107");
        static string strMsg15 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00286");
        static string strMsg16 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00287");
        static string strMsg17 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00288");
        static string strMsg18 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00091");
        static string strMsg19 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00289");
        static string strMsg20 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00290");
        static string strMsg21 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00291");
        static string strMsg22 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00292");
        static string strMsg23 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00293");
        static string strMsg24 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00072");
        static string strMsg25 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00864");//"IN_사은품반납";
        static string strMsg26 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00866");//"IN_해당점포의 영수증만 조회할수 있습니다.";
        static string strMsg27 = "저장물";

        /// <summary>
        /// 여전법 추가 0604
        /// </summary>
        static string MSG_INPUT_APPRNO = "승인번호를 입력 해주세요.";
        static string MSG_INPUT_PREFIX = "프라픽스를 입력 해주세요.";
        static string MSG_INPUT_PBNO = "품번코드를 입력 해주세요.";

        static string strCash01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00259");
        static string strCash02 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00294");
        static string strCash03 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00295");
        static string strCash04 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00296");
        static string strCash05 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00297");
        static string strCash06 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00298");
        static string strCash07 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00299");
        static string strCash08 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00300");
        static string strCash09 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00301");
        static string strTicket01 = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00302");

        static string MSG_VAN_REQ_PROCESSING = WSWD.WmallPos.FX.Shared.ConfigData.Current.SysMessage.GetMessage("00180");


        const string strColNo = "colNo";
        const string strColTime = "colTime";
        const string strColAmt = "colAmt";
        const string strColType = "colType";
        const string strColDesc = "colDesc";
        const string strColRealAmt = "colRealAmt";
        const string strDTNm = "DESC";
        const string strDTColNm = "VC_CONT";
        const string strDTColTrxn = "NO_TRXN";
    }
}
