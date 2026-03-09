@echo off
echo KPSS Study Tracker baslatiliyor...
echo.

cd /d "C:\Users\Firat\Desktop\KPSS Study Tracker\KPSSStudyTracker"

echo Mevcut dotnet sureclerini durduruyor...
taskkill /f /im dotnet.exe 2>nul
taskkill /f /im KPSSStudyTracker.exe 2>nul

echo Temizlik yapiliyor...
if exist bin rmdir /s /q bin
if exist obj rmdir /s /q obj

echo Proje derleniyor...
dotnet build

if %errorlevel% neq 0 (
    echo Derleme hatasi! Cikiliyor...
    pause
    exit /b 1
)

echo Uygulama baslatiliyor...
echo Tarayicinizda http://localhost:5203 adresini acin
echo Login: admin / admin123
echo.
echo Cikmak icin Ctrl+C basin
echo.

dotnet run --urls http://localhost:5203

pause
