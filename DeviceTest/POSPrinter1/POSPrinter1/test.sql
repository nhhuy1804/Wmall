﻿<BSM061T_CDCLASS_Exists>
SELECT * FROM BSM061T WHERE CD_CLASS = @CD_CLASS LIMIT 1;
</BSM061T_CDCLASS_Exists>
<BSM100T_CD_Exists>
SELECT * FROM BSM100T WHERE CD_CTGY = @CD_CTGY LIMIT 1;
</BSM100T_CD_Exists>

<BSM061T_CDCLASS_FG_Exists>
SELECT COUNT(*) FROM BSM061T WHERE CD_CLASS = @CD_CLASS AND FG_CLASS = @FG_CLASS LIMIT 1;
</BSM061T_CDCLASS_FG_Exists>

-- 단품마스터검색
<BSM079T_CDITEM_Exists>
SELECT 	CD_ITEM, NM_ITEM, CD_CLASS, NM_CLASS, FG_TAX, UT_CPRC, UT_SPRC, 
		FG_USE, CD_DP, FG_POINT, AM_APNT, TT_IUDT
FROM	BSM079T
WHERE	FG_USE = '1' AND CD_ITEM = @CD_ITEM LIMIT 1;

</BSM079T_CDITEM_Exists>

<M001TouchGroupSelect>
SELECT CD_GROP, NM_GROP 
FROM BSM043T 
WHERE CD_STORE = @CD_STORE AND NO_POS = @NO_POS;
</M001TouchGroupSelect>

-- TOUCH GROUP SELECTED
<M001TouchItemsByGroupSelect>
SELECT	CD_GROP, SQ_SORT, CD_ITEM, CD_DP, NM_ITEM, UT_SPRC
FROM	BSM044T 
WHERE	CD_STORE = @CD_STORE 
		AND NO_POS = @NO_POS 
		AND CD_GROP = @CD_GROP
		AND (CASE WHEN @CD_DP = '' THEN CD_DP <> '4'
				ELSE CD_DP = @CD_DP END);
</M001TouchItemsByGroupSelect>

<M001PRESETItemCheck>
SELECT	CD_ITEM, CD_DP, UT_SPRC
FROM	BSM045T
WHERE	CD_STORE = @CD_STORE 
		AND NO_POS = @NO_POS
		AND NO_KEY = @NO_KEY
LIMIT 1;	
</M001PRESETItemCheck>

<M001HoldGetMaxNo>
SELECT MAX(NO_BORU)
FROM SAT900T
WHERE DD_SALE = @DD_SALE
		AND CD_STORE = @CD_STORE
		AND NO_POS = @NO_POS
</M001HoldGetMaxNo>

<M001HoldInsert>
INSERT INTO SAT900T(DD_SALE, CD_STORE, NO_POS, NO_BORU, 
		SQ_BORU, VC_CONT, ID_USER, FG_CANCEL, DD_TIME, AM_SALE)
VALUES(@DD_SALE, @CD_STORE, @NO_POS, @NO_BORU, 
		@SQ_BORU, @VC_CONT, @ID_USER, @FG_CANCEL, @DD_TIME, @AM_SALE)
</M001HoldInsert>

<P003HoldList>
SELECT NO_BORU, DD_TIME, cast(AM_SALE as varchar) AM_SALE
FROM SAT900T
WHERE FG_CANCEL = 'N' AND DD_SALE = @DD_SALE 
		AND CD_STORE = @CD_STORE
		AND NO_POS = @NO_POS 
		AND SQ_BORU = 1
ORDER BY NO_BORU
</P003HoldList>

<P003HoldCount>
SELECT COUNT(DISTINCT NO_BORU)
FROM SAT900T
WHERE FG_CANCEL = 'N' AND DD_SALE = @DD_SALE AND CD_STORE = @CD_STORE
		AND NO_POS = @NO_POS
		AND (CASE WHEN @NO_BORU = '' THEN 1=1
			ELSE NO_BORU = @NO_BORU END)
