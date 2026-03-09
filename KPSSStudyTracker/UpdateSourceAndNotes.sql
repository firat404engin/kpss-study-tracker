-- Konular tablosundaki Source ve Notes alanlarını doldurma scripti
-- Bu script'i SQL Server Management Studio'da çalıştırabilirsiniz
-- Tüm konular (Tarih, Türkçe, Matematik) için Source ve Notes güncelleme

-- ============================================
-- TARİH KONULARI (47 konu)
-- ============================================
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Genel bakış ve program tanıtımı' WHERE Title = 'Kamp Programı ve Kitap İçeriği' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'SSS bölümü' WHERE Title = 'Sıkça Sorulan Sorular' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Çalışma stratejileri' WHERE Title = 'Tarih Dersine Nasıl Çalışmalıyım' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İlk Türk devletleri' WHERE Title = 'İslamiyet Öncesi Türk Tarihi Siyasi Tarih - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Türk devletleri devam' WHERE Title = 'İslamiyet Öncesi Türk Tarihi Siyasi Tarih - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Kültürel gelişim' WHERE Title = 'İslamiyet Öncesi Türk Tarihi - Kültür ve Uygarlık - III' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İslamiyet sonrası Türk tarihi' WHERE Title = 'Türk - İslam Tarihi - Siyasi Tarih - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İslami kültür' WHERE Title = 'Türk - İslam Tarihi - Kültür ve Uygarlık - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Anadolu Türk tarihi' WHERE Title = 'Türkiye Tarihi Siyasi Tarih - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Anadolu kültürü' WHERE Title = 'Türkiye Tarihi Kültür ve Uygarlık - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı kültürü başlangıç' WHERE Title = 'Osmanlı Kültür ve Uygarlığı - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı kültürü gelişim' WHERE Title = 'Osmanlı Kültür ve Uygarlığı - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı kültürü zirve' WHERE Title = 'Osmanlı Kültür ve Uygarlığı - III' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı kültürü son dönem' WHERE Title = 'Osmanlı Kültür ve Uygarlığı - IV' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı''nın kuruluşu' WHERE Title = 'Osmanlı Kuruluş Dönemi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı yükselişi başlangıç' WHERE Title = 'Osmanlı Yükselme Dönemi - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Osmanlı yükselişi devam' WHERE Title = 'Osmanlı Yükselme Dönemi - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '17. yüzyıl Osmanlı' WHERE Title = 'Osmanlı Duraklama Dönemi - XVII. Yüzyılda Osmanlı' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '18. yüzyıl siyasi tarih' WHERE Title = 'Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Siyasi Tarih - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '18. yüzyıl ıslahatları' WHERE Title = 'Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Islahatlar - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '19. yüzyıl siyasi tarih' WHERE Title = 'Osmanlı Dağılma Dönemi - XIX. Yüzyıl Siyasi Tarih - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '19. yüzyıl ıslahatları' WHERE Title = 'Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '19. yüzyıl ıslahatları devam' WHERE Title = 'Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - III' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Atatürk''ün hayatı' WHERE Title = 'T.C. İnkılap Tarihi (Atatürk''ün Hayatı)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Savaşlar dönemi' WHERE Title = 'Trablusgarp ve Balkan Savaşları' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '1. Dünya Savaşı başlangıç' WHERE Title = 'I. Dünya Savaşı ve Sonuçları - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '1. Dünya Savaşı devam' WHERE Title = 'I. Dünya Savaşı ve Sonuçları - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '1. Dünya Savaşı sonuçları' WHERE Title = 'I. Dünya Savaşı ve Sonuçları - III' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Kurtuluş savaşı hazırlık' WHERE Title = 'Kurtuluş Savaşı Hazırlık Dönemi - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Kurtuluş savaşı hazırlık devam' WHERE Title = 'Kurtuluş Savaşı Hazırlık Dönemi - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'TBMM''nin kuruluşu' WHERE Title = 'I. TBMM Dönemi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Cepheler dönemi başlangıç' WHERE Title = 'Kurtuluş Savaşı Cepheler Dönemi - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Cepheler dönemi devam' WHERE Title = 'Kurtuluş Savaşı Cepheler Dönemi - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Lozan antlaşması' WHERE Title = 'Lozan Barış Antlaşması' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İç politika' WHERE Title = 'İç Politik Gelişmeler' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Atatürk''ün ilkeleri' WHERE Title = 'Atatürk İlkeleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İnkılaplar başlangıç' WHERE Title = 'Atatürk İnkılapları - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'İnkılaplar devam' WHERE Title = 'Atatürk İnkılapları - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Dış politika' WHERE Title = 'Atatürk Dönemi Türk Dış Politikası' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Savaşlar arası dönem' WHERE Title = 'Çağdaş Türk ve Dünya Tarihi (İki Küresel Savaş Arasında Dünya)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '2. Dünya Savaşı başlangıç' WHERE Title = 'II. Dünya Savaşı - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = '2. Dünya Savaşı devam' WHERE Title = 'II. Dünya Savaşı - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Soğuk savaş başlangıç' WHERE Title = 'Soğuk Savaş Dönemi - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Soğuk savaş devam' WHERE Title = 'Soğuk Savaş Dönemi - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Yumuşama dönemi' WHERE Title = 'Yumuşama (Detant) Dönemi ve Sonrası - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Yumuşama dönemi devam' WHERE Title = 'Yumuşama (Detant) Dönemi ve Sonrası - II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Küreselleşme başlangıç' WHERE Title = 'Küreselleşen Dünya - I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Tarih', Notes = 'Küreselleşme devam' WHERE Title = 'Küreselleşen Dünya - II' AND (Source IS NULL OR Notes IS NULL);

