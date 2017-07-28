using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Unity.Framework.Editor
{
    public class UICodeGenerater
    {
        [MenuItem("Assets/Generate UI Base View")]
        public static void CreateUIView()
        {
            if (Selection.activeGameObject != null)
            {
                BuildViewClass(Selection.activeGameObject);
            }
            else
            {
                string[] ids = Selection.assetGUIDs;

                foreach (string id in ids)
                {
                    string dir = AssetDatabase.GUIDToAssetPath(id);
                    if (dir.IndexOf(".") < 0)
                    {
                        string root = Application.dataPath + dir.Substring(dir.IndexOf("/"));
                        string[] files = Directory.GetFiles(root, "*.prefab", SearchOption.AllDirectories);
                        if (files.Length > 0)
                        {
                            foreach (string file in files)
                            {
                                string path = file.Substring(file.IndexOf("Assets"));
                                GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                                BuildViewClass(obj);
                            }
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        private static void BuildViewClass(GameObject go)
        {
            if (null != go)
            {
                BuildViewClass(BuildClassFactory(go));
            }
        }

        /// <summary>
        /// 可用于UI_Item Class的生成
        /// </summary>
        public static void BuildViewClass(UIBuilder builder)
        {
            if (builder != null)
            {
                builder.GenerateUI();
            }
        }

        /// <summary>
        /// 可用于UI_Item Class的生成
        /// </summary>
        /// <param name="go">生成的根GameObject</param>
        /// <param name="tag">需要生成的种类</param>
        /// <returns></returns>
        public static UIBuilder BuildClassFactory(GameObject go, string tag)
        {
            string rootTag = tag;
            if (rootTag == UITagType.UI_Lua)
            {
                return new LuaUIBuilder(go);
            }
            else if (rootTag == UITagType.UI_CSharp)
            {
                return new CSharpUIBuilder(go);
            }
            else
            {
                throw new Exception("Please Define Prefab Tag !");
            }
        }

        private static UIBuilder BuildClassFactory(GameObject go)
        {
            return BuildClassFactory(go, go.tag);
        }
    }
}