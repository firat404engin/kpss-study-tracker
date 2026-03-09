-- Vatandaşlık dersini ekle
USE [KPSSStudyTrackerDb]
GO

-- Vatandaşlık dersini ekle (eğer yoksa)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Dersler] WHERE [Name] = 'Vatandaşlık')
BEGIN
    INSERT INTO [dbo].[Dersler] ([Name])
    VALUES ('Vatandaşlık')
    PRINT 'Vatandaşlık dersi eklendi.'
END
ELSE
BEGIN
    PRINT 'Vatandaşlık dersi zaten mevcut.'
END

-- Mevcut dersleri kontrol et
SELECT [Id], [Name] FROM [dbo].[Dersler] ORDER BY [Name]
GO



