#!/bin/bash

# 🚀 KPSS Study Tracker - Quick Deploy Script
# Bu script projeyi GitHub'a yükler ve Render'a deploy eder

echo "🚀 KPSS Study Tracker - Hızlı Deployment Başlatılıyor..."
echo ""

# Kullanıcıdan GitHub username al
read -p "GitHub kullanıcı adınız: " GITHUB_USER

# Repository adı
REPO_NAME="kpss-study-tracker"

echo ""
echo "📦 Git repository başlatılıyor..."

# Git başlat
git init

# .gitignore oluştur (eğer yoksa)
if [ ! -f .gitignore ]; then
    cat > .gitignore << EOF
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
EOF
    echo "✅ .gitignore oluşturuldu"
fi

# Tüm dosyaları ekle
git add .

# Commit
git commit -m "Initial commit - KPSS Study Tracker with PostgreSQL"

echo "✅ Git commit tamamlandı"
echo ""

# GitHub remote ekle
git remote add origin https://github.com/$GITHUB_USER/$REPO_NAME.git
git branch -M main

echo "📤 GitHub'a push yapılıyor..."
echo ""
echo "⚠️  GitHub'da '$REPO_NAME' repository'sini oluşturduğunuzdan emin olun!"
echo ""
read -p "Repository oluşturdunuz mu? (y/n): " CONFIRM

if [ "$CONFIRM" == "y" ]; then
    git push -u origin main
    echo ""
    echo "✅ GitHub'a başarıyla yüklendi!"
    echo ""
    echo "🎉 Şimdi Render.com'da deploy edin:"
    echo ""
    echo "1. https://render.com adresine gidin"
    echo "2. Sign Up (GitHub ile)"
    echo "3. New → Blueprint"
    echo "4. Repository seçin: $REPO_NAME"
    echo "5. Apply"
    echo ""
    echo "🌐 URL: https://$REPO_NAME.onrender.com"
    echo ""
else
    echo "❌ İptal edildi. Önce GitHub'da repository oluşturun:"
    echo "   https://github.com/new"
    echo ""
    echo "Sonra bu komutu çalıştırın:"
    echo "   git push -u origin main"
fi
