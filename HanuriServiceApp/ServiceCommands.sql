-- W-mall 회원카드 번호로 포인트 조회
-- RETURNS: 가용 포인트 점수
-- PARAM: @MEMBER_NO
<WSGetPoint>
SELECT 0 AS SAL_UPOINT
</WSGetPoint>
-- 회원 포인트카드 사용여부 체크
-- PARAM: @MEMBER_NO
-- returns:-1(미존재카드), 0(사용회원), 1(만료회원), 2(재발급), 3(점수이관)
<WSCheckCardNo>
SELECT 0
</WSCheckCardNo>
-- 홈페이지 포인트 적립
-- PARAMS: @MEMBER_NO, @USER_ID, @WEB_POINT, @REMARK
-- RETURNS: 없음
<WSSavingPoint>

</WSSavingPoint>

-- 회원 정보리스트 조회
-- PARAMS: @MEMBER_NO
-- RETURNS: DATASET 회원정보
<WSGetMemberInfo>
SELECT [MEMBER_NO]
      ,[CUS_NAME]
      ,[END_IND]
      ,[SAL_UPOINT]
      ,[MINUS_POINT]
      ,[SAL_TPOINT]
      ,[SAL_AMT]
  FROM [wsois].[dbo].[ws600viw] 
 WHERE [MEMBER_NO] = @MEMBER_NO
</WSGetMemberInfo>

-- 홈페이지 회원 포인트 적립리스트 조회
-- PARAMS: @MEMBER_NO
-- RETURNS: 
<WSGetSavingPointInfo>
SELECT [DATES]
      ,[MEMBER_NO]
      ,[SEQ]
      ,[IDATES]
      ,[IEMP_NO]
      ,[REMARKS]
      ,[ADD_FLAG]
      ,[WEB_USER_ID]
      ,[WEB_POINT]
      ,[APPLY_FLAG]
  FROM [db_ws].[ws612tbl] with (nolock)
 WHERE [MEMBER_NO] = @MEMBER_NO
</WSGetSavingPointInfo>


