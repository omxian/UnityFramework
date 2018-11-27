package com.wmcy.YouYiCheng;

import android.app.Activity;
import android.content.res.AssetManager;
import com.unity3d.player.UnityPlayer;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class YYCUtil
{
    public static byte[] readAsset(String path)
    {
        if (UnityPlayer.currentActivity == null) {
            return null;
        }
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        byte[] buf = new byte['?'];

        AssetManager am = UnityPlayer.currentActivity.getAssets();
        InputStream inputStream = null;
        try
        {
            inputStream = am.open(path);
            int len;
            while ((len = inputStream.read(buf)) != -1) {
                outputStream.write(buf, 0, len);
            }
            outputStream.close();
            inputStream.close();
            return outputStream.toByteArray();
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
        catch (Exception e1)
        {
            e1.printStackTrace();
        }
        return null;
    }

    public static boolean isAssetExist(String path)
    {
        if (UnityPlayer.currentActivity == null) {
            return false;
        }
        AssetManager am = UnityPlayer.currentActivity.getAssets();
        try
        {
            am.open(path);
            return true;
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
        return false;
    }

    public static int getAssetLength(String path)
    {
        if (UnityPlayer.currentActivity == null) {
            return -1;
        }
        AssetManager am = UnityPlayer.currentActivity.getAssets();
        try
        {
            InputStream localInputStream = am.open(path);
            return localInputStream.available();
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
        return -1;
    }
}
