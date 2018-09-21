USE [Sarona]
GO

ALTER TABLE [dbo].[NumberingPools]  WITH CHECK ADD  CONSTRAINT [CK_NumberingPools] CHECK  (([dbo].[CheckNumberingRange]([from],[to],[min],[max])=(1)))
GO