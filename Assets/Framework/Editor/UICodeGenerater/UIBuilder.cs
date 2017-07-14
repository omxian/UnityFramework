using System;
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
        protected virtual string GetClassName()
        {
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
        protected void SaveFile()
        {
            string genPath = Path.Combine(Application.dataPath, GetSavePath());
            string fileName = Path.Combine(genPath, GetClassName() + fileSuffix);

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
    }
}