GROUP BY CD_STORE, NO_POS, DD_SALE;
</P003HoldCount>

<P003HoldSelectTop>
SELECT NO_BORU, SQ_BORU, VC_CONT, DD_TIME, AM_SALE
FROM SAT900T
WHERE FG_CANCEL = 'N' 
		AND DD_SALE = @DD_SALE 
		AND CD_STORE = @CD_STORE
		AND NO_POS = @NO_POS
		AND (@NO_BORU = '' OR NO_BORU = @NO_BORU)
ORDER BY SQ_BORU;
</P003HoldSelectTop>

<P003HoldTrxnRelease>
UPDATE SAT900T
SET FG_CANCEL = 'Y'
WHERE FG_CANCEL = 'N'
		AND DD_SALE = @DD_SALE
		AND CD_STORE = @CD_STORE
		AND NO_POS = @NO_POS
		AND NO_BORU = @NO_BORU;
</P003HoldTrxnRelease>

<SAT300TSelectItem>
SELECT AM_ITEM, CT_ITEM
FROM SAT301T
WHERE	DD_SALE = @DD_SALE AND
		CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND
		ID_ITEM = @ID_ITEM
LIMIT 1;		
</SAT300TSelectItem>

-- SAT300TInserUpdate 항목
<SAT300TInserUpdate>
-- SAT300TInserUpdate 항목
INSERT OR REPLACE INTO SAT300T
(
	DD_SALE, 
	CD_STORE, 
	NO_POS, 
	ID_ITEM, 
	AM_ITEM, 
	CT_ITEM)
VALUES (
	@DD_SALE, 
	@CD_STORE, 
	@NO_POS, 
	@ID_ITEM, 
	@AM_ITEM, 
	@CT_ITEM);
</SAT300TInserUpdate>

-- SAT300TInserUpdate 항목
<SAT301TInserUpdate>
INSERT OR REPLACE INTO SAT301T
(
	DD_SALE, 
	CD_STORE, 
	NO_POS, 
	ID_USER, 
	ID_ITEM, 
	AM_ITEM, 
	CT_ITEM)
VALUES (
	@DD_SALE, 
	@CD_STORE, 
	@NO_POS, 
	@ID_USER, 
	@ID_ITEM, 
	@AM_ITEM, 
	@CT_ITEM);
</SAT301TInserUpdate>

<SAT301TSelectItem>
SELECT AM_ITEM, CT_ITEM
FROM SAT301T
WHERE	DD_SALE = @DD_SALE AND
		CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND
		ID_USER = @ID_USER AND
		ID_ITEM = @ID_ITEM
LIMIT 1;		
</SAT301TSelectItem>

