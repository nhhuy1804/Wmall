<GetUploadedTransCount>
SELECT COUNT(*)
FROM SAT011T
WHERE CD_STOR = @CD_STOR AND
		NO_POS = @NO_POS AND
		DD_SALE = @DD_SALE AND
		FG_PRS = 'Y'
</GetUploadedTransCount>