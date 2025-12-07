# Student Management System - Quick Setup Script

Write-Host "=== Student Management System - Phase 2 Setup ===" -ForegroundColor Cyan
Write-Host ""

# Configuration
$sqlServer = "localhost,1433"
$database = "StudentManagementDB"
$sqlUser = "sa"
$sqlPassword = Read-Host "Enter SQL Server SA password" -AsSecureString
$sqlPasswordPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($sqlPassword))

$connectionString = "Server=$sqlServer;Database=$database;User Id=$sqlUser;Password=$sqlPasswordPlain;TrustServerCertificate=True;"

Write-Host ""
Write-Host "Step 1: Testing SQL Server Connection..." -ForegroundColor Yellow

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connectionString
    $conn.Open()
    Write-Host "✓ Connected to SQL Server successfully!" -ForegroundColor Green
    
    # Check if database exists
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT DB_ID('$database')"
    $dbExists = $cmd.ExecuteScalar()
    
    if ($dbExists) {
        Write-Host "✓ Database '$database' exists!" -ForegroundColor Green
    } else {
        Write-Host "✗ Database '$database' not found!" -ForegroundColor Red
        Write-Host "  Please run StudentManagementDB_creation.sql first." -ForegroundColor Yellow
    }
    
    $conn.Close()
} catch {
    Write-Host "✗ Failed to connect to SQL Server!" -ForegroundColor Red
    Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "  Make sure SQL Server is running on $sqlServer" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "Step 2: Updating App.config files..." -ForegroundColor Yellow

# Update DAL App.config
$dalConfigPath = ".\StudentManagementSystem.DAL\App.config"
if (Test-Path $dalConfigPath) {
    [xml]$config = Get-Content $dalConfigPath
    $connString = $config.configuration.connectionStrings.add | Where-Object { $_.name -eq "StudentManagementDB" }
    if ($connString) {
        $connString.connectionString = $connectionString
        $config.Save((Resolve-Path $dalConfigPath))
        Write-Host "✓ Updated DAL App.config" -ForegroundColor Green
    }
} else {
    Write-Host "✗ DAL App.config not found at $dalConfigPath" -ForegroundColor Red
}

# Update BLL App.config
$bllConfigPath = ".\StudentManagementSystem.BLL\App.config"
if (Test-Path $bllConfigPath) {
    [xml]$config = Get-Content $bllConfigPath
    $connString = $config.configuration.connectionStrings.add | Where-Object { $_.name -eq "StudentManagementDB" }
    if ($connString) {
        $connString.connectionString = $connectionString
        $config.Save((Resolve-Path $bllConfigPath))
        Write-Host "✓ Updated BLL App.config" -ForegroundColor Green
    }
} else {
    Write-Host "✗ BLL App.config not found at $bllConfigPath" -ForegroundColor Red
}

# Update UI App.config
$uiConfigPath = ".\StudentManagementSystem.UI\App.config"
if (Test-Path $uiConfigPath) {
    [xml]$config = Get-Content $uiConfigPath
    $connString = $config.configuration.connectionStrings.add | Where-Object { $_.name -eq "StudentManagementDB" }
    if ($connString) {
        $connString.connectionString = $connectionString
        $config.Save((Resolve-Path $uiConfigPath))
        Write-Host "✓ Updated UI App.config" -ForegroundColor Green
    }
} else {
    Write-Host "✗ UI App.config not found at $uiConfigPath" -ForegroundColor Red
}

Write-Host ""
Write-Host "Step 3: Setting BLL Implementation..." -ForegroundColor Yellow

$bllType = Read-Host "Choose BLL implementation (1=LINQ, 2=StoredProcedure)"
$bllValue = if ($bllType -eq "2") { "StoredProcedure" } else { "LINQ" }

if (Test-Path $uiConfigPath) {
    [xml]$config = Get-Content $uiConfigPath
    $appSetting = $config.configuration.appSettings.add | Where-Object { $_.key -eq "BLLImplementation" }
    if ($appSetting) {
        $appSetting.value = $bllValue
        $config.Save((Resolve-Path $uiConfigPath))
        Write-Host "✓ Set BLL implementation to: $bllValue" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "Step 4: Checking NuGet packages..." -ForegroundColor Yellow

if (Test-Path ".\packages") {
    Write-Host "✓ NuGet packages folder exists" -ForegroundColor Green
} else {
    Write-Host "⚠ NuGet packages not restored. Run 'nuget restore' or rebuild in Visual Studio" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Setup Complete! ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Connection String: $connectionString" -ForegroundColor White
Write-Host "BLL Implementation: $bllValue" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Open StudentManagementSystem.sln in Visual Studio 2022"
Write-Host "2. Build > Rebuild Solution"
Write-Host "3. Run the application (F5)"
Write-Host "4. You can switch BLL implementations at runtime using the 'Switch BLL' button"
Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
