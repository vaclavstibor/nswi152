@echo off
REM WebJob startup script for MyWebJob continuous task
REM This script is executed by Azure App Service when the WebJob starts

cd /d "%~dp0"
dotnet MyWebJob.dll
