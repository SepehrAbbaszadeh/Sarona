CREATE FUNCTION CheckNumberingRange 
(
	@from bigint,
	@to bigint,
	@min tinyint,
	@max tinyint
)
RETURNS bit
AS
BEGIN
	DECLARE @c as int;
	IF(@min>@max)
	BEGIN
		return 0
	END
	IF(@from>@to)
	BEGIN
		return 0
	END
	SELECT @c = COUNT(*)
				FROM dbo.NumberingPools np
				where np.[From]<=@to AND np.[To]>=@from;
	IF(@c>0)
	BEGIN
		return 0
	END
	return 1
END



