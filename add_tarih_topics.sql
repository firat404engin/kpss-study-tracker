-- Tarih dersinin ID'sini bul
SELECT Id, Name FROM Dersler WHERE Name = 'Tarih';

-- Eğer Tarih dersi yoksa ekle
IF NOT EXISTS (SELECT 1 FROM Dersler WHERE Name = 'Tarih')
BEGIN
    INSERT INTO Dersler (Name) VALUES ('Tarih');
END

-- Tarih dersinin ID'sini al
DECLARE @TarihDersId INT = (SELECT Id FROM Dersler WHERE Name = 'Tarih');

-- Tüm tarih konularını ekle (47 konu)
INSERT INTO Konular (Title, LessonId, Completed, SolvedQuestions, CorrectAnswers, WrongAnswers, Source, Notes, CreatedAtUtc)
SELECT Title, @TarihDersId, 0, 0, 0, 0, 'KPSS Tarih', Notes, GETUTCDATE()
FROM (
    VALUES 
    -- İslamiyet Öncesi Türk Tarihi (3 konu)
    ('İslamiyet Öncesi Türk Tarihi Siyasi Tarih - I', 'İlk Türk devletleri'),
    ('İslamiyet Öncesi Türk Tarihi Siyasi Tarih - II', 'Türk devletleri devam'),
    ('İslamiyet Öncesi Türk Tarihi - Kültür ve Uygarlık - III', 'Kültürel gelişim'),
    
    -- Türk - İslam Tarihi (2 konu)
    ('Türk - İslam Tarihi - Siyasi Tarih - I', 'İslamiyet sonrası Türk tarihi'),
    ('Türk - İslam Tarihi - Kültür ve Uygarlık - II', 'İslami kültür'),
    
    -- Türkiye Tarihi (2 konu)
    ('Türkiye Tarihi Siyasi Tarih - I', 'Anadolu Türk tarihi'),
    ('Türkiye Tarihi Kültür ve Uygarlık - II', 'Anadolu kültürü'),
    
    -- Osmanlı Kültür ve Uygarlığı (4 konu)
    ('Osmanlı Kültür ve Uygarlığı - I', 'Osmanlı kültürü başlangıç'),
    ('Osmanlı Kültür ve Uygarlığı - II', 'Osmanlı kültürü gelişim'),
    ('Osmanlı Kültür ve Uygarlığı - III', 'Osmanlı kültürü zirve'),
    ('Osmanlı Kültür ve Uygarlığı - IV', 'Osmanlı kültürü son dönem'),
    
    -- Osmanlı Siyasi Tarihi (6 konu)
    ('Osmanlı Kuruluş Dönemi', 'Osmanlı''nın kuruluşu'),
    ('Osmanlı Yükselme Dönemi - I', 'Osmanlı yükselişi başlangıç'),
    ('Osmanlı Yükselme Dönemi - II', 'Osmanlı yükselişi devam'),
    ('Osmanlı Duraklama Dönemi - XVII. Yüzyılda Osmanlı', '17. yüzyıl Osmanlı'),
    ('Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Siyasi Tarih - I', '18. yüzyıl siyasi tarih'),
    ('Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Islahatlar - II', '18. yüzyıl ıslahatları'),
    ('Osmanlı Dağılma Dönemi - XIX. Yüzyıl Siyasi Tarih - I', '19. yüzyıl siyasi tarih'),
    ('Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - II', '19. yüzyıl ıslahatları'),
    ('Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - III', '19. yüzyıl ıslahatları devam'),
    
    -- T.C. İnkılap Tarihi (16 konu)
    ('T.C. İnkılap Tarihi (Atatürk''ün Hayatı)', 'Atatürk''ün hayatı'),
    ('Trablusgarp ve Balkan Savaşları', 'Savaşlar dönemi'),
    ('I. Dünya Savaşı ve Sonuçları - I', '1. Dünya Savaşı başlangıç'),
    ('I. Dünya Savaşı ve Sonuçları - II', '1. Dünya Savaşı devam'),
    ('I. Dünya Savaşı ve Sonuçları - III', '1. Dünya Savaşı sonuçları'),
    ('Kurtuluş Savaşı Hazırlık Dönemi - I', 'Kurtuluş savaşı hazırlık'),
    ('Kurtuluş Savaşı Hazırlık Dönemi - II', 'Kurtuluş savaşı hazırlık devam'),
    ('I. TBMM Dönemi', 'TBMM''nin kuruluşu'),
    ('Kurtuluş Savaşı Cepheler Dönemi - I', 'Cepheler dönemi başlangıç'),
    ('Kurtuluş Savaşı Cepheler Dönemi - II', 'Cepheler dönemi devam'),
    ('Lozan Barış Antlaşması', 'Lozan antlaşması'),
    ('İç Politik Gelişmeler', 'İç politika'),
    ('Atatürk İlkeleri', 'Atatürk''ün ilkeleri'),
    ('Atatürk İnkılapları - I', 'İnkılaplar başlangıç'),
    ('Atatürk İnkılapları - II', 'İnkılaplar devam'),
    ('Atatürk Dönemi Türk Dış Politikası', 'Dış politika'),
    
    -- Çağdaş Türk ve Dünya Tarihi (10 konu)
    ('Çağdaş Türk ve Dünya Tarihi (İki Küresel Savaş Arasında Dünya)', 'Savaşlar arası dönem'),
    ('II. Dünya Savaşı - I', '2. Dünya Savaşı başlangıç'),
    ('II. Dünya Savaşı - II', '2. Dünya Savaşı devam'),
    ('Soğuk Savaş Dönemi - I', 'Soğuk savaş başlangıç'),
    ('Soğuk Savaş Dönemi - II', 'Soğuk savaş devam'),
    ('Yumuşama (Detant) Dönemi ve Sonrası - I', 'Yumuşama dönemi'),
    ('Yumuşama (Detant) Dönemi ve Sonrası - II', 'Yumuşama dönemi devam'),
    ('Küreselleşen Dünya - I', 'Küreselleşme başlangıç'),
    ('Küreselleşen Dünya - II', 'Küreselleşme devam')
) AS NewTopics(Title, Notes)
WHERE NOT EXISTS (
    SELECT 1 FROM Konular 
    WHERE LessonId = @TarihDersId 
    AND Title = NewTopics.Title
);

-- Sonucu kontrol et
SELECT COUNT(*) as ToplamKonu FROM Konular WHERE LessonId = @TarihDersId;
SELECT Title FROM Konular WHERE LessonId = @TarihDersId ORDER BY Id;
