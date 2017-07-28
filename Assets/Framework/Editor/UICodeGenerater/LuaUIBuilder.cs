using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace Unity.Framework.Editor
{
    public class LuaUIBuilder : UIBuilder
    {
        public LuaUIBuilder(GameObject go) : base(go)
        {
            SetSavePath("LuaScripts/Logic/UIView");
            SetTemplatePath(Path.Combine(Application.dataPath, "Framework/Editor/Template/Lua_UI_Template.txt"));
            SetFileSuffix(".lua");
            SetComponentInfo(new UGUIComponentInfo());
        }

        public override void GenerateUI()
        {
            StringBuilder init = new StringBuilder();
            StringBuilder requireInfo = new StringBuilder();
            BaseComponentInfo info = GetComponentInfo();
            string gameObjectInitTemplate = "    o.{0} = go.transform:Find(\"{1}\").gameObject;\n";
            string transformInitTemplate = "    o.{0} = go.transform:Find(\"{1}\");\n";
            string componentInitTemplate = "    o.{0} = go.transform:Find(\"{1}\"):GetComponent(\"{2}\");\n";

            foreach (Transform tran in GetChildrenTransform())
            {
                string name = tran.name;
                string tag = tran.gameObject.tag;

                if (CheckTag(tag))
                {
                    if (CheckNeedToSkip(skipParentTransform, tran))
                    {
                        continue;
                    }

                    CheckPrefabName(name);

                    switch (tag)
                    {
                        case UITagType.UI_Button:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                            break;
                        case UITagType.UI_GameObject:
                            init.Append(string.Format(gameObjectInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case UITagType.UI_Lable:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetLable()));
                            break;
                        case UITagType.UI_Sprite:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSprite()));
                            break;
                        case UITagType.UI_Slider:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetSlider()));
                            break;
                        case UITagType.UI_Texture:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetTexture()));
                            break;
                        case UITagType.UI_Toggle:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetToggle()));
                            break;
                        case UITagType.UI_Transform:
                            init.Append(string.Format(transformInitTemplate, name, GetHierarchy(tran)));
                            break;
                        case UITagType.UI_InputField:
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetInputField()));
                            break;
                        case UITagType.UI_Item_Template:
                            if (tran.gameObject == GetRootGameObject())
                            {
                                renameFile = ParseItemName(tag, name)[0];
                                continue;
                            }

                            string[] nameInfo = ParseItemName(tag, name);
                            string className = GetClassName(nameInfo[0]);
                            string tableName = nameInfo[0] + "Table";

                            //将定义放置最前
                            string temp = string.Format("o.{0} = {1}; \n", tableName, "{}") + init.ToString();
                            init = new StringBuilder(temp);
                            init.Append(string.Format("o.{0}[{1}] = {2}:BindUI(go.transform:Find(\"{3}\").gameObject);\n", tableName, nameInfo[1], className, GetHierarchy(tran)));
                            //添加依赖
                            requireInfo.Append(string.Format("require \"UIView.{0}\"", className));
                            //将transform加入跳过列表
                            skipParentTransform.Add(tran);
                            //开始生成Item Class
                            UICodeGenerater.BuildViewClass(UICodeGenerater.BuildClassFactory(tran.gameObject, UITagType.UI_Lua));
                            break;
                        case UITagType.UI_Item:
                            string[] itemNameInfo = ParseItemName(tag, name);
                            string itemClassName = GetClassName(itemNameInfo[0]);
                            string itemTableName = itemNameInfo[0] + "Table";
                            //命名方式 类名_当前第几个(base0);
                            init.Append(string.Format("o.{0}[{1}] = {2}:BindUI(go.transform:Find(\"{3}\").gameObject);\n", itemTableName, itemNameInfo[1], itemClassName, GetHierarchy(tran)));
                            break;
                    }
                }
            }

            content = ReadTemplateString();
            content = content.Replace("{#class#}", GetClassName(renameFile));
            content = content.Replace("{#init#}", init.ToString());
            content = content.Replace("{#require#}", requireInfo.ToString());
            SaveFile(renameFile);
        }
    }
}