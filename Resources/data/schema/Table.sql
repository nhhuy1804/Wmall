<TableGetTable>
SELECT		*
FROM		Table
WHERE		Floor		= @Floor  
AND 		IsDelete	= 0
ORDER BY	Id
</TableGetTable>
<InsertTable>
INSERT OR REPLACE INTO Table(Id, Index, X, Y, Floor, IsDelete)
VALUES(@Id, @Index, @X, @Y, @Floor, @IsDelete)
</InsertTable>