-- ============================================
-- TÜRKÇE KONULARI (43 konu)
-- ============================================
-- Sözcükte Anlam (8 konu)
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Sözcüğün temel anlam özellikleri' WHERE Title = 'Sözcüğün Anlam Özellikleri I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Sözcüğün yan anlam özellikleri' WHERE Title = 'Sözcüğün Anlam Özellikleri II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Eş anlamlı, zıt anlamlı, eş sesli sözcükler' WHERE Title = 'Sözcükler Arası Anlam İlişkileri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Anlam ilişkileri soru çözüm teknikleri' WHERE Title = 'Sözcükler Arası Anlam İlişkileri Soru Çözümü' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Mecaz anlam ve mecaz yolları' WHERE Title = 'Mecaz Yolları' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'İkileme, deyim ve atasözleri' WHERE Title = 'İkileme - Deyim - Atasözleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Altı çizili sözlerin anlamı' WHERE Title = 'Altı Çizili Söz' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümle tamamlama soruları' WHERE Title = 'Cümle Tamamlama' AND (Source IS NULL OR Notes IS NULL);

-- Cümlede Anlam (10 konu)
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümle yorumlama teknikleri' WHERE Title = 'Cümle Yorumu' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'İki cümle arasındaki ilişki' WHERE Title = 'İki Cümle Analizi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümle birleştirme soruları' WHERE Title = 'Cümle Birleştirme' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümle türleri ve anlamları' WHERE Title = 'Anlamlarına Göre Cümleler' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümleler arası anlam ilişkileri' WHERE Title = 'Anlam İlişkilerine Göre Cümleler' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Kanıtlanabilir ve kanıtlanamaz cümleler' WHERE Title = 'Kanıtlanabilirlik Açısından Cümleler' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümlede kavram analizi' WHERE Title = 'Cümlede Kavramlar' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümle oluşturma soruları' WHERE Title = 'Cümle Oluşturma' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Cümlede anlam etkinlikleri' WHERE Title = 'Cümlede Anlam - Etkinlik' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Anlatım özellikleri ve teknikleri' WHERE Title = 'Anlatım Özellikleri' AND (Source IS NULL OR Notes IS NULL);

-- Paragrafta Anlam (12 konu)
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Açıklayıcı, öyküleyici, betimleyici anlatım' WHERE Title = 'Anlatım Biçimleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Tanımlama, örnekleme, karşılaştırma' WHERE Title = 'Düşünceyi Geliştirme Yolları I' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Tanık gösterme, sayısal veriler' WHERE Title = 'Düşünceyi Geliştirme Yolları II' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragrafta boşluk doldurma' WHERE Title = 'Paragrafta Boşluk Tamamlama' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragrafa cümle ekleme' WHERE Title = 'Paragrafa Cümle Yerleştirme' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf akışını bozan cümleler' WHERE Title = 'Paragraf Akışı Bozan Cümle' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragrafı bölme teknikleri' WHERE Title = 'Paragrafı İkiye Bölme' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf sıralaması' WHERE Title = 'Paragraf Yer Değiştirme' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf oluşturma soruları' WHERE Title = 'Paragraf Oluşturma' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf soru-cevap teknikleri' WHERE Title = 'Paragraf Soru - Cevap' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf konu ve ana düşünce' WHERE Title = 'Paragraf Konu - Ana Düşünce' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Paragraf yardımcı düşünceler' WHERE Title = 'Paragraf Yardımcı Düşünceler' AND (Source IS NULL OR Notes IS NULL);

