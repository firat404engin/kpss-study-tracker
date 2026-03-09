-- Admin kullanıcılarına IsAdmin = 1 yap
-- Önce IsAdmin kolonunu ekle (varsa hata vermez)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Kullanicilar]') AND name = 'IsAdmin')
BEGIN
    ALTER TABLE Kullanicilar ADD IsAdmin BIT NOT NULL DEFAULT 0;
    PRINT 'IsAdmin kolonu eklendi';
END
ELSE
BEGIN
    PRINT 'IsAdmin kolonu zaten mevcut';
END
GO

-- Admin kullanıcısına IsAdmin yetkisi ver
UPDATE Kullanicilar 
SET IsAdmin = 1 
WHERE Username = 'admin';

PRINT 'Admin kullanıcısı güncellendi';
GO

-- Sonuçları göster
SELECT Id, Username, IsAdmin, CreatedAtUtc FROM Kullanicilar;
GO
