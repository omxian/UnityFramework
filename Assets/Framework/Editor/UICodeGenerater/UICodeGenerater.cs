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
                UIBuilder builder = BuildClassFactory(go);

                if (builder != null)
                {
                    builder.GenerateUI();
                }
            }
        }

        private static UIBuilder BuildClassFactory(GameObject go)
        {
            string rootTag = go.tag;
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
    }
}