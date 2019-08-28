USE [Sarona]
GO

/****** Object:  UserDefinedFunction [dbo].[GetRondType]    Script Date: 09/09/1397 11:04:27 ب.ظ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetRondType]
(
	@prefix nvarchar(MAX)
)
RETURNS tinyint
AS
BEGIN		
	DECLARE @a int
	DECLARE @b int
	DECLARE @c int
	DECLARE @d int
	DECLARE @e int


	IF(LEN(@prefix) = 4)
	BEGIN
		set @a = ASCII(SUBSTRING(@prefix,1,1))
		set @b = ASCII(SUBSTRING(@prefix,2,1))
		set @c = ASCII(SUBSTRING(@prefix,3,1))
		set @d = ASCII(SUBSTRING(@prefix,4,1))

		IF(@a = @c AND @b=@d)
			RETURN 0
		IF(@c='0' and @d = '0')
			RETURN 0
		IF(@b=@c AND @c=@d)
			RETURN 0
		IF(@a=@d and @d=@b+1)
			RETURN 0
		IF(@c = @d)
			RETURN 1
		IF(@b = @d)
			RETURN 2
	END

	IF(LEN(@prefix) = 5)
	begin
		set @a = ASCII(SUBSTRING(@prefix,1,1))
		set @b = ASCII(SUBSTRING(@prefix,2,1))
		set @c = ASCII(SUBSTRING(@prefix,3,1))
		set @d = ASCII(SUBSTRING(@prefix,4,1))
		set @e = ASCII(SUBSTRING(@prefix, 5,1))

		IF(@c=@d and @d=@e)
			return 0;
		IF(@c=@b+1 and @d=@c+1 and @e = @d+1)
			return 0;
		IF(@e=@d-1 and @d=@c-1 and @c=@b-1)
			return 0;
		IF(@a = @d and  @b=@e)
			return 1;
		IF(@d = '0' and @e='0')
			return 1;

		IF(@c = @d-1 and @d = @e-1)
			return 2;
		IF(@d = @c-1 and @e = @d - 1)
			return 2;
		IF(@c=@d and @d='0')
			return 2
	end

	RETURN 3
END
GO


