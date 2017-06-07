using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;

/// <summary>
/// Same with the tag define in Unity Editor
/// </summary>
public class TagType
{
    public const string Untagged = "Untagged";
    public const string UI_CSharp = "UI_CSharp";
    public const string UI_Lua = "UI_Lua";
    public const string UI_Texture = "UI_Texture";
    public const string UI_Sprite = "UI_Sprite";
    public const string UI_Lable = "UI_Lable";
    public const string UI_GameObject = "UI_GameObject";
    public const string UI_Transform = "UI_Transform";
    public const string UI_Toggle = "UI_Toggle";
    public const string UI_Button = "UI_Button";
    public const string UI_InputField = "UI_InputField";
    public const string UI_Slider = "UI_Slider";
}

/// <summary>
/// Define Component Name
/// </summary>
public abstract class ComponentInfo
{
    public abstract string GetTexture();
    public abstract string GetLable();
    public abstract string GetSprite();
    public abstract string GetButton();
    public abstract string GetToggle();
    public abstract string GetInputField();
    public abstract string GetSlider();
    public string GetGameObject()
    {
        return "GameObject";
    }

    public string GetTransform()
    {
        return "Transform";
    }
}

public class NGUIComponentInfo : ComponentInfo
{
    public override string GetButton()
    {
        return "UIButton";
    }

    public override string GetInputField()
    {
        return "UIInput";
    }

    public override string GetLable()
    {
        return "UILabel";
    }

    public override string GetSlider()
    {
        throw new NotImplementedException();
    }

    public override string GetSprite()
    {
        return "UISprite";
    }

    public override string GetTexture()
    {
        return "UITexture";
    }

    public override string GetToggle()
    {
        return "UIToggle";
    }
}

public class UGUIComponentInfo : ComponentInfo
{
    public override string GetButton()
    {
        return "Button";
    }

    public override string GetInputField()
    {
        return "InputField";
    }

    public override string GetLable()
    {
        return "Text";
    }

    public override string GetSlider()
    {
        return "Slider";
    }

    public override string GetSprite()
    {
        return "Image";
    }

    public override string GetTexture()
    {
        return "RawImage";
    }

    public override string GetToggle()
    {
        return "Toggle";
    }
}

public abstract class UIBuilder
{
    protected string content = string.Empty;
    private string savePath;
    private string templatePath;
    private Transform[] childrenTransform;
    private string fileSuffix;
    private GameObject rootGameObject = null;
    private ComponentInfo componentInfo = null;
    public UIBuilder(GameObject go)
    {
        rootGameObject = go;
        childrenTransform = go.GetComponentsInChildren<Transform>(true);
    }

    protected void SetComponentInfo(ComponentInfo c)
    {
        componentInfo = c;
    }

    protected ComponentInfo GetComponentInfo()
    {
        return componentInfo;
    }

