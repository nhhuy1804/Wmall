using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WSWD.WmallPos.FX.Shared.NetComm.Types;

namespace WSWD.WmallPos.FX.Shared.NetComm.Request.PV
{
    /// <summary>
    /// 여전법 추가 0622
    /// IPEK 요청정보
    /// </summary>
    public class PV23ReqData : SerializeClassBase
    {
        public PV23ReqData()
        {
            this.TrxnType = "51";
            this.WorkType = "IK";
            this.PublicKeyType = "A1";
        }

        /// <summary>
        /// 거래구분
        /// 51: IPEK 요청
        /// </summary>
        [TypeGubun(1, TypeProperties.Text, 2)]
        public string TrxnType;

        /// <summary>
        /// 업무구분
        /// IK : IPEK 요청
        /// </summary>
        [TypeGubun(2, TypeProperties.Text, 2)]
        public string WorkType;

        /// <summary>
        /// 단말기 SERIAL
        /// </summary>
        [TypeGubun(3, TypeProperties.Text, 10)]
        public string TmlSerialNo;

        /// <summary>
        /// 공개키 구분
        /// A1 : PDA용 공개키
        /// </summary>
        [TypeGubun(4, TypeProperties.Text, 2)]
        public string PublicKeyType;

        /// <summary>
        /// KSN 정보
        /// </summary>
        [TypeGubun(5, TypeProperties.Text, 10)]
        public string KSN;

        /// <summary>
        /// 암호화 데이터
        /// 단말기는 랜덤값을 생성
        /// SHA256 [ Len(2) + 랜덤값 (16)] = 32 byte
        ///     Len(2) = 0x00 0x10 고정
        ///     Len(2) + 랜덤값 (16) =  18 byte
        /// CAT 공개키로 RSA 
        ///     ( HASH(32)+Len(2)+랜덤값(16))  = 256 byte
        /// </summary>
        [TypeGubun(6, TypeProperties.TextBytes, 256)]
        public string EncData;

        /// <summary>
        /// Filler
        /// </summary>
        [TypeGubun(7, TypeProperties.Text, 10)]
        public string Filler;
    }
}
