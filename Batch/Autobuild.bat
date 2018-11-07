@echo off

@set unity="C:\Program Files\Unity2018\Editor\Unity.exe"

echo 正在生成APK文件...

%unity%  -batchmode -quit -nographics -executeMethod Batchmode.BuildAndroid  -logFile D:\Editor.log -projectPath "D:\workspace\UnityFramework" 

echo APK文件生成完毕!

pause