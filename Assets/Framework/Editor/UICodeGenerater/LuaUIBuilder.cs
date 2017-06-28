using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace UI.Code.Generater
{
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
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                            break;
                        case TagType.UI_GameObject:
                            init.Append(string.Format(gameObjectInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case TagType.UI_Lable:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetLable()));
                            break;
                        case TagType.UI_Sprite:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSprite()));
                            break;
                        case TagType.UI_Slider:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSlider()));
                            break;
                        case TagType.UI_Texture:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetTexture()));
                            break;
                        case TagType.UI_Toggle:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetToggle()));
                            break;
                        case TagType.UI_Transform:
                            init.Append(string.Format(transformInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case TagType.UI_InputField:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetInputField()));
                            break;
                    }
                }
            }

            content = ReadTemplateString();
            content = content.Replace("{#class#}", GetClassName());
            content = content.Replace("{#init#}", init.ToString());
            SaveFile();
        }
    }
}