USE [Sarona]
GO

ALTER TABLE [dbo].[NumberingPools]  WITH CHECK ADD  CONSTRAINT [CK_NumberingPools] CHECK  (([dbo].[CheckNumberingPool]([Prefix],[Min],[Max],[SecondaryMin],[SecondaryMax])=(1)))
GO

ALTER TABLE [dbo].[NumberingPools] CHECK CONSTRAINT [CK_NumberingPools]
GO