-- Dil Bilgisi (13 konu)
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Dil bilgisi konularına genel bakış' WHERE Title = 'Dil Bilgisi Yol Haritası' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Sözcük kökleri ve yapı bilgisi' WHERE Title = 'Kök Bilgisi - Yapı Bilgisi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Yapım ekleri ve işlevleri' WHERE Title = 'Yapım Ekleri - Yapı Bilgisi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Çekim ekleri ve işlevleri' WHERE Title = 'Çekim Ekleri - Yapı Bilgisi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Basit, türemiş, birleşik sözcükler' WHERE Title = 'Yapısına Göre Sözcükler - Yapı Bilgisi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Yapı bilgisi çıkmış sorular' WHERE Title = 'Yapı Bilgisi - Çıkmış Sınav Soruları' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Ses bilgisi kuralları' WHERE Title = 'Ses Bilgisi' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Ses bilgisi etkinlikleri' WHERE Title = 'Ses Bilgisi - Etkinlik ve Çıkmış Sorular' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'İsim türleri ve özellikleri' WHERE Title = 'İsim (Ad) - Sözcük Türleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Sıfat türleri ve özellikleri' WHERE Title = 'Sıfat (Ön Ad) - Sözcük Türleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'Zamir türleri ve özellikleri' WHERE Title = 'Zamir (Adıl) - Sözcük Türleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Türkçe', Notes = 'İsim ve sıfat tamlamaları' WHERE Title = 'Tamlamalar - Sözcük Türleri' AND (Source IS NULL OR Notes IS NULL);

-- ============================================
-- MATEMATİK KONULARI (15 konu)
-- ============================================
-- Problemler (10 konu)
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Sayı problemleri temel' WHERE Title = 'Sayı-Kesir Problemleri - 1' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Sayı problemleri gelişmiş' WHERE Title = 'Sayı-Kesir Problemleri - 2' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Kesir problemleri temel' WHERE Title = 'Sayı-Kesir Problemleri - 3' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Kesir problemleri gelişmiş' WHERE Title = 'Sayı-Kesir Problemleri - 4' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Karma problemler' WHERE Title = 'Sayı-Kesir Problemleri - 5' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Yaş problemleri temel' WHERE Title = 'Yaş Problemleri - 1' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Yaş problemleri gelişmiş' WHERE Title = 'Yaş Problemleri - 2' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Karışım ve oran problemleri' WHERE Title = 'Karışım Problemleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'İşçi ve havuz problemleri' WHERE Title = 'İşçi-Havuz Problemleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Yüzde problemleri' WHERE Title = 'Yüzde-Kar-Zarar Problemleri - 1' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Kar-zarar problemleri' WHERE Title = 'Yüzde-Kar-Zarar Problemleri - 2' AND (Source IS NULL OR Notes IS NULL);

-- Sayısal Mantık (5 konu)
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Mantık temel kavramları' WHERE Title = 'Temel Mantık Kavramları' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Küme teorisi ve işlemler' WHERE Title = 'Kümeler ve Küme İşlemleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Sayı örüntüleri ve diziler' WHERE Title = 'Örüntü ve Diziler' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Mantıksal akıl yürütme' WHERE Title = 'Akıl Yürütme Problemleri' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Matematik', Notes = 'Sıralama ve karşılaştırma' WHERE Title = 'Sıralama ve Karşılaştırma Problemleri' AND (Source IS NULL OR Notes IS NULL);

