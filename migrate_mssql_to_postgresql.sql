-- =====================================================
-- KPSS Study Tracker - MSSQL to PostgreSQL Migration
-- =====================================================
-- Bu script MSSQL veritabanındaki verileri PostgreSQL'e aktarır
--
-- KULLANIM:
-- 1. PostgreSQL veritabanını oluşturun: dotnet ef database update
-- 2. MSSQL'den verileri export edin (aşağıdaki SELECT'leri kullanarak)
-- 3. PostgreSQL'e import edin (aşağıdaki INSERT'leri kullanarak)
--
-- NOT: Bu script manuel export/import için template'dir.
-- Otomatik taşıma için C# migration tool kullanın.
-- =====================================================

-- =====================================================
-- ADIM 1: MSSQL'DEN VERİLERİ EXPORT ETME
-- =====================================================
-- MSSQL'de bu query'leri çalıştırın ve sonuçları CSV olarak kaydedin

-- 1. Kullanıcılar
SELECT 
    Id, 
    Username, 
    PasswordHash, 
    IsAdmin, 
    CreatedAtUtc
FROM Kullanicilar
ORDER BY Id;

-- 2. Dersler
SELECT 
    Id, 
    Name
FROM Dersler
ORDER BY Id;

-- 3. Konular
SELECT 
    Id, 
    Title, 
    LessonId, 
    Source, 
    Notes, 
    CreatedAtUtc
FROM Konular
ORDER BY Id;

-- 4. Kullanıcı Konu İlerlemeleri
SELECT 
    Id, 
    UserId, 
    TopicId, 
    Completed, 
    SolvedQuestions, 
    CorrectAnswers, 
    WrongAnswers, 
    Source, 
    Notes, 
    CreatedAtUtc, 
    CompletedAtUtc
FROM KullaniciKonuIlerlemeleri
ORDER BY Id;

-- 5. Denemeler
SELECT 
    Id, 
    UserId, 
    Name, 
    Date, 
    Notes
FROM Denemeler
ORDER BY Id;

-- 6. Deneme Sonuçları
SELECT 
    Id, 
    MockExamId, 
    Subject, 
    Correct, 
    Wrong, 
    Empty, 
    Net
FROM DenemeSonuclari
ORDER BY Id;

-- 7. Motivasyon Sözleri
SELECT 
    Id, 
    Text, 
    Author
FROM MotivasyonSozleri
ORDER BY Id;

-- 8. Çalışma Planları
SELECT 
    Id, 
    UserId, 
    Title, 
    StartDate, 
    EndDate, 
    Status, 
    Notes, 
    CreatedAt
FROM CalismaPlanlari
ORDER BY Id;

-- 9. Haftalık Planlar
SELECT 
    Id, 
    UserId, 
    WeekNumber, 
    PlanTitle, 
    Description, 
    IsActive, 
    CreatedAt
FROM HaftalikPlanlar
ORDER BY Id;

-- 10. Günlük Planlar
SELECT 
    Id, 
    WeeklyPlanId, 
    DayNumber, 
    LessonId, 
    TopicCount, 
    DailyGoal, 
    IsCompleted, 
    CompletedAt, 
    CreatedAt
FROM GunlukPlanlar
ORDER BY Id;

-- 11. Plan Konuları
SELECT 
    Id, 
    WeeklyPlanId, 
    DailyPlanId, 
    TopicId, 
    IsCompleted, 
    CompletedAt, 
    CreatedAt
FROM PlanKonulari
ORDER BY Id;

-- 12. Yapılacaklar
SELECT 
    Id, 
    UserId, 
    Title, 
    Description, 
    Date, 
    IsCompleted, 
    Priority, 
    Category
FROM Yapilacaklar
ORDER BY Id;

-- =====================================================
-- ADIM 2: POSTGRESQL'E VERİLERİ IMPORT ETME
-- =====================================================
-- PostgreSQL'de bu INSERT'leri çalıştırın
-- CSV dosyalarından veya yukarıdaki SELECT sonuçlarından alın

-- NOT: PostgreSQL'de sequence'ları resetlemek için:
-- SELECT setval('tablename_id_seq', (SELECT MAX(id) FROM tablename));

-- Örnek INSERT template (her tablo için CSV'den veri alın):

-- COPY "Kullanicilar" (Id, Username, PasswordHash, IsAdmin, CreatedAtUtc)
-- FROM '/path/to/kullanicilar.csv' 
-- DELIMITER ',' 
-- CSV HEADER;

-- Sequence reset (tüm import'lardan sonra):
SELECT setval('"Kullanicilar_Id_seq"', (SELECT MAX("Id") FROM "Kullanicilar"));
SELECT setval('"Dersler_Id_seq"', (SELECT MAX("Id") FROM "Dersler"));
SELECT setval('"Konular_Id_seq"', (SELECT MAX("Id") FROM "Konular"));
SELECT setval('"KullaniciKonuIlerlemeleri_Id_seq"', (SELECT MAX("Id") FROM "KullaniciKonuIlerlemeleri"));
SELECT setval('"Denemeler_Id_seq"', (SELECT MAX("Id") FROM "Denemeler"));
SELECT setval('"DenemeSonuclari_Id_seq"', (SELECT MAX("Id") FROM "DenemeSonuclari"));
SELECT setval('"MotivasyonSozleri_Id_seq"', (SELECT MAX("Id") FROM "MotivasyonSozleri"));
SELECT setval('"CalismaPlanlari_Id_seq"', (SELECT MAX("Id") FROM "CalismaPlanlari"));
SELECT setval('"HaftalikPlanlar_Id_seq"', (SELECT MAX("Id") FROM "HaftalikPlanlar"));
SELECT setval('"GunlukPlanlar_Id_seq"', (SELECT MAX("Id") FROM "GunlukPlanlar"));
SELECT setval('"PlanKonulari_Id_seq"', (SELECT MAX("Id") FROM "PlanKonulari"));
SELECT setval('"Yapilacaklar_Id_seq"', (SELECT MAX("Id") FROM "Yapilacaklar"));
