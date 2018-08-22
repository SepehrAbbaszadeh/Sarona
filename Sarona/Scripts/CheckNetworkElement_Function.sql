USE [Sarona]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<sepehr>
-- Create date: <970531>
-- Description:	<To use in check constraint in NetworkElements table.>
-- =============================================
CREATE FUNCTION [dbo].[CheckNetworkElement] 
(
	-- Add the parameters for the function here
	@parentId bigint,
	@networkType int,
	@installedCap int,
	@usedCap int
)
RETURNS bit
AS
BEGIN
	IF(@installedCap<@usedCap)
	BEGIN
		return 0
	END
	if(@parentId is not null)
	begin
		if(@networkType = 1 OR @networkType = 2)
		begin 
			return 1
		end
	end
	else
	begin
		return 1
	end
	return 0
END
GO