    protected bool CheckTag(string tag)
    {
        return tag != string.Empty && tag != TagType.Untagged;
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

public class CSharpUIBuilder : UIBuilder
{
    public CSharpUIBuilder(GameObject go) : base(go)
    {
        SetSavePath("Resources/Scripts/UI/View");
        SetTemplatePath(Path.Combine(Application.dataPath, "Editor/Template/C#_UI_Template.txt"));
        SetFileSuffix(".cs");
        SetComponentInfo(new UGUIComponentInfo());
    }

    public override void GenerateUI()
    {
        string gameObjectInitTemplate = "{0} = transform.Find(\"{1}\").gameObject;\n";
        string transformInitTemplate = "{0} = transform.Find(\"{1}\");\n";
        string componentInitTemplate = "{0} = transform.Find(\"{1}\").GetComponent<{2}>();\n";
        string paramTemplate = "public {0} {1};\n";

        StringBuilder init = new StringBuilder();
        StringBuilder param = new StringBuilder();
        ComponentInfo info = GetComponentInfo();

        foreach (Transform tran in GetChildrenTransform())
        {
            string name = tran.name;
            string tag = tran.gameObject.tag;

            if (CheckTag(tag))
            {
                CheckPrefabName(name);

                if (tag == TagType.UI_Button)
                {
                    param.Append(string.Format(paramTemplate, info.GetButton(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                }
                else if (tag == TagType.UI_GameObject)
                {
                    param.Append(string.Format(paramTemplate, info.GetGameObject(), name));
                    init.Append(string.Format(gameObjectInitTemplate, name, GetHierarchy(tran)));
                }
                else if (tag == TagType.UI_Lable)
                {
                    param.Append(string.Format(paramTemplate, info.GetLable(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetLable()));
                }
                else if (tag == TagType.UI_Sprite)
                {
                    param.Append(string.Format(paramTemplate, info.GetSprite(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSprite()));
                }
                else if (tag == TagType.UI_Slider)
                {
                    param.Append(string.Format(paramTemplate, info.GetSlider(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSlider()));
                }
                else if (tag == TagType.UI_Texture)
                {
                    param.Append(string.Format(paramTemplate, info.GetTexture(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetTexture()));
                }
                else if (tag == TagType.UI_Toggle)
                {
                    param.Append(string.Format(paramTemplate, info.GetToggle(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetToggle()));
                }
                else if (tag == TagType.UI_Transform)
                {
                    param.Append(string.Format(paramTemplate, info.GetTransform(), name));
                    init.Append(string.Format(transformInitTemplate, name, GetHierarchy(tran)));
                }
                else if (tag == TagType.UI_InputField)
                {
                    param.Append(string.Format(paramTemplate, info.GetInputField(), name));
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetInputField()));
                }
            }
        }

        content = ReadTemplateString();
        content = content.Replace("{#class#}", GetClassName());
        content = content.Replace("{#param#}", param.ToString());
        content = content.Replace("{#init#}", init.ToString());
        SaveFile();
    }
}

public class LuaUIBuilder : UIBuilder
{
    public LuaUIBuilder(GameObject go) : base(go)
    {
        SetSavePath("LuaScripts/Logic/UIView");
        SetTemplatePath(Path.Combine(Application.dataPath, "Editor/Template/Lua_UI_Template.txt"));
        SetFileSuffix(".lua");
        SetComponentInfo(new UGUIComponentInfo());
    }

    public override void GenerateUI()
    {
        StringBuilder init = new StringBuilder();
        ComponentInfo info = GetComponentInfo();
        string gameObjectInitTemplate = "    o.{0} = go.transform:Find(\"{1}\").gameObject;\n";
        string transformInitTemplate = "    o.{0} = go.transform:Find(\"{1}\");\n";
        string componentInitTemplate = "    o.{0} = go.transform:Find(\"{1}\"):GetComponent(\"{2}\");\n";

        foreach (Transform tran in GetChildrenTransform())
        {
            string name = tran.name;
            string tag = tran.gameObject.tag;

            if (CheckTag(tag))
            {
                CheckPrefabName(name);

                switch (tag)
                {
                    case TagType.UI_Button:
                        break;
                }

                if (tag == TagType.UI_Button)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                }
                else if (tag == TagType.UI_GameObject)
                {
                    init.Append(string.Format(gameObjectInitTemplate, name, GetHierarchy(tran)));
                }
                else if (tag == TagType.UI_Lable)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetLable()));
                }
                else if (tag == TagType.UI_Sprite)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSprite()));
                }
                else if (tag == TagType.UI_Slider)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSlider()));
                }
                else if (tag == TagType.UI_Texture)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetTexture()));
                }
                else if (tag == TagType.UI_Toggle)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetToggle()));
                }
                else if (tag == TagType.UI_Transform)
                {
                    init.Append(string.Format(transformInitTemplate, name, GetHierarchy(tran)));
                }
                else if (tag == TagType.UI_InputField)
                {
                    init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetInputField()));
                }
            }
        }

        content = ReadTemplateString();
        content = content.Replace("{#class#}", GetClassName());
        content = content.Replace("{#init#}", init.ToString());
        SaveFile();
    }
}

public class GenUITools
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
        if (rootTag == TagType.UI_Lua)
        {
            return new LuaUIBuilder(go);
        }
        else if (rootTag == TagType.UI_CSharp)
        {
            return new CSharpUIBuilder(go);
        }
        else
        {
            throw new Exception("Please Define Prefab Tag !");
        }
    }
}
