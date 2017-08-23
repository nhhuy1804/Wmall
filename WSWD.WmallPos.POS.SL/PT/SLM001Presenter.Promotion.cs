//-----------------------------------------------------------------
/*
 * 화면명   : SLM001Presenter.Promotion.cs
 * 화면설명 : 결제 프로모션 적용
 * 개발자   : 정광호
 * 개발일자 : 2015.06.08
*/
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

using WSWD.WmallPos.FX.Shared.DB;
using WSWD.WmallPos.FX.Shared.NetComm;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.POS.SL.Data;
using WSWD.WmallPos.POS.FX.Win.Utils;
using WSWD.WmallPos.FX.Shared.Utils;
using WSWD.WmallPos.FX.Shared.NetComm.Basket;
using WSWD.WmallPos.FX.BO.Trans;
using WSWD.WmallPos.POS.FX.Win.Devices;
using System.Windows.Forms;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PV;
using WSWD.WmallPos.FX.Shared.NetComm.Types;
using WSWD.WmallPos.FX.Shared.NetComm.Response.PQ;
using WSWD.WmallPos.FX.Shared.NetComm.Request.PQ;
using WSWD.WmallPos.FX.NetComm.Tasks.PQ;

namespace WSWD.WmallPos.POS.SL.PT
{
    partial class SLM001Presenter
    {
        /// <summary>
        /// 포인트 행사
        /// </summary>
        Dictionary<string, object> dicPromoPoint = null;

        /// <summary>
        /// 증정권,응모권,경품 행사
        /// </summary>
        DataTable dtPromoPrint = null;
        DataTable REAL_EVT_ARRAY = null;

        string strGetCD_CLASS = string.Empty;

