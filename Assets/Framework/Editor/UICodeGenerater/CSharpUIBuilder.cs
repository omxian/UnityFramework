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
            SetTemplatePath(Path.Combine(Application.dataPath, "Framework/Editor/Template/C#_UI_Template.txt"));
            SetFileSuffix(".cs");
            SetComponentInfo(new UGUIComponentInfo());
        }

        public override void GenerateUI()
        {
            string gameObjectInitTemplate = "        {0} = transform.Find(\"{1}\").gameObject;\r\n";
            string transformInitTemplate = "        {0} = transform.Find(\"{1}\");\r\n";
            string componentInitTemplate = "        {0} = transform.Find(\"{1}\").GetComponent<{2}>();\r\n";
            string paramTemplate = "    public {0} {1};\r\n";

            StringBuilder init = new StringBuilder();
            StringBuilder param = new StringBuilder();
            StringBuilder clear = new StringBuilder();
            BaseComponentInfo info = GetComponentInfo();

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
                            string actionName = name;
                            if(actionName.Length > 1)
                            {
                                actionName = "On" + name[0].ToString().ToUpper() + name.Substring(1, name.Length - 1) + "Action";
                            }
                            else
                            {
                                actionName = "On" + name[0].ToString().ToUpper() + "Action";
                            }
                            
                            param.Append(string.Format(paramTemplate, "UnityAction", actionName + " = DefaultAction"));
                            param.Append(string.Format(paramTemplate, info.GetButton(), name));
                            init.Append(string.Format(componentInitTemplate, name, GetHierarchy(tran), info.GetButton()));
                            init.Append(string.Format ("        {0}.onClick.AddListener(()=>{{ {1}();}});\r\n", name, actionName));
                            clear.Append(string.Format("        {0}.onClick.RemoveAllListeners();\r\n", name));
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
                        case UITagType.UI_Item_Template:
                            string[] nameInfo = ParseItemName(tag, name);
                            if (tran.gameObject == GetRootGameObject())
                            {
                                renameFile = ParseItemName(tag, name)[0];
                                continue;
                            }

                            string className = GetClassName(nameInfo[0]);
                            string arrayName = nameInfo[0] + "Array";

                            //定义Array
                            param.Append(string.Format("    public {0}[] {1} = new {0}[{2}];\r\n", className, arrayName, nameInfo[2]));
                            init.Append(string.Format("        {0}[{1}] = transform.Find(\"{3}\").gameObject.AddComponent<{2}>();\r\n", arrayName, nameInfo[1], className, GetHierarchy(tran)));

                            //将transform加入跳过列表
                            skipParentTransform.Add(tran);
                            //开始生成
                            UICodeGenerater.BuildViewClass(UICodeGenerater.BuildClassFactory(tran.gameObject, UITagType.UI_CSharp));
                            break;
                        case UITagType.UI_Item:
                            string[] itemNameInfo = ParseItemName(tag, name);
                            string itemClassName = GetClassName(itemNameInfo[0]);
                            string itemArrayName = itemNameInfo[0] + "Array";
                            init.Append(string.Format("        {0}[{1}] = transform.Find(\"{3}\").gameObject.AddComponent<{2}>();\r\n", itemArrayName, itemNameInfo[1], itemClassName, GetHierarchy(tran)));
                            break;
                    }
                }
            }

            content = ReadTemplateString();
            content = content.Replace("{#class#}", GetClassName(renameFile));
            content = content.Replace("{#param#}", param.ToString());
            content = content.Replace("{#init#}", init.ToString());
            content = content.Replace("{#clear#}", clear.ToString());
            SaveFile(renameFile);
        }
    }
}