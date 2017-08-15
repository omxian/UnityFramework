using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Unity.Framework.Editor
{
    public abstract class UIBuilder
    {
        protected string content = string.Empty;
        private string savePath;
        private string templatePath;
        private Transform[] childrenTransform;
        private string fileSuffix;
        private GameObject rootGameObject = null;
        private BaseComponentInfo componentInfo = null;

        /// <summary>
        /// 重新命名类名,为空即不需要重命名
        /// </summary>
        protected string renameFile = ""; 
        /// <summary>
        /// 需要跳过的父节点
        /// </summary>
        protected List<Transform> skipParentTransform = new List<Transform>();

        public UIBuilder(GameObject go)
        {
            rootGameObject = go;
            childrenTransform = go.GetComponentsInChildren<Transform>(true);
        }

        protected void SetComponentInfo(BaseComponentInfo c)
        {
            componentInfo = c;
        }

        protected BaseComponentInfo GetComponentInfo()
        {
            return componentInfo;
        }

        protected bool CheckTag(string tag)
        {
            return tag != string.Empty && tag != UITagType.Untagged;
        }

        protected void CheckPrefabName(string name)
        {
            if (name.Contains(" "))
            {
                throw new Exception(name + " Contains Space! Please Check!");
            }
        }

        protected Transform[] GetChildrenTransform()
        {
            return childrenTransform;
        }

        protected GameObject GetRootGameObject()
        {
            return rootGameObject;
        }

        protected void SetFileSuffix(string str)
        {
            fileSuffix = str;
        }

        protected string GetSavePath()
        {
            return savePath;
        }

        protected void SetSavePath(string path)
        {
            savePath = path;
        }

        protected void SetTemplatePath(string str)
        {
            templatePath = str;
        }

        protected string GetTemplatePath()
        {
            return templatePath;
        }
        protected virtual string GetClassName(string name = "")
        {
            if (name != "")
            {
                return name + "View";
            }
            if (rootGameObject != null)
            {
                return rootGameObject.name + "View";
            }
            return "";
        }

        protected string GetResultContent()
        {
            return content;
        }
        public abstract void GenerateUI();
        protected void SaveFile(string className = "")
        {
            string genPath = Path.Combine(Application.dataPath, GetSavePath());
            string fileName = Path.Combine(genPath, GetClassName(className) + fileSuffix);

            if (!Directory.Exists(genPath))
            {
                Directory.CreateDirectory(genPath);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (FileStream writer = File.Open(fileName, FileMode.OpenOrCreate))
            {
                byte[] fileData = Encoding.UTF8.GetBytes(GetResultContent());
                writer.Write(fileData, 0, fileData.Length);
            }
        }
        protected string ReadTemplateString()
        {
            using (StreamReader reader = new StreamReader(GetTemplatePath()))
            {
                return reader.ReadToEnd();
            }
        }
        protected string GetHierarchy(Transform obj)
        {
            if (obj == null)
                return "";
            string path = obj.name;

            while (obj.parent != null)
            {
                obj = obj.parent;
                path = obj.name + "/" + path;
            }

            path = path.Substring(path.IndexOf("/") + 1);
            return path;
        }

        /// <summary>
        /// 检查Transform是否需要跳过
        /// </summary>
        /// <param name="skipParentList"></param>
        /// <param name="checkTarget"></param>
        /// <returns></returns>
        protected bool CheckNeedToSkip(List<Transform> skipParentList, Transform checkTarget)
        {
            foreach (Transform skipTran in skipParentList)
            {
                if (checkTarget.IsChildOf(skipTran))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 解析名称
        /// 0: 名称 1: 所在索引 2: 总数量(UI_Item_Template)
        /// </summary>
        /// <param name="tagType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string[] ParseItemName(string tagType, string name)
        {
            string[] info = null;
            if (tagType == UITagType.UI_Item_Template)
            {
                info = new string[3];

                int numberStartIndex = name.LastIndexOf('_');
                int currentIndex = name.LastIndexOf('_', numberStartIndex - 1);

                string count = name.Substring(numberStartIndex + 1);
                string index = name.Substring(currentIndex + 1, numberStartIndex - currentIndex - 1);
                string realName = name.Substring(0, name.Length - name.Substring(currentIndex).Length);

                info[0] = realName;
                info[1] = index;
                info[2] = count;
            }
            else if (tagType == UITagType.UI_Item)
            {
                info = new string[2];

                int currentIndex = name.LastIndexOf('_');

                string index = name.Substring(currentIndex + 1);
                string realName = name.Substring(0, name.Length - name.Substring(currentIndex).Length);

                info[0] = realName;
                info[1] = index;

                return info;
            }
            return info;
        }
    }
}