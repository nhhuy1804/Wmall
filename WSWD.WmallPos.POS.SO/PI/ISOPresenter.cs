using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSWD.WmallPos.POS.SO.PI
{
    public interface ISOPresenter
    {
        /// <summary>
        /// 
        ///① Config 파일 "AppConfig.ini" 항목 업데이트
        ///- CasNo 항목 Clear
        ///- CasName 항목 Clear
        ///- CasPass 항목 Clear
        ///② SignOff 처리 정보를 전자저널에 남긴다.
        ///- "2015-01-14 11:12:43 [SignOff              ] 계산원 : 9999999 홍길동"
        ///③ 계산원 번호 입력을 대기 한다.,        
        /// </summary>
        void Initialize();

        /// <summary>
        /// Validate CasNo,입력한 계산원번호
        /// </summary>
        /// <returns></returns>
        void ValidateCasNo();

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLoggedIn">사용자 로그인 된상태인지</param>
        /// <returns></returns>
        void ValidateLogin(bool userLoggedIn);
    }
}
