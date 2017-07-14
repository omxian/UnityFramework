using System.IO;
using System.Text;
using UnityEngine;

namespace Unity.Framework.Editor
{
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
            BaseComponentInfo info = GetComponentInfo();

            foreach (Transform tran in GetChildrenTransform())
            {
                string name = tran.name;
                string tag = tran.gameObject.tag;

                if (CheckTag(tag))
                {
                    CheckPrefabName(name);

                    switch (tag)
                    {
                        case UITagType.UI_Button:
                            param.Append(string.Format(paramTemplate, info.GetButton(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                            break;
                        case UITagType.UI_GameObject:
                            param.Append(string.Format(paramTemplate, info.GetGameObject(), name));
                            init.Append(string.Format(gameObjectInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case UITagType.UI_Lable:
                            param.Append(string.Format(paramTemplate, info.GetLable(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetLable()));
                            break;
                        case UITagType.UI_Sprite:
                            param.Append(string.Format(paramTemplate, info.GetSprite(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSprite()));
                            break;
                        case UITagType.UI_Slider:
                            param.Append(string.Format(paramTemplate, info.GetSlider(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSlider()));
                            break;
                        case UITagType.UI_Texture:
                            param.Append(string.Format(paramTemplate, info.GetTexture(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetTexture()));
                            break;
                        case UITagType.UI_Toggle:
                            param.Append(string.Format(paramTemplate, info.GetToggle(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetToggle()));
                            break;
                        case UITagType.UI_Transform:
                            param.Append(string.Format(paramTemplate, info.GetTransform(), name));
                            init.Append(string.Format(transformInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case UITagType.UI_InputField:
                            param.Append(string.Format(paramTemplate, info.GetInputField(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetInputField()));
                            break;
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
}