<P004GetNoticeBSM130T>
SELECT		SUBSTR(A.DD_START,1,4) || '-' || SUBSTR(A.DD_START,5,2) || '-' || SUBSTR(A.DD_START,7,2) AS DD_START
,			A.NO_SEQ
,			A.DD_END
,			A.NM_TITLE
,			A.NM_DESC
,			A.FG_URGT
,			A.NO_SEND_USER
,			A.NM_SEND_USER
,			CASE WHEN IFNULL(B.ID_USER, '') = @ID_USER THEN 'Y' ELSE 'N' END AS FLAG_YN 
FROM		BSM130T	A
LEFT JOIN	BSM131T	B	ON	A.DD_START = B.DD_START AND A.NO_SEQ = B.NO_SEQ
WHERE		IFNULL(A.DD_START,STRFTIME('%Y%m%d', 'NOW'))	<= @DD_SALE
AND			IFNULL(A.DD_END,STRFTIME('%Y%m%d', 'NOW'))		>= @DD_SALE
ORDER BY	A.FG_URGT	DESC
,			A.DD_START	DESC
,			A.NO_SEQ	DESC
</P004GetNoticeBSM130T>
<P004SetNoticeBSM131T>
INSERT OR REPLACE INTO BSM131T
(
	DD_START
,	NO_SEQ
,	ID_USER
,	DD_END
,	DD_CONF
) 
VALUES
(
	@DD_START
,	@NO_SEQ
,	@ID_USER
,	@DD_END
,	@DD_CONF
)
</P004SetNoticeBSM131T>
<P002GetCD_CTGYBSM100T>
SELECT		CD_CTGY
,			NM_CTGY
FROM		BSM100T
WHERE		CD_CTGY		=	@CD_CTGY
LIMIT		1
</P002GetCD_CTGYBSM100T>
<P002GetCD_CLASSBSM061T>
SELECT		A.CD_CLASS
,			A.FG_CLASS
,			A.NM_CLASS
,			A.FG_TAX
,			B.NM_BODY AS NM_TAX
,			A.TP_TRADE
,			A.CD_DEPT
,			A.VC_IP_PRINTER
FROM		BSM061T		A
LEFT JOIN	SYM051T		B	ON	B.CD_HEAD = 'BS14' AND A.FG_TAX = B.CD_BODY
WHERE		A.CD_CLASS	=	@CD_CLASS
LIMIT		1
</P002GetCD_CLASSBSM061T>
<P002GetCD_ITEMBSM079T>
SELECT		A.CD_ITEM
,			A.NM_ITEM
,			A.CD_CLASS
,			A.NM_CLASS
,			A.FG_TAX
,			B.NM_BODY AS NM_TAX
,			A.UT_CPRC
,			A.UT_SPRC
,			A.CD_CLASS_DTL
,			A.FG_USE
,			A.CD_DP
,			C.NM_BODY AS NM_DP
,			A.FG_POINT
,			A.AM_APNT
,			A.TT_IUDT
FROM		BSM079T		A
LEFT JOIN	SYM051T		B	ON	B.CD_HEAD = 'BS14' AND A.FG_TAX = B.CD_BODY
LEFT JOIN	SYM051T		C	ON	C.CD_HEAD = 'BS21' AND A.CD_DP = C.CD_BODY
WHERE		A.CD_ITEM	=	@CD_ITEM
LIMIT		1
</P002GetCD_ITEMBSM079T>

<M001LoadTransByNoTrxn>
--- TR거래로드
--- 개발자: TCL
--- 생성일: 04.28
SELECT	DD_SALE, CD_STORE, NO_POS, NO_TRXN, SQ_TRXN, VC_CONT
FROM	SAT010T
WHERE	DD_SALE = @DD_SALE AND
		CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND 
		NO_TRXN = @NO_TRXN
ORDER BY SQ_TRXN;
</M001LoadTransByNoTrxn>
<M001GetPrintMsg>
-- W-MALL 광고메세지 조회
SELECT		NM_DESC
,			FG_SIZ
FROM		BSM041T
WHERE		CD_STORE	=	@CD_STORE
AND			FG_USE		=	'1'	--출력
AND			FG_LOC		=	'E'	--행사글
ORDER BY	SQ_LOC
;

-- 안내메세지 조회
SELECT		NM_DESC
,			FG_SIZ
FROM		BSM041T
WHERE		CD_STORE	=	@CD_STORE
AND			FG_USE		=	'1'	--출력
AND			FG_LOC		=	'F'	--행사글
ORDER BY	SQ_LOC
;
</M001GetPrintMsg>
<P002GetCD>
-- FG_TAX 공통코드
SELECT		CD_BODY	AS FG_TAX
,			NM_BODY	AS NM_TAX
FROM		SYM051T
WHERE		CD_HEAD	=	'BS14'
;

-- CD_DP 공통코드
SELECT		CD_BODY	AS CD_DP
,			NM_BODY	AS NM_DP
FROM		SYM051T
WHERE		CD_HEAD	=	'BS21'
;
</P002GetCD>

