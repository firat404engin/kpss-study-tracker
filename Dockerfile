# 🚀 KPSS Study Tracker - Render.com Ücretsiz Deployment

## 📋 Gereksinimler

- GitHub hesabı
- Render.com hesabı (ücretsiz)

## 🎯 Adım 1: Projeyi GitHub'a Push

### 1.1. Git Repository Oluştur

```bash
cd "c:\Users\Firat\Desktop\KPSS Study Tracker"

# Git başlat
git init

# .gitignore ekle
echo "bin/
obj/
*.user
*.suo
.vs/
.vscode/
appsettings.Development.json
*.db
*.db-shm
*.db-wal
Migrations/" > .gitignore

# Commit
git add .
git commit -m "Initial commit - KPSS Study Tracker"
```

### 1.2. GitHub'da Repository Oluştur

1. [github.com](https://github.com) → New Repository
2. Repository adı: `kpss-study-tracker`
3. Public (ücretsiz için gerekli)
4. Create repository

### 1.3. GitHub'a Push

```bash
# Remote ekle
git remote add origin https://github.com/KULLANICI_ADINIZ/kpss-study-tracker.git

# Push
git branch -M main
git push -u origin main
```

## 🌐 Adım 2: Render.com Setup

### 2.1. Render Hesabı Oluştur

1. [render.com](https://render.com) → Sign Up
2. GitHub ile giriş yap
3. Repository erişimi ver

### 2.2. PostgreSQL Veritabanı Oluştur

1. Dashboard → **New +** → **PostgreSQL**
2. Settings:
   - **Name:** `kpss-db`
   - **Database:** `kpssdb`
   - **User:** `kpss_user`
   - **Region:** Frankfurt (EU) - Türkiye'ye yakın
   - **Plan:** **Free**
3. **Create Database**
4. **Internal Database URL**'i kopyala (sonra lazım)

### 2.3. Web Service Oluştur

1. Dashboard → **New +** → **Web Service**
2. GitHub repository'yi seç: `kpss-study-tracker`
3. Settings:
   - **Name:** `kpss-study-tracker`
   - **Region:** Frankfurt (EU)
   - **Branch:** `main`
   - **Root Directory:** `KPSSStudyTracker`
   - **Runtime:** `Docker` VEYA `.NET`
   - **Build Command:** `dotnet publish -c Release -o out`
   - **Start Command:** `cd out && dotnet KPSSStudyTracker.dll --urls http://0.0.0.0:$PORT`
   - **Plan:** **Free**

4. **Environment Variables** ekle:
   ```
   ASPNETCORE_ENVIRONMENT = Production
   ConnectionStrings__DefaultConnection = [PostgreSQL Internal URL'i yapıştır]
   ```

5. **Create Web Service**

## 📝 Adım 3: Production Konfigürasyonu

### 3.1. Dockerfile Oluştur (Render için)

<function_calls>
<invoke name="Write">
<parameter name="contents"># Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyala
COPY ["KPSSStudyTracker/KPSSStudyTracker.csproj", "KPSSStudyTracker/"]
RUN dotnet restore "KPSSStudyTracker/KPSSStudyTracker.csproj"

# Tüm dosyaları kopyala ve build et
COPY . .
WORKDIR "/src/KPSSStudyTracker"
RUN dotnet build "KPSSStudyTracker.csproj" -c Release -o /app/build

# Publish aşaması
FROM build AS publish
RUN dotnet publish "KPSSStudyTracker.csproj" -c Release -o /app/publish

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Port expose
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "KPSSStudyTracker.dll"]
