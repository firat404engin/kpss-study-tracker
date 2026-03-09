-- Matematik dersini ve konularını ekle
-- Eğer Matematik dersi yoksa ekle
IF NOT EXISTS (SELECT 1 FROM Dersler WHERE Name = 'Matematik')
BEGIN
    INSERT INTO Dersler (Name) VALUES ('Matematik');
END

-- Matematik dersinin ID'sini al
DECLARE @MatematikDersId INT = (SELECT Id FROM Dersler WHERE Name = 'Matematik');

-- Matematik konularını ekle (15 konu)
INSERT INTO Konular (Title, LessonId, Completed, SolvedQuestions, CorrectAnswers, WrongAnswers, Source, Notes, CreatedAtUtc)
SELECT Title, @MatematikDersId, 0, 0, 0, 0, 'KPSS Matematik', Notes, GETUTCDATE()
FROM (
    VALUES 
    -- Problemler (10 konu)
    ('Sayı-Kesir Problemleri - 1', 'Sayı problemleri temel'),
    ('Sayı-Kesir Problemleri - 2', 'Sayı problemleri gelişmiş'),
    ('Sayı-Kesir Problemleri - 3', 'Kesir problemleri temel'),
    ('Sayı-Kesir Problemleri - 4', 'Kesir problemleri gelişmiş'),
    ('Sayı-Kesir Problemleri - 5', 'Karma problemler'),
    ('Yaş Problemleri - 1', 'Yaş problemleri temel'),
    ('Yaş Problemleri - 2', 'Yaş problemleri gelişmiş'),
    ('Karışım Problemleri', 'Karışım ve oran problemleri'),
    ('İşçi-Havuz Problemleri', 'İşçi ve havuz problemleri'),
    ('Yüzde-Kar-Zarar Problemleri - 1', 'Yüzde problemleri'),
    ('Yüzde-Kar-Zarar Problemleri - 2', 'Kar-zarar problemleri'),
    
    -- Sayısal Mantık (5 konu)
    ('Temel Mantık Kavramları', 'Mantık temel kavramları'),
    ('Kümeler ve Küme İşlemleri', 'Küme teorisi ve işlemler'),
    ('Örüntü ve Diziler', 'Sayı örüntüleri ve diziler'),
    ('Akıl Yürütme Problemleri', 'Mantıksal akıl yürütme'),
    ('Sıralama ve Karşılaştırma Problemleri', 'Sıralama ve karşılaştırma')
) AS NewTopics(Title, Notes)
WHERE NOT EXISTS (
    SELECT 1 FROM Konular 
    WHERE LessonId = @MatematikDersId 
    AND Title = NewTopics.Title
);

-- Sonucu kontrol et
SELECT COUNT(*) as ToplamKonu FROM Konular WHERE LessonId = @MatematikDersId;
SELECT Title FROM Konular WHERE LessonId = @MatematikDersId ORDER BY Id;



