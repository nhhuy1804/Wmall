using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.VersionManager.Utils
{
    public class clsUtilsSTRING
    {
        public const string conAppConfig = "AppConfig.ini";
        public const string conDevConfig = "DevConfig.ini";
        public const string conMstChanges = "mstchanges.sql";
        public const string conTranChanges = "tranchanges.sql";
        public const string conBin = "bin";
        public const string conLib = "lib";
        public const string conDamo = "damo";
        public const string conOcx = "ocx";
        public const string conConfig = "config";
        public const string conData = "data";
        public const string conMst = "mst";
        public const string conSchema = "schema";
        public const string conTran = "tran";
        public const string conPatch = "patch";
        public const string conMstChange = "mstchange";
        public const string conTranChange = "tranchange";
        public const string conResource = "resource";
        public const string conUpdate = "update";
        public const string conVersion = "version.xml";
        public const string conVersionCopy = "versionCopy.xml";
        public const string conFile = "File";
        public const string conSaveList = "SaveList";

        public const string conAppSection = "AppSection";
        public const string conAppKey = "AppKey";
        public const string conDevSection = "DevSection";
        public const string conDevKey = "DevKey";

        public const string conCD_STORE = "CD_STORE";
        public const string conNO_POS = "NO_POS";

        public const string conServerPOS = "pos";
        public const string conServerPDA = "pda";

        public const string conSlush = "/";
        public const string conY = "Y";
        public const string conN = "N";
        public const string conFTPUploadFail = "FTP 저장에 실패하였습니다";
        public const string conFTPDeleteFail = "FTP 삭제에 실패하였습니다";


        public const string conINIPosComm = "PosComm";
        public const string conINIPosComm01 = "PosComm01";
        public const string conINIPosComm02 = "PosComm02";
        public const string conINIPosFTP = "PosFTP";
        public const string conINIPosVan = "PosVan";
        public const string conINIPosOption = "PosOption";

        public const string conINIPosComm01Nm = "점서버";
        public const string conINIPosComm02Nm = "본부서버";
        public const string conINIPosFTPNm = "FTP";
        public const string conINIPosVanNm = "VAN";
        public const string conINIPosOptionNm = "기타옵션";

        public const string conPosCommSvrIP1 = "SvrIP1";
        public const string conPosCommSvrIP2 = "SvrIP2";
        public const string conPosCommComDPort1 = "ComDPort1";
        public const string conPosCommComDPort2 = "ComDPort2";
        public const string conPosCommComDTimeOut = "ComDTimeOut";
        public const string conPosCommComUPort1 = "ComUPort1";
        public const string conPosCommComUPort2 = "ComUPort2";
        public const string conPosCommComUTimeOut = "ComUTimeOut";
        public const string conPosCommComQPort1 = "ComQPort1";
        public const string conPosCommComQPort2 = "ComQPort2";
        public const string conPosCommComQTimeOut = "ComQTimeOut";

        public const string conPosCommSvrIP1Nm = "1차 점서버 IP";
        public const string conPosCommSvrIP2Nm = "2차 점서버 IP";
        public const string conPosCommComDPort1Nm = "1차 다운로드 PORT";
        public const string conPosCommComDPort2Nm = "2차 다운로드 PORT";
        public const string conPosCommComDTimeOutNm = "다운로드 대기시간";
        public const string conPosCommComUPort1Nm = "1차 업로드 PORT";
        public const string conPosCommComUPort2Nm = "2차 업로드 PORT";
        public const string conPosCommComUTimeOutNm = "업로드 대기시간";
        public const string conPosCommComQPort1Nm = "1차 조회 PORT";
        public const string conPosCommComQPort2Nm = "2차 조회 PORT";
        public const string conPosCommComQTimeOutNm = "조회 대기시간";

        public const string conPosCommSvrGftIP1 = "SvrGftIP1";
        public const string conPosCommSvrGftIP2 = "SvrGftIP2";
        public const string conPosCommComGPort1 = "ComGPort1";
        public const string conPosCommComGPort2 = "ComGPort2";
        public const string conPosCommComGTimeOut = "ComGTimeOut";
        public const string conPosCommSvrPntIP1 = "SvrPntIP1";
        public const string conPosCommSvrPntIP2 = "SvrPntIP2";
        public const string conPosCommComPPort1 = "ComPPort1";
        public const string conPosCommComPPort2 = "ComPPort2";
        public const string conPosCommComPTimeOut = "ComPTimeOut";
        public const string conPosCommHqSvrIP1 = "HqSvrIP1";
        public const string conPosCommHqSvrIP2 = "HqSvrIP2";
        public const string conPosCommHqComQPort1 = "HqComQPort1";
        public const string conPosCommHqComQPort2 = "HqComQPort2";
        public const string conPosCommHqComQTimeOut = "HqComQTimeOut";
        public const string conPosCommHqComUPort1 = "HqComUPort1";
        public const string conPosCommHqComUPort2 = "HqComUPort2";
        public const string conPosCommHqComUTimeOut = "HqComUTimeOut";

        public const string conPosCommSvrGftIP1Nm = "상품권 1차 서버 IP";
        public const string conPosCommSvrGftIP2Nm = "상품권 2차 서버 IP";
        public const string conPosCommComGPort1Nm = "상품권 1차 PORT";
        public const string conPosCommComGPort2Nm = "상품권 2차 PORT";
        public const string conPosCommComGTimeOutNm = "상품권 대기시간";
        public const string conPosCommSvrPntIP1Nm = "포인트 1차 서버 IP";
        public const string conPosCommSvrPntIP2Nm = "포인트 2차 서버 IP";
        public const string conPosCommComPPort1Nm = "포인트 1차 PORT";
        public const string conPosCommComPPort2Nm = "포인트 2차 PORT";
        public const string conPosCommComPTimeOutNm = "포인트 대기시간";
        public const string conPosCommHqSvrIP1Nm = "기타 1차 서버 IP";
        public const string conPosCommHqSvrIP2Nm = "기타 2차 서버 IP";
        public const string conPosCommHqComQPort1Nm = "기타 1차 조회 PORT";
        public const string conPosCommHqComQPort2Nm = "기타 2차 조회 PORT";
        public const string conPosCommHqComQTimeOutNm = "기타 조회 대기시간";
        public const string conPosCommHqComUPort1Nm = "기타 1차 업로드 PORT";
        public const string conPosCommHqComUPort2Nm = "기타 2차 업로드 PORT";
        public const string conPosCommHqComUTimeOutNm = "기타 업로드 대기시간";

        public const string conPosFTPFtpSvrIP1 = "FtpSvrIP1";
        public const string conPosFTPFtpSvrIP2 = "FtpSvrIP2";
        public const string conPosFTPFtpComPort1 = "FtpComPort1";
        public const string conPosFTPFtpComPort2 = "FtpComPort2";
        public const string conPosFTPMode = "Mode";
        public const string conPosFTPUser = "User";
        public const string conPosFTPPass = "Pass";
        public const string conPosFTPJournalPath = "JournalPath";
        public const string conPosFTPVersionInfoPath = "VersionInfoPath";
        public const string conPosFTPDataFileDownloadPath = "DataFileDownloadPath";
        public const string conPosFTPCreateUploadPathByDate = "CreateUploadPathByDate";

        public const string conPosFTPFtpSvrIP1Nm = "1차 서버 IP";
        public const string conPosFTPFtpSvrIP2Nm = "2차 서버 IP";
        public const string conPosFTPFtpComPort1Nm = "1차 PORT";
        public const string conPosFTPFtpComPort2Nm = "2차 PORT";
        public const string conPosFTPModeNm = "Mode";
        public const string conPosFTPUserNm = "사용자 아이디";
        public const string conPosFTPPassNm = "사용자 패스워드";
        public const string conPosFTPJournalPathNm = "저널 업로드 경로";
        public const string conPosFTPVersionInfoPathNm = "데이터 다운로드 경로";
        public const string conPosFTPDataFileDownloadPathNm = "버전 업데이트 경로";
        public const string conPosFTPCreateUploadPathByDateNm = "서버 업로드 경로";

        public const string conPosVANVanSvrIP1 = "VanSvrIP1";
        public const string conPosVANVanSvrIP2 = "VanSvrIP2";
        public const string conPosVANVanComPort1 = "VanComPort1";
        public const string conPosVANVanComPort2 = "VanComPort2";
        public const string conPosVANComTimeOut = "ComTimeOut";

        public const string conPosVANVanSvrIP1Nm = "1차 서버 IP";
        public const string conPosVANVanSvrIP2Nm = "2차 서버 IP";
        public const string conPosVANVanComPort1Nm = "1차 PORT";
        public const string conPosVANVanComPort2Nm = "2차 PORT";
        public const string conPosVANComTimeOutNm = "대기시간";

        public const string conPosOptionPointUse = "PointUse";
        public const string conPosOptionPointSchemePrefix = "PointSchemePrefix";
        public const string conPosOptionPointPayKeyInputEnable = "PointPayKeyInputEnable";
        public const string conPosOptionCashReceiptUse = "CashReceiptUse";
        public const string conPosOptionCashReceiptIssue = "CashReceiptIssue";
        public const string conPosOptionCashReceiptApplAmount = "CashReceiptApplAmount";
        public const string conPosOptionGoodsCodePrefix1 = "GoodsCodePrefix1";
        public const string conPosOptionGoodsCodePrefix2 = "GoodsCodePrefix2";
        public const string conPosOptionDataKeepDays = "DataKeepDays";
        public const string conPosOptionAutoReturnCardRead = "AutoReturnCardRead";
        public const string conPosOptionSalesReturn = "SalesReturn";
        public const string conPosOptionSignUploadTask = "SignUploadTask";
        public const string conPosOptionTransUploadTask = "TransUploadTask";
        public const string conPosOptionTransStatusTask = "TransStatusTask";
        public const string conPosOptionNoticeStatusTask = "NoticeStatusTask";
        public const string conPosOptionDefAdminPass = "DefAdminPass";
        public const string conPosOptionUploadTask = "UploadTask";
        public const string conPosOptionStatusTask = "StatusTask";
        public const string conPosOptionNoticeTask = "NoticeTask";
        public const string conPosOptionSignTask = "SignTask";

        public const string conPosOptionPointUseNm = "포인트 업무 사용여부";
        public const string conPosOptionPointSchemePrefixNm = "포인트 Prefix";
        public const string conPosOptionPointPayKeyInputEnableNm = "포인트 지불시 키입력";
        public const string conPosOptionCashReceiptUseNm = "현금영수증 업무 사용여부";
        public const string conPosOptionCashReceiptIssueNm = "현금영수증 자진 발급가능";
        public const string conPosOptionCashReceiptApplAmountNm = "현금영수증 적용 가능 금액";
        public const string conPosOptionGoodsCodePrefix1Nm = "상품 Prefix 품번 1단 Tag";
        public const string conPosOptionGoodsCodePrefix2Nm = "상품 Prefix 품번 2단 Tag";
        public const string conPosOptionDataKeepDaysNm = "기타 자료보관일수";
        public const string conPosOptionAutoReturnCardReadNm = "기타 자동반품시 카드 READ";
        public const string conPosOptionSalesReturnNm = "기타 반품 가능여부";
        public const string conPosOptionSignUploadTaskNm = "기타 싸인파일 업로드 대기시간";
        public const string conPosOptionTransUploadTaskNm = "기타 TR 업로드 대기시간";
        public const string conPosOptionTransStatusTaskNm = "기타 전송상태 업데이트 대기시간";
        public const string conPosOptionNoticeStatusTaskNm = "기타 공지사항 확인 대기시간";
        public const string conPosOptionDefAdminPassNm = "DefAdminPass";
        public const string conPosOptionUploadTaskNm = "UploadTask";
        public const string conPosOptionStatusTaskNm = "StatusTask";
        public const string conPosOptionNoticeTaskNm = "NoticeTask";
        public const string conPosOptionSignTaskNm = "SignTask";

        public const string conINIScannerGun = "ScannerGun";
        public const string conINILineDisplay = "LineDisplay";
        public const string conINIPrinter = "Printer";
        public const string conINIMSR = "MSR";
        public const string conINICashDrawer = "CashDrawer";
        public const string conINISignPad = "SignPad";

        public const string conUse = "Use";
        public const string conMethod = "Method";
        public const string conLogicalName = "LogicalName";
        public const string conPort = "Port";
        public const string conSpeed = "Speed";
        public const string conDataBit = "DataBit";
        public const string conStopBit = "StopBit";
        public const string conParity = "Parity";
        public const string conFlowControl = "FlowControl";
        public const string conLogoBMP = "LogoBMP";
        public const string conCutFeedCn = "CutFeedCn";
        public const string conBarCodeWidth = "BarCodeWidth";
        public const string conBarCodeHeight = "BarCodeHeight";

        public const string conUseNm = "사용여부";
        public const string conMethodNm01 = "처리방식";
        public const string conMethodNm02 = "Model";
        public const string conLogicalNameNm = "디바이스명";
        public const string conPortNm = "포트번호";
        public const string conSpeedNm = "통신속도";
        public const string conDataBitNm = "데이터비트";
        public const string conStopBitNm = "정지비트";
        public const string conParityNm = "페리티";
        public const string conFlowControlNm = "흐름제어";
        public const string conLogoBMPNm = "로고이미지 경로";
        public const string conCutFeedCnNm = "Cut전 Line Feed";
        public const string conBarCodeWidthNm = "바코드 출력 넓이";
        public const string conBarCodeHeightNm = "바코드 출력 높이";


        public const string conGroupListNm = "업그레이드 상세 목록";



        public const string conStatusSelect = " 조회중입니다. 잠시만 기다리십시오.";
        public const string conStatusSave = " 서버적용중입니다. 잠시만 기다리십시오.";
        public const string conStatusDelete = " 업그레이드 정보 삭제중입니다. 잠시만 기다리십시오.";

        public const string conProgressSave = " 파일을 저장중입니다.";
        public const string conProgressSaveComplete = " 파일 저장에 성공하였습니다.";
        public const string conProgressDelete = " 파일을 삭제중입니다.";
        public const string conProgressDeleteComplete = " 파일 삭제에 성공하였습니다.";
        public const string conProgressDownload = " 파일을 다운로드중입니다.";
        public const string conProgressDownloadComplete = " 파일 다운로드에 성공하였습니다.";

        public const string conProgressNormalStatus = "진행 현황";
        public const string conProgressSaveStatus = "저장 현황";
        public const string conProgressDeleteStatus = "삭제 현황";
        public const string conProgressDownloadStatus = "다운로드 현황";
    }
}
