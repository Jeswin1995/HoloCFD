@echo off
SETLOCAL EnableDelayedExpansion

REM The name of the source folder inside the "meshes" directory
SET "sourceFolderName=HE12"

REM The path to the "meshes" directory
SET "meshesPath=%~dp0meshes"

REM The path to the "constant" directory
SET "constantPath=%~dp0constant"

REM Full path to the source folder
SET "fullSourcePath=!meshesPath!\!sourceFolderName!"

REM Full path to the destination "polyMesh" folder
SET "polyMeshPath=!constantPath!\polyMesh"

REM Path to the "300" folder inside the "latestTime" directory
SET "latestTimePath=%~dp0latestTime\!sourceFolderName!\300"

REM Path to the "300" folder in the script's directory
SET "destination300Path=%~dp0300"

REM Check if the source folder exists
IF NOT EXIST "!fullSourcePath!" (
    echo Source folder "!fullSourcePath!" does not exist.
    exit /b
)

REM Remove the existing "polyMesh" folder if it exists
IF EXIST "!polyMeshPath!" (
    echo Removing existing polyMesh folder...
    RMDIR /S /Q "!polyMeshPath!"
)

REM Copy the contents from the source folder to the "polyMesh" folder
echo Copying from "!fullSourcePath!" to "!polyMeshPath!"...
XCOPY /E /I "!fullSourcePath!" "!polyMeshPath!"

REM Check if the "300" folder exists in the "latestTime" directory
IF NOT EXIST "!latestTimePath!" (
    echo "300" folder "!latestTimePath!" does not exist.
    exit /b
)

REM Remove the existing "300" folder in the script's directory if it exists
IF EXIST "!destination300Path!" (
    echo Removing existing "300" folder in the script's directory...
    RMDIR /S /Q "!destination300Path!"
)

REM Copy the "300" folder from the "latestTime" directory to the script's directory
echo Copying from "!latestTimePath!" to "!destination300Path!"...
XCOPY /E /I "!latestTimePath!" "!destination300Path!"

echo Done.
ENDLOCAL
