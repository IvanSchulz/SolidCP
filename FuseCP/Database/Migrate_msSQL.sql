-- This is to upgrade from FuseCP .net to FuseCP CoreWCF

-- Removing Helicon Zoo
-- GroupID: 42
-- ProviderID: 135

DELETE FROM [Quotas] WHERE [GroupID] = 42
GO
DELETE FROM [ServiceDefaultProperties] WHERE [ProviderID] = 135
GO
DELETE FROM [ServiceItemTypes] WHERE [GroupID] = 42
GO
DELETE FROM [VirtualGroups] WHERE [GroupID] = 42 
GO
DELETE FROM [dbo].[Services] WHERE [ProviderID] = 135
GO
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 135
GO
DELETE FROM [dbo].[ResourceGroups] WHERE [GroupID] = 42
GO


-- Removing Microsoft Web Platform Installer (WebPI)
DELETE FROM [dbo].[SystemSettings] WHERE [PropertyName] = 'WpiSettings'
GO

-- Removing Windows Server 2003
-- ProviderID: 1

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 1)
BEGIN
	UPDATE [Services] SET [ProviderID] = 111 WHERE [ProviderID] = 1
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 1 AND [DisplayName] = 'Windows Server 2003')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 1
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 1 AND DisplayName = 'Windows Server 2003'
END
GO

-- Removing Windows Server 2008
-- ProviderID: 100

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 100)
BEGIN
	UPDATE [Services] SET [ProviderID] = 111 WHERE [ProviderID] = 100
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 100 AND DisplayName = 'Windows Server 2008')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 100
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 100 AND [DisplayName] = 'Windows Server 2008'
END
GO

-- Removing Windows Server 2012
-- ProviderID: 104

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 104)
BEGIN
	UPDATE [Services] SET [ProviderID] = 111 WHERE [ProviderID] = 104
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 104 AND DisplayName = 'Windows Server 2012')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 104
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 104 AND [DisplayName] = 'Windows Server 2012'
END
GO

-- Removing MSFTP60
-- ProviderID: 3

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 3)
BEGIN
	UPDATE [Services] SET [ProviderID] = 113 WHERE [ProviderID] = 3
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 3 AND DisplayName = 'Microsoft FTP Server 6.0')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 3
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 3 AND [DisplayName] = 'Microsoft FTP Server 6.0'
END
GO

-- Removing MSFTP70
-- ProviderID: 102

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 102)
BEGIN
	UPDATE [Services] SET [ProviderID] = 113 WHERE [ProviderID] = 102
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 102 AND DisplayName = 'Microsoft FTP Server 7.0')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 102
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 102 AND [DisplayName] = 'Microsoft FTP Server 7.0'
END
GO

-- Removing MSFTP80
-- ProviderID: 106

IF EXISTS (SELECT * FROM [dbo].[Services] WHERE [ProviderID] = 106)
BEGIN
	UPDATE [Services] SET [ProviderID] = 113 WHERE [ProviderID] = 106
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 106 AND DisplayName = 'Microsoft FTP Server 8.0')
BEGIN
DELETE FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = 106
DELETE FROM [dbo].[Providers] WHERE [ProviderID] = 106 AND [DisplayName] = 'Microsoft FTP Server 8.0'
END
GO

-- Rename of Providers

UPDATE [dbo].[Providers]
SET ProviderType = REPLACE(ProviderType, 'SolidCP', 'FuseCP')
WHERE ProviderType LIKE '%SolidCP%';

UPDATE [dbo].[ServiceItemTypes]
SET TypeName = REPLACE(TypeName, 'SolidCP', 'FuseCP')
WHERE TypeName LIKE '%SolidCP%';

UPDATE [dbo].[ResourceGroups]
SET GroupController = REPLACE(GroupController, 'SolidCP', 'FuseCP')
WHERE GroupController LIKE '%SolidCP%';

UPDATE [dbo].[ScheduleTasks]
SET TaskType = REPLACE(TaskType, 'SolidCP', 'FuseCP')
WHERE TaskType LIKE '%SolidCP%';

UPDATE [dbo].[ScheduleTaskViewConfiguration]
SET Description = REPLACE(Description, 'SolidCP', 'FuseCP')
WHERE Description LIKE '%SolidCP%';

UPDATE [dbo].[ServiceDefaultProperties]
SET PropertyValue = REPLACE(PropertyValue, 'SolidCP', 'FuseCP')
WHERE PropertyValue LIKE '%SolidCP%';

UPDATE [dbo].[Themes]
SET DisplayName = REPLACE(DisplayName, 'SolidCP', 'FuseCP')
WHERE DisplayName LIKE '%SolidCP%';
