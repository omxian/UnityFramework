@echo off

@set unity="C:\Program Files\Unity2018\Editor\Unity.exe"

echo ��������APK�ļ�...

%unity%  -batchmode -quit -nographics -executeMethod Batchmode.BuildAndroid  -logFile D:\Editor.log -projectPath "D:\workspace\UnityFramework" 

echo APK�ļ��������!

pause