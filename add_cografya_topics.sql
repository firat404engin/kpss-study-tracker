-- Coğrafya dersini ve konularını ekle
-- Eğer Coğrafya dersi yoksa ekle
IF NOT EXISTS (SELECT 1 FROM Dersler WHERE Name = 'Coğrafya')
BEGIN
    INSERT INTO Dersler (Name) VALUES ('Coğrafya');
END

-- Coğrafya dersinin ID'sini al
DECLARE @CografyaDersId INT = (SELECT Id FROM Dersler WHERE Name = 'Coğrafya');

-- Coğrafya konularını ekle (60 konu)
INSERT INTO Konular (Title, LessonId, Completed, SolvedQuestions, CorrectAnswers, WrongAnswers, Source, Notes, CreatedAtUtc)
SELECT Title, @CografyaDersId, 0, 0, 0, 0, 'KPSS Coğrafya', Notes, GETUTCDATE()
FROM (
    VALUES 
    -- Coğrafi Konum (5 konu)
    ('COĞRAFİ KONUM - 1', 'Matematik konum ve özel konum'),
    ('COĞRAFİ KONUM - 2', 'Enlem ve boylam etkileri'),
    ('COĞRAFİ KONUM - 3', 'Türkiye''nin matematik konumu'),
    ('COĞRAFİ KONUM - 4', 'Türkiye''nin özel konumu'),
    ('COĞRAFİ KONUM - 5', 'Coğrafi konumun etkileri'),
    
    -- Türkiye''nin Yer Şekilleri (11 konu)
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 1 (İÇ ve DIŞ KUVVET)', 'İç ve dış kuvvetler'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 2 (JEOLOJİK ZAMANLAR)', 'Jeolojik zamanlar ve Türkiye'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 3 (DAĞLAR 1)', 'Kuzey Anadolu Dağları'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 4 (DAĞLAR 2)', 'Güney Anadolu ve Doğu Anadolu Dağları'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 5 (PLATOLAR)', 'Türkiye''deki platolar'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 6 (OVALAR)', 'Türkiye''deki ovalar'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 7 (AKARSU ŞEKİLLERİ)', 'Akarsu şekilleri ve vadiler'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 8 (RÜZGAR VE BUZULLAR)', 'Rüzgar ve buzul şekilleri'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 9 (KARSTİK ŞEKİLLER)', 'Karstik şekiller'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 10 (KIYI ŞEKİLLENMESİ)', 'Kıyı şekilleri'),
    ('TÜRKİYE''NİN YER ŞEKİLLERİ - 11 (GENEL ÖZELLİKLER)', 'Yer şekillerinin genel özellikleri'),
    
    -- Türkiye İklimi (4 konu)
    ('TÜRKİYE İKLİMİ - 1 (SICAKLIK)', 'Türkiye''de sıcaklık dağılışı'),
    ('TÜRKİYE İKLİMİ - 2 (BASINÇ ve RÜZGAR)', 'Basınç ve rüzgar sistemleri'),
    ('TÜRKİYE İKLİMİ - 3 (NEMLİLİK ve YAĞIŞ)', 'Nemlilik ve yağış dağılışı'),
    ('TÜRKİYE İKLİMİ - 4 (MİKROKLİMALAR)', 'Mikroklima alanları'),
    
    -- Su, Toprak ve Bitki Varlığı (10 konu)
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 1 (AKARSULAR -1)', 'Akarsu havzaları'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 2 (AKARSULAR -2)', 'Akarsu rejimleri'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 3 (GÖLLER)', 'Türkiye''deki göller'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 4 (DENİZLER)', 'Denizler ve özellikleri'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 5 (YERALTI SULARI)', 'Yeraltı suları'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 6 (TOPRAKLAR -1)', 'Toprak türleri'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 7 (TOPRAKLAR -2)', 'Toprak dağılışı'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 8 (BİTKİ -1)', 'Bitki örtüsü türleri'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 9 (BİTKİ -2)', 'Bitki örtüsü dağılışı'),
    ('TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 10 (BİTKİ -3)', 'Bitki örtüsü koruma'),
    
    -- Çevre ve Doğal Afetler (3 konu)
    ('TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 1', 'Çevre sorunları'),
    ('TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 2', 'Doğal afetler'),
    ('TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 3', 'Çevre koruma'),
    
    -- Beşeri Coğrafya (8 konu)
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 1 (NÜFUS -1)', 'Nüfus artışı'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 2 (NÜFUS -2)', 'Nüfus dağılışı'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 3 (NÜFUS -3)', 'Nüfus yapısı'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 4 (NÜFUS -4)', 'Nüfus politikaları'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 5 (YERLEŞME -1)', 'Kır yerleşmeleri'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 6 (YERLEŞME -2)', 'Kent yerleşmeleri'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 7 (YERLEŞME -3)', 'Yerleşme tipleri'),
    ('TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 8 (GÖÇLER)', 'İç ve dış göçler'),
    
    -- Ekonomik Coğrafya (16 konu)
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 1 (EKONOMİ POLİTİKALARI)', 'Ekonomi politikaları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 2 (TARIM 1)', 'Tarım alanları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 3 (TARIM 2)', 'Tarım ürünleri'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 4 (TARIM 3)', 'Tarım teknikleri'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 5 (TARIM 4)', 'Tarım sorunları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 6 (HAYVANCILIK)', 'Hayvancılık faaliyetleri'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 7 (MADENLER 1)', 'Maden yatakları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 8 (MADENLER 2)', 'Maden çıkarımı'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 9 (ENERJİ KAYNAKLARI -1)', 'Enerji kaynakları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 10 (ENERJİ KAYNAKLARI 2)', 'Enerji üretimi'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 11 (SANAYİ)', 'Sanayi kolları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 12 (SANAYİ ve TİCARET)', 'Sanayi ve ticaret'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 13 (ULAŞIM -1)', 'Kara yolu ulaşımı'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 14 (ULAŞIM -2)', 'Demir yolu ve hava yolu'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 15 (TURİZM -1)', 'Turizm alanları'),
    ('TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 16 (TURİZM -2)', 'Turizm türleri'),
    
    -- Bölge Kavramı ve Sistematik (3 konu)
    ('TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 1)', 'Jeopolitik bölgeler'),
    ('TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 2)', 'Bölge özellikleri'),
    ('TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (PLAN BÖLGELER)', 'Plan bölgeleri')
) AS NewTopics(Title, Notes)
WHERE NOT EXISTS (
    SELECT 1 FROM Konular 
    WHERE LessonId = @CografyaDersId 
    AND Title = NewTopics.Title
);

-- Sonucu kontrol et
SELECT COUNT(*) as ToplamKonu FROM Konular WHERE LessonId = @CografyaDersId;
SELECT Title FROM Konular WHERE LessonId = @CografyaDersId ORDER BY Id;



