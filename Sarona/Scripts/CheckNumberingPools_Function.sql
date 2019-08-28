USE [Sarona]
GO

/****** Object:  UserDefinedFunction [dbo].[CheckNumberingPool]    Script Date: 11/21/2018 3:19:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CheckNumberingPool]
(
	@prefix nvarchar(MAX),
	@min int,
	@max int,
	@secMin int,
	@secMax int
)
RETURNS bit
AS
BEGIN
	Declare @length int
	DECLARE @point int
	declare @query nvarchar(200)
	declare @count int
	IF(@min>@max)
		return 0
	IF (@secMin>@secMax)
		return 0
		
	set @length = LEN(@prefix)
	
	set @point = 1; 

	WHILE @point<@length
	BEGIN
		set @query = SUBSTRING(@prefix,1,@point)
		select @count=COUNT(*) from NumberingPools where Prefix=@query
		IF(@count > 0)
			RETURN 0
		set @point = @point + 1
	END

	select @count=Count(*) from NumberingPools where Prefix like @prefix + '%'
	IF(@count >2)
		RETURN 0

	return 1

END
GO


