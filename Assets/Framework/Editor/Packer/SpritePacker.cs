using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Unity.Framework.Editor
{
    /// <summary>
    /// 处理图集相关问题
    /// </summary>
    public static class SpritePacker
    {
        private const string spritePath = "Assets/ExternalAsset/UI/Sprite";

        //设置图片，压缩质量,Packing Tag等属性
        public static void SetSprite()
        {
            int i = 0;
            string[] directories = Directory.GetDirectories(spritePath);
            foreach (string path in directories)
            {
                i++;
                foreach (string file in Directory.GetFiles(path))
                {
                    if (Path.GetExtension(file) == ".meta")
                    {
                        continue;
                    }

                    TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(file);
                    if (importer.textureType == TextureImporterType.Sprite)
                    {
                        //设置sprite packing tag
                        importer.spritePackingTag = i.ToString();
                        //关闭mipmap,节省内存
                        importer.mipmapEnabled = false;
                    }

                    importer.SaveAndReimport();
                    AssetDatabase.SaveAssets();
                }
            }
            AssetDatabase.Refresh();
        }
    }
}