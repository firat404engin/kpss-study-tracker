# 🚀 KPSS Study Tracker - Quick Deploy Script (Windows)
# Bu script projeyi GitHub'a yükler

Write-Host "🚀 KPSS Study Tracker - Hızlı Deployment Başlatılıyor..." -ForegroundColor Cyan
Write-Host ""

# Kullanıcıdan GitHub username al
$GITHUB_USER = Read-Host "GitHub kullanıcı adınız"

# Repository adı
$REPO_NAME = "kpss-study-tracker"

Write-Host ""
Write-Host "📦 Git repository başlatılıyor..." -ForegroundColor Yellow

# Proje dizinine git
Set-Location "c:\Users\Firat\Desktop\KPSS Study Tracker"

# Git başlat
git init

# .gitignore oluştur (eğer yoksa)
if (-not (Test-Path .gitignore)) {
    @"
bin/
obj/
*.user
*.suo
.vs/
.vscode/
appsettings.Development.json
*.db
*.db-shm
*.db-wal
node_modules/
.idea/
*.log
"@ | Out-File -FilePath .gitignore -Encoding UTF8
    Write-Host "✅ .gitignore oluşturuldu" -ForegroundColor Green
}

# Tüm dosyaları ekle
git add .

# Commit
git commit -m "Initial commit - KPSS Study Tracker with PostgreSQL"

Write-Host "✅ Git commit tamamlandı" -ForegroundColor Green
Write-Host ""

# GitHub remote ekle
git remote add origin "https://github.com/$GITHUB_USER/$REPO_NAME.git"
git branch -M main

Write-Host "📤 GitHub'a push yapılacak..." -ForegroundColor Yellow
Write-Host ""
Write-Host "⚠️  GitHub'da '$REPO_NAME' repository'sini oluşturduğunuzdan emin olun!" -ForegroundColor Red
Write-Host ""
Write-Host "GitHub'da yeni repository oluşturmak için:" -ForegroundColor Cyan
Write-Host "   https://github.com/new" -ForegroundColor White
Write-Host ""

$CONFIRM = Read-Host "Repository oluşturdunuz mu? (y/n)"

if ($CONFIRM -eq "y") {
    Write-Host ""
    Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
    git push -u origin main
    
    Write-Host ""
    Write-Host "✅ GitHub'a başarıyla yüklendi!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🎉 Şimdi Render.com'da deploy edin:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. https://render.com adresine gidin" -ForegroundColor White
    Write-Host "2. Sign Up (GitHub ile)" -ForegroundColor White
    Write-Host "3. New → Blueprint" -ForegroundColor White
    Write-Host "4. Repository seçin: $REPO_NAME" -ForegroundColor White
    Write-Host "5. Apply" -ForegroundColor White
    Write-Host ""
    Write-Host "🌐 URL: https://$REPO_NAME.onrender.com" -ForegroundColor Green
    Write-Host ""
    Write-Host "💡 İpucu: İlk açılış 30-50 saniye sürebilir (cold start)" -ForegroundColor Yellow
    Write-Host ""
    
    # Tarayıcıda aç
    $OPEN = Read-Host "Render.com'u tarayıcıda açmak ister misiniz? (y/n)"
    if ($OPEN -eq "y") {
        Start-Process "https://render.com"
    }
} else {
    Write-Host ""
    Write-Host "❌ İptal edildi." -ForegroundColor Red
    Write-Host ""
    Write-Host "Önce GitHub'da repository oluşturun:" -ForegroundColor Yellow
    Write-Host "   https://github.com/new" -ForegroundColor White
    Write-Host ""
    Write-Host "Sonra bu komutu çalıştırın:" -ForegroundColor Yellow
    Write-Host "   git push -u origin main" -ForegroundColor White
    Write-Host ""
}

Write-Host "Devam etmek için bir tuşa basın..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
