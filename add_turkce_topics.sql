-- Türkçe dersini ve konularını ekle
-- Tarih dersinin ID'sini bul
SELECT Id, Name FROM Dersler WHERE Name = 'Tarih';

-- Eğer Türkçe dersi yoksa ekle
IF NOT EXISTS (SELECT 1 FROM Dersler WHERE Name = 'Türkçe')
BEGIN
    INSERT INTO Dersler (Name) VALUES ('Türkçe');
END

-- Türkçe dersinin ID'sini al
DECLARE @TurkceDersId INT = (SELECT Id FROM Dersler WHERE Name = 'Türkçe');

-- Türkçe konularını ekle (43 konu)
INSERT INTO Konular (Title, LessonId, Completed, SolvedQuestions, CorrectAnswers, WrongAnswers, Source, Notes, CreatedAtUtc)
SELECT Title, @TurkceDersId, 0, 0, 0, 0, 'KPSS Türkçe', Notes, GETUTCDATE()
FROM (
    VALUES 
    -- Sözcükte Anlam (8 konu)
    ('Sözcüğün Anlam Özellikleri I', 'Sözcüğün temel anlam özellikleri'),
    ('Sözcüğün Anlam Özellikleri II', 'Sözcüğün yan anlam özellikleri'),
    ('Sözcükler Arası Anlam İlişkileri', 'Eş anlamlı, zıt anlamlı, eş sesli sözcükler'),
    ('Sözcükler Arası Anlam İlişkileri Soru Çözümü', 'Anlam ilişkileri soru çözüm teknikleri'),
    ('Mecaz Yolları', 'Mecaz anlam ve mecaz yolları'),
    ('İkileme - Deyim - Atasözleri', 'İkileme, deyim ve atasözleri'),
    ('Altı Çizili Söz', 'Altı çizili sözlerin anlamı'),
    ('Cümle Tamamlama', 'Cümle tamamlama soruları'),
    
    -- Cümlede Anlam (10 konu)
    ('Cümle Yorumu', 'Cümle yorumlama teknikleri'),
    ('İki Cümle Analizi', 'İki cümle arasındaki ilişki'),
    ('Cümle Birleştirme', 'Cümle birleştirme soruları'),
    ('Anlamlarına Göre Cümleler', 'Cümle türleri ve anlamları'),
    ('Anlam İlişkilerine Göre Cümleler', 'Cümleler arası anlam ilişkileri'),
    ('Kanıtlanabilirlik Açısından Cümleler', 'Kanıtlanabilir ve kanıtlanamaz cümleler'),
    ('Cümlede Kavramlar', 'Cümlede kavram analizi'),
    ('Cümle Oluşturma', 'Cümle oluşturma soruları'),
    ('Cümlede Anlam - Etkinlik', 'Cümlede anlam etkinlikleri'),
    ('Anlatım Özellikleri', 'Anlatım özellikleri ve teknikleri'),
    
    -- Paragrafta Anlam (12 konu)
    ('Anlatım Biçimleri', 'Açıklayıcı, öyküleyici, betimleyici anlatım'),
    ('Düşünceyi Geliştirme Yolları I', 'Tanımlama, örnekleme, karşılaştırma'),
    ('Düşünceyi Geliştirme Yolları II', 'Tanık gösterme, sayısal veriler'),
    ('Paragrafta Boşluk Tamamlama', 'Paragrafta boşluk doldurma'),
    ('Paragrafa Cümle Yerleştirme', 'Paragrafa cümle ekleme'),
    ('Paragraf Akışı Bozan Cümle', 'Paragraf akışını bozan cümleler'),
    ('Paragrafı İkiye Bölme', 'Paragrafı bölme teknikleri'),
    ('Paragraf Yer Değiştirme', 'Paragraf sıralaması'),
    ('Paragraf Oluşturma', 'Paragraf oluşturma soruları'),
    ('Paragraf Soru - Cevap', 'Paragraf soru-cevap teknikleri'),
    ('Paragraf Konu - Ana Düşünce', 'Paragraf konu ve ana düşünce'),
    ('Paragraf Yardımcı Düşünceler', 'Paragraf yardımcı düşünceler'),
    
    -- Dil Bilgisi (13 konu)
    ('Dil Bilgisi Yol Haritası', 'Dil bilgisi konularına genel bakış'),
    ('Kök Bilgisi - Yapı Bilgisi', 'Sözcük kökleri ve yapı bilgisi'),
    ('Yapım Ekleri - Yapı Bilgisi', 'Yapım ekleri ve işlevleri'),
    ('Çekim Ekleri - Yapı Bilgisi', 'Çekim ekleri ve işlevleri'),
    ('Yapısına Göre Sözcükler - Yapı Bilgisi', 'Basit, türemiş, birleşik sözcükler'),
    ('Yapı Bilgisi - Çıkmış Sınav Soruları', 'Yapı bilgisi çıkmış sorular'),
    ('Ses Bilgisi', 'Ses bilgisi kuralları'),
    ('Ses Bilgisi - Etkinlik ve Çıkmış Sorular', 'Ses bilgisi etkinlikleri'),
    ('İsim (Ad) - Sözcük Türleri', 'İsim türleri ve özellikleri'),
    ('Sıfat (Ön Ad) - Sözcük Türleri', 'Sıfat türleri ve özellikleri'),
    ('Zamir (Adıl) - Sözcük Türleri', 'Zamir türleri ve özellikleri'),
    ('Tamlamalar - Sözcük Türleri', 'İsim ve sıfat tamlamaları')
) AS NewTopics(Title, Notes)
WHERE NOT EXISTS (
    SELECT 1 FROM Konular 
    WHERE LessonId = @TurkceDersId 
    AND Title = NewTopics.Title
);

-- Sonucu kontrol et
SELECT COUNT(*) as ToplamKonu FROM Konular WHERE LessonId = @TurkceDersId;
SELECT Title FROM Konular WHERE LessonId = @TurkceDersId ORDER BY Id;