        void CheckPromotion()
        {
            REAL_EVT_ARRAY = null;
            dtPromoPrint = null;
            dicPromoPoint = null;
            dtPromoPrint = null;
            strGetCD_CLASS = string.Empty;

            Int32 iTARGET_CASH_PAY = 0;                                                     //현금 부분 결제 합
            Int32 iTARGET_CARD_PAY = 0;                                                     //카드 부분 결제 합
            Int32 iTARGET_PAY = 0;                                                          //현금 + 카드 결제 합
            Int32 iTARGET_TOTAL_PAY = TypeHelper.ToInt32(this.BasketSubTtl.AmTotal);        //총결제합
            double iREAL_CASH_PAY = 0;                                                      //프로모션 적용된 현금
            double iREAL_CARD_PAY = 0;                                                      //프로모션 적용된 카드
            double iREAL_APPLY_PAY = 0;                                                     //프로모션 적용된 현금 + 카드

            DataTable ITEM_SALE_ARRAY = null;
            DataRow NewITEM_SALE_ARRAY;
            DataTable EVT_MASTER_ARRAY = null; //PRM010T
            DataTable dt012T = null;    //PRM012T
            DataTable dt011T = null;    //PRM011T

            #region STEP 1 판매 등록된 상품 품번별 판매 합계금액 저장 해준다.

            ITEM_SALE_ARRAY = new DataTable();
            ITEM_SALE_ARRAY.Columns.Add("CD_CLASS");
            ITEM_SALE_ARRAY.Columns.Add("AMT");
            
            foreach (BasketItem item in BasketItems)
            {
                if (item.FgCanc == "0")
                {
                    DataRow[] drFilter = ITEM_SALE_ARRAY.Select(string.Format("CD_CLASS = '{0}'", item.CdClass));

                    if (drFilter != null && drFilter.Length > 0)
                    {
                        drFilter[0]["AMT"] = TypeHelper.ToInt32(drFilter[0]["AMT"]) + TypeHelper.ToInt32(item.AmSale);
                    }
                    else
                    {
                        NewITEM_SALE_ARRAY = ITEM_SALE_ARRAY.NewRow();
                        NewITEM_SALE_ARRAY["CD_CLASS"] = item.CdClass;
                        NewITEM_SALE_ARRAY["AMT"] = item.AmSale;
                        ITEM_SALE_ARRAY.Rows.Add(NewITEM_SALE_ARRAY);

                        if (strGetCD_CLASS.Length > 0)
                        {
                            strGetCD_CLASS += ",";
                        }

                        strGetCD_CLASS += string.Format("'{0}'", item.CdClass);
                    }
                }
            }

            //품번 없을경우  종료
            if (ITEM_SALE_ARRAY == null || ITEM_SALE_ARRAY.Rows.Count <= 0)
            {
                return;
            }

            #endregion

            #region STEP 3 현재 일자에 적용되는 행사 정보가 있는지 확인

            EVT_MASTER_ARRAY = GetPRM010T();

            if (EVT_MASTER_ARRAY == null || EVT_MASTER_ARRAY.Rows.Count <= 0)
            {
                return;
            }

            #endregion

            #region STEP 4 행사 적용 결제 금액을 계산

            foreach (BasketPay bp in this.BasketPays)
            {
                if (bp.PayGrpCd != null && bp.PayGrpCd.Length > 0 && bp.PayDtlCd != null && bp.PayDtlCd.Length > 0)
                {
                    if ((bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //현금
                        (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CASH &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH) || //수표
                        (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_TKCKET &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_TICKET_OTHER)) // 타사상품권
                    {
                        iTARGET_CASH_PAY += TypeHelper.ToInt32(bp.PayAmt) - TypeHelper.ToInt32(bp.BalAmt);
                        iTARGET_PAY += TypeHelper.ToInt32(bp.PayAmt) - TypeHelper.ToInt32(bp.BalAmt);
                    }
                    else if ((bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD) || //신용카드
                        (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_OTHER) || //타건카드
                        (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CARD_WELFARE) || //타건복지
                        (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_CARD &&
                        bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_CASH_IC)) //현금IC
                    {
                        iTARGET_CARD_PAY += TypeHelper.ToInt32(bp.PayAmt) - TypeHelper.ToInt32(bp.BalAmt);
                        iTARGET_PAY += TypeHelper.ToInt32(bp.PayAmt) - TypeHelper.ToInt32(bp.BalAmt);
                    }
                    else if (bp.PayGrpCd == NetCommConstants.PAYMENT_GROUP_SPECIAL && bp.PayDtlCd == NetCommConstants.PAYMENT_DETAIL_ONLINE)
                    {
                        //온라인인 경우 행사적용하지 않는다.
                        return;
                    }
                }
            }

            //현금 또는 카드결제 금액이없을경우 종료
            if (iTARGET_CASH_PAY <= 0 && iTARGET_CARD_PAY <= 0)
            {
                return;
            }

            #endregion

            #region STEP 5 [STEP 3]에서 확보한 행사에서 품번별로 적용 할수 있는 행사를 찾아 1차로 선정 작업 진행

            DataTable APPLY_EVT_ARRAY = new DataTable();
            APPLY_EVT_ARRAY.Columns.Add("CD_STORE");
            APPLY_EVT_ARRAY.Columns.Add("YY_PRM");
            APPLY_EVT_ARRAY.Columns.Add("MM_PRM");
            APPLY_EVT_ARRAY.Columns.Add("WE_PRM");
            APPLY_EVT_ARRAY.Columns.Add("SQ_PRM");
            APPLY_EVT_ARRAY.Columns.Add("CD_PRM");
            APPLY_EVT_ARRAY.Columns.Add("FG_BRAND");
            APPLY_EVT_ARRAY.Columns.Add("CD_CLASS");
            APPLY_EVT_ARRAY.Columns.Add("REAL_CASH_PAY");
            APPLY_EVT_ARRAY.Columns.Add("REAL_CARD_PAY");
            APPLY_EVT_ARRAY.Columns.Add("CD_PRM_TGT");
            DataRow NewAPPLY_EVT_ARRAY;

            foreach (DataRow drEVT_MASTER_ARRAY in EVT_MASTER_ARRAY.Rows)
            {
                #region STEP 5.1 판매등록된 품번의 프로모션 적용 대상 금액 계산 한다.

                foreach (DataRow drITEM_SALE_ARRAY in ITEM_SALE_ARRAY.Rows)
                {
                    //(1) ITEM_SALE_ARRAY.품번코드와 매칭되는 품번이 PRM012T Table에 존재하는지 확인
                    dt012T = GetPRM012T(TypeHelper.ToString(drEVT_MASTER_ARRAY["CD_STORE"]),
                        TypeHelper.ToString(drEVT_MASTER_ARRAY["YY_PRM"]),
                        TypeHelper.ToString(drEVT_MASTER_ARRAY["MM_PRM"]),
                        TypeHelper.ToString(drEVT_MASTER_ARRAY["WE_PRM"]),
                        TypeHelper.ToString(drEVT_MASTER_ARRAY["SQ_PRM"]),
                        TypeHelper.ToString(drITEM_SALE_ARRAY["CD_CLASS"]));

                    if (dt012T != null && dt012T.Rows.Count > 0)
                    {
                        foreach (DataRow dr012T in dt012T.Rows)
                        {
                            //(2) STEP 4에서 계산한 행사 적용 결제 금액을 품번판매 금액에 비율로 계산
                            double iTemp = Math.Ceiling(TypeHelper.ToDouble(iTARGET_PAY) * ((TypeHelper.ToDouble(drITEM_SALE_ARRAY["AMT"]) / TypeHelper.ToDouble(iTARGET_TOTAL_PAY))));

                            //(3) (2)번의 금액에 브랜드별 적용율을 반영하여 계산 한다
                            iREAL_APPLY_PAY = Math.Ceiling(iTemp * (TypeHelper.ToDouble(dr012T["CD_PROC_EVENT"]) / 100));
                            iREAL_CASH_PAY = Math.Ceiling(TypeHelper.ToDouble(iTARGET_CASH_PAY) * (TypeHelper.ToDouble(drITEM_SALE_ARRAY["AMT"]) / TypeHelper.ToDouble(iTARGET_TOTAL_PAY)) * (TypeHelper.ToDouble(dr012T["CD_PROC_EVENT"]) / 100));
                            iREAL_CARD_PAY = iREAL_APPLY_PAY - iREAL_CASH_PAY;

                            string strTemp_01 = TypeHelper.ToString(drEVT_MASTER_ARRAY["CD_PRM"]);
                            string strTemp_02 = TypeHelper.ToString(drEVT_MASTER_ARRAY["FG_BRAND"]) == "0" ? "999999" : TypeHelper.ToString(drITEM_SALE_ARRAY["CD_CLASS"]);
                            DataRow[] drFilter = APPLY_EVT_ARRAY.Select(
                                string.Format("CD_PRM = '{0}' AND CD_CLASS = '{1}'", strTemp_01, strTemp_02));

                            if (drFilter != null && drFilter.Length > 0)
                            {
                                //(4) 행사별 적용 브랜드 저장 ARRAY에 정보 저장 : APPLY_EVT_ARRAY
                                drFilter[0]["REAL_CASH_PAY"] = TypeHelper.ToInt32(drFilter[0]["REAL_CASH_PAY"]) + iREAL_CASH_PAY;
                                drFilter[0]["REAL_CARD_PAY"] = TypeHelper.ToInt32(drFilter[0]["REAL_CARD_PAY"]) + iREAL_CARD_PAY;
                            }
                            else
                            {
                                //(4) 행사별 적용 브랜드 저장 ARRAY에 정보 저장 : APPLY_EVT_ARRAY
                                NewAPPLY_EVT_ARRAY = APPLY_EVT_ARRAY.NewRow();
                                NewAPPLY_EVT_ARRAY["CD_STORE"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["CD_STORE"]);
                                NewAPPLY_EVT_ARRAY["YY_PRM"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["YY_PRM"]);
                                NewAPPLY_EVT_ARRAY["MM_PRM"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["MM_PRM"]);
                                NewAPPLY_EVT_ARRAY["WE_PRM"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["WE_PRM"]);
                                NewAPPLY_EVT_ARRAY["SQ_PRM"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["SQ_PRM"]);
                                NewAPPLY_EVT_ARRAY["CD_PRM"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["CD_PRM"]);
                                NewAPPLY_EVT_ARRAY["FG_BRAND"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["FG_BRAND"]);
                                NewAPPLY_EVT_ARRAY["CD_CLASS"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["FG_BRAND"]) == "0" ? "999999" : TypeHelper.ToString(drITEM_SALE_ARRAY["CD_CLASS"]);
                                NewAPPLY_EVT_ARRAY["REAL_CASH_PAY"] = iREAL_CASH_PAY;
                                NewAPPLY_EVT_ARRAY["REAL_CARD_PAY"] = iREAL_CARD_PAY;
                                NewAPPLY_EVT_ARRAY["CD_PRM_TGT"] = TypeHelper.ToString(drEVT_MASTER_ARRAY["CD_PRM_TGT"]);
                                APPLY_EVT_ARRAY.Rows.Add(NewAPPLY_EVT_ARRAY);
                            }
                        }
                    }
                }

                #endregion
            }

            if (APPLY_EVT_ARRAY == null || APPLY_EVT_ARRAY.Rows.Count <= 0)
            {
                return;
            }

            #endregion

            #region STEP 6 [STEP 5]에서 APPLY_EVT_ARRAY에 저장한 행사의 허들 금액을 확인해서 최종 적용 행사를 결정

            if (REAL_EVT_ARRAY != null)
            {
                REAL_EVT_ARRAY.Clear();
            }
            else
	        {
                REAL_EVT_ARRAY = new DataTable();
                REAL_EVT_ARRAY.Columns.Add("CD_STORE");
                REAL_EVT_ARRAY.Columns.Add("YY_PRM");
                REAL_EVT_ARRAY.Columns.Add("MM_PRM");
                REAL_EVT_ARRAY.Columns.Add("WE_PRM");
                REAL_EVT_ARRAY.Columns.Add("SQ_PRM");
                REAL_EVT_ARRAY.Columns.Add("SQ_PRM_DTL");
                REAL_EVT_ARRAY.Columns.Add("CD_PRM");
                REAL_EVT_ARRAY.Columns.Add("SUM_CASH_PAY");
                REAL_EVT_ARRAY.Columns.Add("SUM_CARD_PAY");
                REAL_EVT_ARRAY.Columns.Add("CD_PRM_TGT");
	        }

            DataRow NewREAL_EVT_ARRAY;

            foreach (DataRow dr in APPLY_EVT_ARRAY.Rows)
            {
                dt011T = GetPRM011T(TypeHelper.ToString(dr["CD_STORE"]),
                    TypeHelper.ToString(dr["YY_PRM"]),
                    TypeHelper.ToString(dr["MM_PRM"]),
                    TypeHelper.ToString(dr["WE_PRM"]),
                    TypeHelper.ToString(dr["SQ_PRM"]),
                    TypeHelper.ToString(dr["CD_CLASS"]),
                    TypeHelper.ToInt32(dr["REAL_CASH_PAY"]) + TypeHelper.ToInt32(dr["REAL_CARD_PAY"]));

                if (dt011T != null && dt011T.Rows.Count > 0)
                {
                    foreach (DataRow dr011T in dt011T.Rows)
                    {
                        NewREAL_EVT_ARRAY = REAL_EVT_ARRAY.NewRow();
                        NewREAL_EVT_ARRAY["CD_STORE"] = TypeHelper.ToString(dr["CD_STORE"]);
                        NewREAL_EVT_ARRAY["YY_PRM"] = TypeHelper.ToString(dr["YY_PRM"]);
                        NewREAL_EVT_ARRAY["MM_PRM"] = TypeHelper.ToString(dr["MM_PRM"]);
                        NewREAL_EVT_ARRAY["WE_PRM"] = TypeHelper.ToString(dr["WE_PRM"]);
                        NewREAL_EVT_ARRAY["SQ_PRM"] = TypeHelper.ToString(dr["SQ_PRM"]);
                        NewREAL_EVT_ARRAY["SQ_PRM_DTL"] = TypeHelper.ToString(dr011T["SQ_PRM_DTL"]);
                        NewREAL_EVT_ARRAY["CD_PRM"] = TypeHelper.ToString(dr["CD_PRM"]);
                        NewREAL_EVT_ARRAY["SUM_CASH_PAY"] = TypeHelper.ToInt32(dr["REAL_CASH_PAY"]);
                        NewREAL_EVT_ARRAY["SUM_CARD_PAY"] = TypeHelper.ToInt32(dr["REAL_CARD_PAY"]);
                        NewREAL_EVT_ARRAY["CD_PRM_TGT"] = TypeHelper.ToInt32(dr["CD_PRM_TGT"]);
                        REAL_EVT_ARRAY.Rows.Add(NewREAL_EVT_ARRAY);
                    }
                }
            }

            if (REAL_EVT_ARRAY == null || REAL_EVT_ARRAY.Rows.Count <= 0)
            {
                return;
            }

            #endregion

            #region STEP 7 행사 적용

            #region STEP 7.1 포인트 적립 전문 만들때 포인트 관련 행사를 먼저 반영

            DataRow[] drPoint = REAL_EVT_ARRAY.Select("CD_PRM = '01'");

            if (drPoint != null && drPoint.Length > 0)
            {
                string strPointEventInfo = string.Empty;
                string strEventPayCashAmt = string.Empty;
                string strEventPayCardAmt = string.Empty;
                string strEventPayEtcAmt = string.Empty;

                foreach (DataRow drTemp in drPoint)
                {
                    if (TypeHelper.ToString(drTemp["CD_STORE"]).Length > 0)
                    {
                        strPointEventInfo = TypeHelper.ToString(drTemp["CD_STORE"]).PadRight(4, ' ') + TypeHelper.ToString(drTemp["YY_PRM"]) + TypeHelper.ToString(drTemp["MM_PRM"]) + TypeHelper.ToString(drTemp["WE_PRM"]) + TypeHelper.ToString(drTemp["SQ_PRM"]) + TypeHelper.ToString(drTemp["SQ_PRM_DTL"]);
                        strEventPayCashAmt = (TypeHelper.ToInt32(drTemp["SUM_CASH_PAY"])).ToString();
                        strEventPayCardAmt = (TypeHelper.ToInt32(drTemp["SUM_CARD_PAY"])).ToString();
                    }
                }

                dicPromoPoint = new Dictionary<string, object>();
                dicPromoPoint.Add("PointEventInfo", strPointEventInfo);
                dicPromoPoint.Add("EventPayCashAmt", strEventPayCashAmt);
                dicPromoPoint.Add("EventPayCardAmt", strEventPayCardAmt);
                dicPromoPoint.Add("EventPayEtcAmt", strEventPayEtcAmt);
            }

            #endregion

            #endregion
        }

        private int m_promotionCount = 0;

        /*
        //01 포인트

        //02 사은품

        //03 카드

        //04 입점업체

        //05 증정권

        //06 응모권

        //07 경품


        //STEP 1. 상품이 판매 등록 될때 마다 품번별 판매 합계금액을 별도로 저장 해준다. 
        //   - 에누리 금액은 제외 해서 계산
        //   - 품번코드, 판매금액, 행사적용금액(0)  :ITEM_SALE_ARRAY

        //STEP 2. 판매가 완료되면 포인트 적립 업무 들어가기 전에 항상 확인 한다.

        //STEP 3. 현재 일자에 적용되는 행사 정보가 있는지 확인 한다.
     
        //      STEP 3.1 쿼리
        //        SELECT cd_store, yy_prm, mm_prm, we_prm, sq_prm, cd_prm, fg_brand
        //          FROM prm010t
        //         WHERE cd_store =  :점포코드
        //           AND DD_START <= :현재일자
        //           AND DD_END   >= :현재일자;               
           
        //      STEP 3.2 쿼리 결과 확인하여 행사 정보 배열로 저장 : EVT_MASTER_ARRAY
      
        //        if 행사 정보 없으면 then 
        //           End

        //        else 행사 정보 있으면 then
        //           - 1 Row씩 행사 정보 배열(EVT_MASTER_ARRAY)에 저징
        //           - 배열에 저장되는 값
        //             cd_store, yy_prm, mm_prm, we_prm, sq_prm, cd_prm, fg_brand            
        //        end if

        //STEP 4. 행사 적용 결제 금액을 계산한다.
      
        //        - 상품교환권, 포인트지불, 할인결제, 할인쿠폰을 제외한 결제금액 합을 구한다.
        
        //          (1) 현금 부분 결제 합 : TARGET_CASH_PAY
        //          (2) 카드 부분 결제 합 : TARGET_CARD_PAY          

             
        //STEP 5, STEP 3에서 확보한 행사에서 품번별로 적용 할수 있는 행사를 찾아 1차로 선정 작업 진행

        //      WHILE (EVT_MASTER_ARRAY 배열 수 만큼:idx_a)
        //      {
      
        //          STEP 5.1 판매등록된 품번의 프로모션 적용 대상 금액 게산 한다.
          
        //                 WHILE (ITEM_SALE_ARRAY의 품번 수 만큼:idx_b)
        //                 {
        //                     (1) ITEM_SALE_ARRAY.품번코드와 매칭되는 품번이 PRM012T Table에 존재하는지 확인
                         
        //                         - 쿼리
        //                         SELECT cd_class, cd_proc_event 
        //                           FROM PRM012T
        //                          WHERE cd_store = :EVT_MASTER_ARRAY(idx_a).cd_store
        //                            AND yy_prm   = :EVT_MASTER_ARRAY(idx_a).yy_prm   
        //                            AND mm_prm   = :EVT_MASTER_ARRAY(idx_a).mm_prm   
        //                            AND we_prm   = :EVT_MASTER_ARRAY(idx_a).we_prm   
        //                            AND sq_prm   = :EVT_MASTER_ARRAY(idx_a).sq_prm   
        //                            AND cd_class = :ITEM_SALE_ARRAY(idx_b).품번코드;
                        
        //                         if 매칭되는 품번이 아니면 then                             
        //                            continue
        //                         end if
                         
        //                     (2) STEP 4에서 계산한 행사 적용 결제 금액을 품번판매 금액에 비율로 계산
                         
        //                         - TEMP_APPLY_PAY = (TARGET_CASH_PAY + TARGET_CARD_PAY) * (ITEM_SALE_ARRAY(idx_b).판매금액 / 소계금액) * 100 
                      
        //                     (3) (2)번의 금액에 브랜드별 적용율을 반영하여 계산 한다.
                     
        //                         - if cd_proc_event = 0 then
        //                              REAL_APPLY_PAY = TEMP_APPLY_PAY X (100/100)
    	  		              
        //                           else if CD_PROC_EVENT = 1 then
        //                              REAL_APPLY_PAY = TEMP_APPLY_PAY X (50/100)
                              
        //                           else if CD_PROC_EVENT = 2 then
        //                              REAL_APPLY_PAY = TEMP_APPLY_PAY X (20/100)
        //                           end if    
                           
        //                     (4) 행사별 적용 브랜드 저장 ARRAY에 정보 저장 : APPLY_EVT_ARRAY
                     
        //                       * APPLY_EVT_ARRAY 배열에 저장되는 값
        //                         [cd_store(KEY), yy_prm(KEY), mm_prm(KEY), we_prm(KEY), sq_prm(KEY), 품번코드(KEY), cd_prm, fg_brand, 현금부분행사적용금액, 카드부분행사적용금액]
                         
        //                       (4-1) 현금부분행사적용금액(REAL_CASH_PAY) = TARGET_CASH_PAY * (ITEM_SALE_ARRAY(idx_b).판매금액 / 소계금액) * 100 
                       
        //                       (4-2) 카드부분행사적용금액(REAL_CARD_PAY) = REAL_APPLY_PAY - REAL_CASH_PAY                       
                         
        //                       (4-3) 행사 브랜드 적용 방식에 따라 ARRAY에 정보 저장
                         
        //                         if EVT_MASTER_ARRAY(idx_a).fg_brand = '브랜드합산' then
        //                            - 품번코드 '999999'로 ARRAY key를 기준으로 합산하여 APPLY_EVT_ARRAY에 저장
        //                              cd_store, yy_prm, mm_prm, we_prm, sq_prm, cd_prm, fg_brand, '999999', REAL_CASH_PAY, REAL_CARD_PAY
                         
        //                         else EVT_MASTER_ARRAY(idx_a).fg_brand = '단일브랜드' then
        //                            - 품번코드로 ARRAY key를 기준으로 합산하여 APPLY_EVT_ARRAY에 저장
        //                              cd_store, yy_prm, mm_prm, we_prm, sq_prm, cd_prm, fg_brand, ITEM_SALE_ARRAY(idx_b).품번코드, REAL_CASH_PAY, REAL_CARD_PAY
        //                         end if                                                  
        //                 } // WHILE (ITEM_SALE_ARRAY의 품번 수 만큼:idx_b)
               
        //      } //  WHILE (EVT_MASTER_ARRAY 배열 수 만큼:idx_a)


        //STEP 6, STEP 5에서 APPLY_EVT_ARRAY에 저장한 행사의 허들 금액을 확인해서 최종 적용 행사를 결정 한다,

        //      WHILE (APPLY_EVT_ARRAY 배열 수 만큼:idx_a)
        //      {
        //          (1) 행사의 허들 금액 확인
        //              select sq_prm_dtl  
        //                from prm011t
        //               where cd_store = APPLY_EVT_ARRAY(idx_a).cd_store   
        //                 and yy_prm   = APPLY_EVT_ARRAY(idx_a).yy_prm
        //                 and mm_prm   = APPLY_EVT_ARRAY(idx_a).mm_prm
        //                 and we_prm   = APPLY_EVT_ARRAY(idx_a).we_prm 
        //                 and sq_prm   = APPLY_EVT_ARRAY(idx_a).sq_prm
        //                 and am_min  <= (APPLY_EVT_ARRAY(idx_a).REAL_CASH_PAY + APPLY_EVT_ARRAY(idx_a).REAL_CARD_PAY)
        //                 and am_max  >= (APPLY_EVT_ARRAY(idx_a).REAL_CASH_PAY + APPLY_EVT_ARRAY(idx_a).REAL_CARD_PAY);
             
        //             if 조건에 맞는 허들금액이 존재하면
        //                - 행사 적용 최종 ARRAY에 적용할 행사 저장 : REAL_EVT_ARRAY
        //                  * REAL_EVT_ARRAY 배열에 저장되는 값
        //                    [cd_store(KEY), yy_prm(KEY), mm_prm(KEY), we_prm(KEY), sq_prm(KEY), sq_prm_dtl(KEY), cd_prm, 현금부분행사적용금액(SUM), 카드부분행사적용금액(SUM)]
        //             end if                    
                
        //      }
      
        //STEP 7. 행사를 적용 한다.

        //      STEP 7.1 포인트 적립 전문 만들때 포인트 관련 행사를 먼저 반영 한다.
      
        //             - REAL_EVT_ARRAY에서 cd_prm = '포인트행사'가 있으면 포인트 적립 요청시 포인트 적립전문에 행사코드와 행사 적용금액 반영
        //               REAL_EVT_ARRAY.cd_store + REAL_EVT_ARRAY.yy_prm + REAL_EVT_ARRAY.mm_prm + REAL_EVT_ARRAY.we_prm +  REAL_EVT_ARRAY.sq_prm + sq_prm_dtl(KEY) + 현금부분행사적용금액 + 카드부분행사적용금액


        //     STEP 7.2 판매 영수증 출력후 행사 정보를 확인하여 행사 반영 영수증을 출력 한다.
     
        //            while(REAL_EVT_ARRAY 배열 수 만큼:idx_a)
        //            {
        //                case REAL_EVT_ARRAY(idx_a).sq_prm = '증정권행사'
        //                    증정권 행사 문구 출력
                    
        //                case REAL_EVT_ARRAY(idx_a).sq_prm = '응모권행사'
        //                    응모권행사 문구 출력
                    
        //                case REAL_EVT_ARRAY(idx_a).sq_prm = '경품행사'
        //                    경품행사 문구 출력
        //            }

        //            [ 증정권 행사 문구 출력인 경우]
        //              Step 1 증정권 행사 마스터에서 제한시간 확인
        //                     현재시간 >= prm016t.AM_TIME_LIMIT_FR and 현제시간 <= AM_TIME_LIMIT_to
        //              Step 2 증정권 발행 가능 여부 본사 서버와 통신하여 prm023t의 행사글 출력
              
        //                   if 발행 불가 then 
        //                      end;
        //                   else 발행 가능 then
        //                      prm023t의 행사글 출력
        //                   endif 
              
        //           [ 응모권 행사 문구 출력인 경우]
        //             prm023t 의 응모권 행사글 출력
           
        //           [ 경품 행사 문구 출력]
        //             Step 1 경품 행사 마스터에서 제한시간 확인
        //                    현재시간 >= prm016t.AM_TIME_LIMIT_FR and 현재시간 <= AM_TIME_LIMIT_to
        //             Step 2 판매 거래번호가 prm016t의 NO_WINING1, NO_WINING2, NO_WINING3, NO_WINING4, NO_WINING5와 일치 하는지 확인
        //             Step 3 경품 행사 발행 가능 여부 본사 서버와 통신하여 prm023t의 행사 행사글 출력
        //                    if 발행 불가 then 
        //                       end;
        //                    else 발행 가능 then
        //                       prm023t의 행사글 출력
        //                    endif 
        */
        /// <summary>
        /// 포인트 적립후 행상 프로모션 출력 확인
        /// </summary>
        bool CheckPromotionPrint()
        {
            // 변수초기화
            bool hasPromotionCount = false;
            m_promotionCount = 0;

            #region STEP 7.2 판매 영수증 출력후 행사 정보를 확인하여 행사 반영 영수증을 출력

            dtPromoPrint = new DataTable();
            dtPromoPrint.Columns.Add("CD_STORE");
            dtPromoPrint.Columns.Add("YY_PRM");
            dtPromoPrint.Columns.Add("MM_PRM");
            dtPromoPrint.Columns.Add("WE_PRM");
            dtPromoPrint.Columns.Add("SQ_PRM");
            dtPromoPrint.Columns.Add("SQ_PRM_DTL");
            dtPromoPrint.Columns.Add("SQ_LOC");
            dtPromoPrint.Columns.Add("FG_TEXT");
            dtPromoPrint.Columns.Add("FG_ALIGN");
            dtPromoPrint.Columns.Add("NM_DESC");
            dtPromoPrint.Columns.Add("FG_SIZ");
            dtPromoPrint.Columns.Add("CD_PRM");
            dtPromoPrint.Columns.Add("CD_PRM_TGT");

            if (REAL_EVT_ARRAY != null)
            {
                foreach (DataRow drREAL_EVT_ARRAY in REAL_EVT_ARRAY.Rows)
                {
                    if (TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]) == "05")
                    {
                        #region 증정권

                        //Step 1 증정권 행사 마스터에서 제한시간 확인
                        //       현재시간 >= prm015t.AM_TIME_LIMIT_FR and 현제시간 <= AM_TIME_LIMIT_to
                        //Step 2 증정권 발행 가능 여부 본사 서버와 통신하여 prm023t의 행사글 출력
                        DataTable dt = GetPRM015T(TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]));

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            PQ13ReqData reqData = new PQ13ReqData();
                            reqData.CD_STORE = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]);
                            reqData.YY_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]);
                            reqData.MM_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]);
                            reqData.WE_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]);
                            reqData.SQ_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]);
                            reqData.SQ_PRM_DTL = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]);
                            reqData.CD_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]);
                            reqData.CD_PRM_TGT = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM_TGT"]);
                            reqData.NO_POINT_MEMBER = this.BasketPointSave.NoPointMember;

                            // 통신건수 생긴다
                            hasPromotionCount = true;
                            m_promotionCount++;

                            var pq13 = new PQ13DataTask(reqData);
                            pq13.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq13_TaskCompleted);
                            pq13.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq13_Errored);
                            pq13.ExecuteTask();
                        }

                        #endregion
                    }
                    else if (TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]) == "06")
                    {
                        // 응모권
                        // SetPrint(
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]),
                        //    TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM_TGT"]));

                        PQ13ReqData reqData = new PQ13ReqData();
                        reqData.CD_STORE = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]);
                        reqData.YY_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]);
                        reqData.MM_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]);
                        reqData.WE_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]);
                        reqData.SQ_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]);
                        reqData.SQ_PRM_DTL = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]);
                        reqData.CD_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]);
                        reqData.CD_PRM_TGT = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM_TGT"]);
                        reqData.NO_POINT_MEMBER = this.BasketPointSave.NoPointMember;

                        // 통신건수 생긴다
                        hasPromotionCount = true;
                        m_promotionCount++;

                        var pq13 = new PQ13DataTask(reqData);
                        pq13.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq13_TaskCompleted);
                        pq13.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq13_Errored);
                        pq13.ExecuteTask();
                    }
                    else if (TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]) == "07")
                    {
                        #region 경품

                        //Step 1 경품 행사 마스터에서 제한시간 확인
                        //       현재시간 >= prm016t.AM_TIME_LIMIT_FR and 현재시간 <= AM_TIME_LIMIT_to
                        //Step 2 판매 거래번호가 prm016t의 NO_WINING1, NO_WINING2, NO_WINING3, NO_WINING4, NO_WINING5와 일치 하는지 확인
                        //Step 3 경품 행사 발행 가능 여부 본사 서버와 통신하여 prm023t의 행사 행사글 출력
                        DataTable dt = GetPRM016T(TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]),
                            TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]),
                            TypeHelper.ToString(TypeHelper.ToInt32(this.BasketHeader.TrxnNo)));

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            PQ13ReqData reqData = new PQ13ReqData();
                            reqData.CD_STORE = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_STORE"]);
                            reqData.YY_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["YY_PRM"]);
                            reqData.MM_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["MM_PRM"]);
                            reqData.WE_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["WE_PRM"]);
                            reqData.SQ_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM"]);
                            reqData.SQ_PRM_DTL = TypeHelper.ToString(drREAL_EVT_ARRAY["SQ_PRM_DTL"]);
                            reqData.CD_PRM = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM"]);
                            reqData.CD_PRM_TGT = TypeHelper.ToString(drREAL_EVT_ARRAY["CD_PRM_TGT"]);
                            reqData.NO_POINT_MEMBER = this.BasketPointSave.NoPointMember;

                            // 통신건수 생긴다
                            hasPromotionCount = true;
                            m_promotionCount++;

                            var pq13 = new PQ13DataTask(reqData);
                            pq13.TaskCompleted += new WSWD.WmallPos.FX.NetComm.Tasks.TaskCompletedHandler(pq13_TaskCompleted);
                            pq13.Errored += new WSWD.WmallPos.FX.NetComm.Client.ErrorHandler(pq13_Errored);
                            pq13.ExecuteTask();
                        }

                        #endregion
                    }
                }
            }

            #endregion

            return hasPromotionCount;
        }

        #region 전문조회

        /// <summary>
        /// 경품권,응모권 출력 마감 여부 확인
        /// </summary>
        /// <param name="responseData"></param>
        void pq13_TaskCompleted(WSWD.WmallPos.FX.Shared.NetComm.Response.TaskResponseData responseData)
        {
            m_promotionCount--;
            if (responseData.Response.ResponseState == SocketTrxnResType.Success)
            {
                var data = responseData.DataRecords.ToDataRecords<PQ13RespData>();
                if (data.Length > 0)
                {
                    if (data[0].CLOSE_YN == "0")
                    {
                        SetPrint(data[0].CD_STORE, data[0].YY_PRM, data[0].MM_PRM, data[0].WE_PRM, data[0].SQ_PRM, data[0].SQ_PRM_DTL, data[0].CD_PRM, data[0].CD_PRM_TGT);
                    }
                }
            }

            //else if (responseData.Response.ResponseState == WSWD.WmallPos.FX.Shared.NetComm.SocketTrxnResType.NoInfo)
            //{
            //    //txtCardNo.Text = "";
            //    //msgBar.Text = strMsg02;
            //}
            //else
            //{
            //    //msgBar.Text = responseData.Response.ErrorMessage.ToString();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CD_STORE"></param>
        /// <param name="YY_PRM"></param>
        /// <param name="MM_PRM"></param>
        /// <param name="WE_PRM"></param>
        /// <param name="SQ_PRM"></param>
        /// <param name="SQ_PRM_DTL"></param>
        /// <param name="CD_PRM"></param>
        /// <param name="CD_PRM_TGT"></param>
        private void SetPrint(string CD_STORE, string YY_PRM, string MM_PRM, string WE_PRM, string SQ_PRM, string SQ_PRM_DTL, string CD_PRM, string CD_PRM_TGT)
        {
            DataTable dt = GetPRM023T(CD_STORE, YY_PRM, MM_PRM, WE_PRM, SQ_PRM);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow NewDr;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow[] drFilter = dtPromoPrint.Select(string.Format("CD_STORE = '{0}' AND YY_PRM = '{1}' AND MM_PRM = '{2}' AND WE_PRM = '{3}' AND SQ_PRM = '{4}' AND SQ_PRM_DTL = '{5}' AND SQ_LOC = '{6}'", 
                        CD_STORE, YY_PRM, MM_PRM, WE_PRM, SQ_PRM, SQ_PRM_DTL, TypeHelper.ToString(dr["SQ_LOC"])));

                    if (drFilter == null || drFilter.Length <= 0)
                    {
                        NewDr = dtPromoPrint.NewRow();
                        NewDr["CD_STORE"] = CD_STORE;
                        NewDr["YY_PRM"] = YY_PRM;
                        NewDr["MM_PRM"] = MM_PRM;
                        NewDr["WE_PRM"] = WE_PRM;
                        NewDr["SQ_PRM"] = SQ_PRM;
                        NewDr["SQ_PRM_DTL"] = SQ_PRM_DTL;
                        NewDr["SQ_LOC"] = TypeHelper.ToString(dr["SQ_LOC"]);
                        NewDr["FG_TEXT"] = TypeHelper.ToString(dr["FG_TEXT"]);
                        NewDr["FG_ALIGN"] = TypeHelper.ToString(dr["FG_ALIGN"]);
                        NewDr["NM_DESC"] = TypeHelper.ToString(dr["NM_DESC"]).Trim();
                        NewDr["FG_SIZ"] = TypeHelper.ToString(dr["FG_SIZ"]);
                        NewDr["CD_PRM"] = CD_PRM;
                        NewDr["CD_PRM_TGT"] = CD_PRM_TGT;
                        dtPromoPrint.Rows.Add(NewDr);    
                    }
                }
            }
        }

        /// <summary>
        /// 경품권,응모권 출력 마감 여부 확인 에러
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="lastException"></param>
        void pq13_Errored(string errorMessage, Exception lastException)
        {
            m_promotionCount--;
            // msgBar.Text = errorMessage.ToString();
        }

        #endregion

        #region DB 조회

        /// <summary>
        /// 행사 출력 날짜 확인
        /// </summary>
        /// <returns></returns>
        DataTable GetPRM010T()
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM010T"),
                    new string[] { "@CD_STORE", "@DD_SALE" },
                    new object[] { ConfigData.Current.AppConfig.PosInfo.StoreNo, ConfigData.Current.AppConfig.PosInfo.SaleDate });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 행사 출력 품번 확인
        /// </summary>
        /// <param name="strCD_STORE"></param>
        /// <param name="strYY_PRM"></param>
        /// <param name="strMM_PRM"></param>
        /// <param name="strWE_PRM"></param>
        /// <param name="strSQ_PRM"></param>
        /// <param name="strCD_CLASS"></param>
        /// <returns></returns>
        DataTable GetPRM012T(string strCD_STORE, string strYY_PRM, string strMM_PRM, string strWE_PRM, string strSQ_PRM, string strCD_CLASS)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM012T"),
                    new string[] { "@CD_STORE", "@YY_PRM", "@MM_PRM", "@WE_PRM", "@SQ_PRM", "@CD_CLASS" },
                    new object[] { strCD_STORE, strYY_PRM, strMM_PRM, strWE_PRM, strSQ_PRM, strCD_CLASS });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 행사 출력 금액 확인
        /// </summary>
        /// <param name="strCD_STORE"></param>
        /// <param name="strYY_PRM"></param>
        /// <param name="strMM_PRM"></param>
        /// <param name="strWE_PRM"></param>
        /// <param name="strSQ_PRM"></param>
        /// <param name="strCD_CLASS"></param>
        /// <param name="iAM_ITEM"></param>
        /// <returns></returns>
        DataTable GetPRM011T(string strCD_STORE, string strYY_PRM, string strMM_PRM, string strWE_PRM, string strSQ_PRM, string strCD_CLASS, Int32 iAM_ITEM)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM011T"),
                    new string[] { "@CD_STORE", "@YY_PRM", "@MM_PRM", "@WE_PRM", "@SQ_PRM", "@CD_CLASS", "@AM_ITEM" },
                    new object[] { strCD_STORE, strYY_PRM, strMM_PRM, strWE_PRM, strSQ_PRM, strCD_CLASS, iAM_ITEM });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 행사 출력(증정권) 가능 여부 확인 조회
        /// </summary>
        /// <param name="strCD_STORE"></param>
        /// <param name="strYY_PRM"></param>
        /// <param name="strMM_PRM"></param>
        /// <param name="strWE_PRM"></param>
        /// <param name="strSQ_PRM"></param>
        /// <param name="strSQ_PRM_DTL"></param>
        /// <param name="strTRXN_NO"></param>
        /// <returns></returns>
        DataTable GetPRM015T(string strCD_STORE, string strYY_PRM, string strMM_PRM, string strWE_PRM, string strSQ_PRM, string strSQ_PRM_DTL)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM015T"),
                    new string[] { "@CD_STORE", "@YY_PRM", "@MM_PRM", "@WE_PRM", "@SQ_PRM", "@SQ_PRM_DTL", "@AM_TIME_LIMIT" },
                    new object[] { strCD_STORE, strYY_PRM, strMM_PRM, strWE_PRM, strSQ_PRM, strSQ_PRM_DTL, DateTimeUtils.Get24TimeString(DateTime.Now).Replace(":", "") });

            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 행사 출력(경품) 가능 여부 확인 조회
        /// </summary>
        /// <param name="strCD_STORE"></param>
        /// <param name="strYY_PRM"></param>
        /// <param name="strMM_PRM"></param>
        /// <param name="strWE_PRM"></param>
        /// <param name="strSQ_PRM"></param>
        /// <param name="strSQ_PRM_DTL"></param>
        /// <param name="strTRXN_NO"></param>
        /// <returns></returns>
        DataTable GetPRM016T(string strCD_STORE, string strYY_PRM, string strMM_PRM, string strWE_PRM, string strSQ_PRM, string strSQ_PRM_DTL, string strTRXN_NO)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM016T"),
                    new string[] { "@CD_STORE", "@YY_PRM", "@MM_PRM", "@WE_PRM", "@SQ_PRM", "@SQ_PRM_DTL", "@AM_TIME_LIMIT", "@TRXN_NO" },
                    new object[] { strCD_STORE, strYY_PRM, strMM_PRM, strWE_PRM, strSQ_PRM, strSQ_PRM_DTL, DateTimeUtils.GetTimeString(DateTime.Now).Replace(":", ""), strTRXN_NO });

            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 행사 출력 문구 조회
        /// </summary>
        /// <param name="strCD_STORE"></param>
        /// <param name="strYY_PRM"></param>
        /// <param name="strMM_PRM"></param>
        /// <param name="strWE_PRM"></param>
        /// <param name="strSQ_PRM"></param>
        /// <returns></returns>
        DataTable GetPRM023T(string strCD_STORE, string strYY_PRM, string strMM_PRM, string strWE_PRM, string strSQ_PRM)
        {
            DataSet ds = new DataSet();
            var masterdb = MasterDbHelper.InitInstance();
            try
            {
                ds = masterdb.ExecuteQuery(Extensions.LoadSqlCommand("POS_SL", "M001GetPRM023T"),
                    new string[] { "@CD_STORE", "@YY_PRM", "@MM_PRM", "@WE_PRM", "@SQ_PRM" },
                    new object[] { strCD_STORE, strYY_PRM, strMM_PRM, strWE_PRM, strSQ_PRM });
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogException(ex);
            }
            finally
            {
                masterdb.EndInstance();
            }

            return ds.Tables[0];
        }

        #endregion
    }
}