-- ============================================
-- COĞRAFYA KONULARI (60 konu)
-- ============================================
-- Coğrafi Konum (5 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Coğrafi konum temel' WHERE Title = 'COĞRAFİ KONUM - 1' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Coğrafi konum gelişmiş' WHERE Title = 'COĞRAFİ KONUM - 2' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Coğrafi konum uygulamaları' WHERE Title = 'COĞRAFİ KONUM - 3' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Coğrafi konum detaylar' WHERE Title = 'COĞRAFİ KONUM - 4' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Coğrafi konum özet' WHERE Title = 'COĞRAFİ KONUM - 5' AND (Source IS NULL OR Notes IS NULL);

-- Türkiye'nin Yer Şekilleri (11 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'İç ve dış kuvvetler' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 1 (İÇ ve DIŞ KUVVET)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Jeolojik zamanlar' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 2 (JEOLOJİK ZAMANLAR)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Dağlar bölüm 1' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 3 (DAĞLAR 1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Dağlar bölüm 2' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 4 (DAĞLAR 2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Platolar' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 5 (PLATOLAR)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Ovalar' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 6 (OVALAR)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Akarsu şekilleri' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 7 (AKARSU ŞEKİLLERİ)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Rüzgar ve buzullar' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 8 (RÜZGAR VE BUZULLAR)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Karstik şekiller' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 9 (KARSTİK ŞEKİLLER)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Kıyı şekillenmesi' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 10 (KIYI ŞEKİLLENMESİ)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Yer şekilleri genel özellikler' WHERE Title = 'TÜRKİYE''NİN YER ŞEKİLLERİ - 11 (GENEL ÖZELLİKLER)' AND (Source IS NULL OR Notes IS NULL);

-- Türkiye İklimi (4 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'İklim sıcaklık' WHERE Title = 'TÜRKİYE İKLİMİ - 1 (SICAKLIK)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Basınç ve rüzgar' WHERE Title = 'TÜRKİYE İKLİMİ - 2 (BASINÇ ve RÜZGAR)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Nemlilik ve yağış' WHERE Title = 'TÜRKİYE İKLİMİ - 3 (NEMLİLİK ve YAĞIŞ)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Mikro iklimler' WHERE Title = 'TÜRKİYE İKLİMİ - 4 (MİKROKLİMALAR)' AND (Source IS NULL OR Notes IS NULL);

-- Su, Toprak ve Bitki Varlığı (10 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Akarsular bölüm 1' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 1 (AKARSULAR -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Akarsular bölüm 2' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 2 (AKARSULAR -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Göller' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 3 (GÖLLER)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Denizler' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 4 (DENİZLER)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Yeraltı suları' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 5 (YERALTI SULARI)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Topraklar bölüm 1' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 6 (TOPRAKLAR -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Topraklar bölüm 2' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 7 (TOPRAKLAR -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Bitki örtüsü bölüm 1' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 8 (BİTKİ -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Bitki örtüsü bölüm 2' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 9 (BİTKİ -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Bitki örtüsü bölüm 3' WHERE Title = 'TÜRKİYE''NİN SU, TOPRAK ve BİTKİ VARLIĞI - 10 (BİTKİ -3)' AND (Source IS NULL OR Notes IS NULL);

-- Çevre ve Doğal Afetler (3 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Çevre ve afetler bölüm 1' WHERE Title = 'TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 1' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Çevre ve afetler bölüm 2' WHERE Title = 'TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 2' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Çevre ve afetler bölüm 3' WHERE Title = 'TÜRKİYE''DE ÇEVRE ve DOĞAL AFETLER - 3' AND (Source IS NULL OR Notes IS NULL);

-- Beşeri Coğrafya (8 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Nüfus bölüm 1' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 1 (NÜFUS -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Nüfus bölüm 2' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 2 (NÜFUS -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Nüfus bölüm 3' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 3 (NÜFUS -3)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Nüfus bölüm 4' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 4 (NÜFUS -4)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Yerleşme bölüm 1' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 5 (YERLEŞME -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Yerleşme bölüm 2' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 6 (YERLEŞME -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Yerleşme bölüm 3' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 7 (YERLEŞME -3)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Göçler' WHERE Title = 'TÜRKİYE''NİN BEŞERİ COĞRAFYASI - 8 (GÖÇLER)' AND (Source IS NULL OR Notes IS NULL);

-- Ekonomik Coğrafya (16 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Ekonomi politikaları' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 1 (EKONOMİ POLİTİKALARI)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Tarım bölüm 1' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 2 (TARIM 1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Tarım bölüm 2' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 3 (TARIM 2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Tarım bölüm 3' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 4 (TARIM 3)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Tarım bölüm 4' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 5 (TARIM 4)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Hayvancılık' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 6 (HAYVANCILIK)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Madenler bölüm 1' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 7 (MADENLER 1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Madenler bölüm 2' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 8 (MADENLER 2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Enerji kaynakları bölüm 1' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 9 (ENERJİ KAYNAKLARI -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Enerji kaynakları bölüm 2' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 10 (ENERJİ KAYNAKLARI 2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Sanayi' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 11 (SANAYİ)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Sanayi ve ticaret' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 12 (SANAYİ ve TİCARET)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Ulaşım bölüm 1' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 13 (ULAŞIM -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Ulaşım bölüm 2' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 14 (ULAŞIM -2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Turizm bölüm 1' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 15 (TURİZM -1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Turizm bölüm 2' WHERE Title = 'TÜRKİYE''NİN EKONOMİK COĞRAFYASI - 16 (TURİZM -2)' AND (Source IS NULL OR Notes IS NULL);

-- Bölge Kavramı ve Sistematik (3 konu)
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Jeopolitik bölge bölüm 1' WHERE Title = 'TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 1)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Jeopolitik bölge bölüm 2' WHERE Title = 'TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 2)' AND (Source IS NULL OR Notes IS NULL);
UPDATE Konular SET Source = 'KPSS Coğrafya', Notes = 'Plan bölgeler' WHERE Title = 'TÜRKİYE''DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (PLAN BÖLGELER)' AND (Source IS NULL OR Notes IS NULL);

PRINT 'Tüm konular için Source ve Notes alanları güncellendi!';
PRINT 'Toplam güncellenen konu sayısı: Tarih (47), Türkçe (43), Matematik (15), Coğrafya (60) = 165 konu';
