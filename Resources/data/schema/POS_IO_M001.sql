<SelectSAT300T>
SELECT AM_ITEM
FROM SAT300T
WHERE DD_SALE = @DD_SALE AND
		CD_STORE = @CD_STORE AND
		NO_POS = @NO_POS AND
		ID_ITEM = @ID_ITEM
ORDER BY CT_ITEM
LIMIT 1		
</SelectSAT300T>
<SaveSAT300T>
INSERT OR REPLACE INTO SAT300T
(
	DD_SALE
, CD_STORE
, NO_POS
, ID_ITEM
, AM_ITEM
, CT_ITEM
) 
VALUES
(
	@DD_SALE
, @CD_STORE
, @NO_POS
, @ID_ITEM
, @AM_ITEM
, (SELECT IFNULL(MAX(CT_ITEM),0) + 1 FROM SAT300T WHERE DD_SALE = @DD_SALE AND CD_STORE = @CD_STORE AND NO_POS = @NO_POS AND ID_ITEM = @ID_ITEM)
)
</SaveSAT300T>
<SaveSAT301T>
INSERT OR REPLACE INTO SAT301T
(
	DD_SALE
, CD_STORE
, NO_POS
,	NO_USER
, ID_ITEM
, AM_ITEM
, CT_ITEM
) 
VALUES
(
	@DD_SALE
, @CD_STORE
, @NO_POS
,	@NO_USER
, @ID_ITEM
, @AM_ITEM
, (SELECT IFNULL(MAX(CT_ITEM),0) + 1 FROM SAT301T WHERE DD_SALE = @DD_SALE AND CD_STORE = @CD_STORE AND NO_POS = @NO_POS AND NO_USER = @NO_USER AND ID_ITEM = @ID_ITEM)
)
</SaveSAT301T>
<SaveSAT302T>
INSERT OR REPLACE INTO SAT302T
(
	DD_SALE
, CD_STORE
, NO_POS
,	NO_USER
,	SQ_SHIFT
, ID_ITEM
, AM_ITEM
, CT_ITEM
) 
VALUES
(
	@DD_SALE
, @CD_STORE
, @NO_POS
,	@NO_USER
,	@SQ_SHIFT
, @ID_ITEM
, @AM_ITEM
, (SELECT IFNULL(MAX(CT_ITEM),0) + 1 FROM SAT302T WHERE DD_SALE = @DD_SALE AND CD_STORE = @CD_STORE AND NO_POS = @NO_POS AND NO_USER = @NO_USER AND SQ_SHIFT = @SQ_SHIFT AND ID_ITEM = @ID_ITEM)
)
</SaveSAT302T>