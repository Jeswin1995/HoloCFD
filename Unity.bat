@echo off
set UNITY_EXECUTABLE="C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor\Unity.exe"
set PROJECT_PATH="C:\Users\XR-Lab\Documents\Jeswin Files\Project HoloCFD\openfoamdocker\TestCFD"
set METHOD_NAME="AssetBundleBuilder.AssetBundleBuilderFunction"
:: Start Unity in batch mode with the AutoRunScript
%UNITY_EXECUTABLE% -batchmode -projectPath %PROJECT_PATH% -executeMethod %METHOD_NAME%
:: Exit the script
exit 
