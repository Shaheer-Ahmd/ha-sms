# Quick Run Script for Student Management System
# This script runs the WinForms application directly

Write-Host "=== Student Management System - Quick Run ===" -ForegroundColor Cyan
Write-Host ""

# Check if application is built
$exePath = ".\StudentManagementSystem.UI\bin\Debug\StudentManagementSystem.UI.exe"

if (Test-Path $exePath) {
    Write-Host "✓ Application found at: $exePath" -ForegroundColor Green
    Write-Host "Launching application..." -ForegroundColor Yellow
    Write-Host ""
    
    # Run the application
    Start-Process $exePath
    
    Write-Host "✓ Application launched!" -ForegroundColor Green
    Write-Host ""
    Write-Host "If you see errors, check:" -ForegroundColor Yellow
    Write-Host "1. SQL Server is running (Docker or local)"
    Write-Host "2. Database 'StudentManagementDB' exists"
    Write-Host "3. Connection string in App.config is correct"
    Write-Host ""
} else {
    Write-Host "✗ Application not built yet!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please build the solution first:" -ForegroundColor Yellow
    Write-Host "Option 1: Open StudentManagementSystem.sln in Visual Studio and press F5"
    Write-Host "Option 2: Run 'msbuild StudentManagementSystem.sln' (if MSBuild is in PATH)"
    Write-Host ""
    Write-Host "Current location checked: $exePath" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
