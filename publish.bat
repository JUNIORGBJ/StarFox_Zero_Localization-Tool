@echo off
echo Publicando Projetos StarFoxZeroLocalizationTool...

echo.
echo Publicando StarFoxZeroLocalizationTool.Gui...
dotnet publish StarFoxZeroLocalizationTool.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if %errorlevel% neq 0 (
    echo Falha na publicacao do StarFoxZeroLocalizationTool.Gui
    pause
    exit /b 1
)

echo.
echo Publicando StarFoxZeroLocalizationTool.Service...
dotnet publish StarFoxZeroLocalizationTool.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if %errorlevel% neq 0 (
    echo Falha na publicacao do StarFoxZeroLocalizationTool.Service
    pause
    exit /b 1
)

echo.
echo Publicacao concluida com sucesso!
echo.
echo Arquivos da GUI em: bin\Release\net9.0-windows\win-x64\publish\
echo Arquivos do Serviço em: bin\Release\net9.0\win-x64\publish\
pause
