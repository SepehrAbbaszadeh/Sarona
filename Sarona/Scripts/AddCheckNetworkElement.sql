USE [Sarona]
GO

ALTER TABLE [dbo].[NetworkElements]  WITH CHECK ADD  CONSTRAINT [CK_NetworkElements] CHECK  (([dbo].[CheckNetworkElement]([ParentId],[NetworkType],[InstalledCapacity],[UsedCapacity])=(1)))
GO