--- 반품시, 영수증번호 확인
<GetSaleTrxnHeader>
SELECT VC_CONT
FROM SAT010T
WHERE	CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND
		DD_SALE = @DD_SALE AND 
		NO_TRXN = @NO_TRXN AND
		SQ_TRXN = '000' AND 
		VC_CONT LIKE '100%' LIMIT 1;
</GetSaleTrxnHeader>

--- 반품시, 원거래BASKET HEADER 다시저장
<SaveSaleTrxnHeader>
UPDATE SAT010T
SET VC_CONT = @VC_CONT
WHERE	CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND
		DD_SALE = @DD_SALE AND 
		NO_TRXN = @NO_TRXN AND
		SQ_TRXN = '000';
</SaveSaleTrxnHeader>
<M001GetPrintTitleMsg>
-- 점포 명판 조회
SELECT		NM_DESC
,			FG_SIZ
FROM		BSM041T
WHERE		CD_STORE	=	@CD_STORE
AND			FG_USE		=	'1'	--출력
AND			FG_LOC		=	'H'	--행사글
ORDER BY	SQ_LOC
;
</M001GetPrintTitleMsg>
<M001GetPRM023T>
SELECT		CD_STORE
,			YY_PRM
,			MM_PRM
,			WE_PRM
,			SQ_PRM
,			SQ_LOC
,			FG_TEXT
,			FG_ALIGN
,			NM_DESC
,			FG_USE
,			FG_SIZ
,			'N' AS PRINT_YN
FROM		PRM023T
WHERE		CD_STORE	=	@CD_STORE
AND			FG_USE		=	'1'
;

</M001GetPRM023T>
<M001GetPRM016T>
SELECT		CD_STORE
,			YY_PRM
,			MM_PRM
,			WE_PRM
,			SQ_PRM
,			SQ_PRM_DTL
,			AM_TIME_LIMIT_FR
,			AM_TIME_LIMIT_TO
,			CT_ORDER
,			CD_RULE
,			NO_WINING1
,			NO_WINING2
,			NO_WINING3
,			NO_WINING4
,			NO_WINING5
FROM		PRM016T
WHERE		CD_STORE			=	@CD_STORE
AND			YY_PRM				=	@YY_PRM
AND			MM_PRM				=	@MM_PRM
AND			WE_PRM				=	@WE_PRM
AND			SQ_PRM				=	@SQ_PRM
AND			SQ_PRM_DTL			=	@SQ_PRM_DTL
AND			AM_TIME_LIMIT_FR	<=	@AM_TIME_LIMIT
AND			AM_TIME_LIMIT_TO	>=	@AM_TIME_LIMIT
;
</M001GetPRM016T>
<M001GetTrxnNoPRM016T>
SELECT		CD_STORE
,			YY_PRM
,			MM_PRM
,			WE_PRM
,			SQ_PRM
,			SQ_PRM_DTL
,			AM_TIME_LIMIT_FR
,			AM_TIME_LIMIT_TO
,			CT_ORDER
,			CD_RULE
,			NO_WINING1
,			NO_WINING2
,			NO_WINING3
,			NO_WINING4
,			NO_WINING5
FROM		PRM016T
WHERE		CD_STORE			=	@CD_STORE
AND			YY_PRM				=	@YY_PRM
AND			MM_PRM				=	@MM_PRM
AND			WE_PRM				=	@WE_PRM
AND			SQ_PRM				=	@SQ_PRM
AND			SQ_PRM_DTL			=	@SQ_PRM_DTL
AND			AM_TIME_LIMIT_FR	<=	@AM_TIME_LIMIT
AND			AM_TIME_LIMIT_TO	>=	@AM_TIME_LIMIT
AND			(NO_WINING1			=	@TRXN_NO
OR			NO_WINING2			=	@TRXN_NO
OR			NO_WINING3			=	@TRXN_NO
OR			NO_WINING4			=	@TRXN_NO
OR			NO_WINING5			=	@TRXN_NO)
;
</M001GetTrxnNoPRM016T>
