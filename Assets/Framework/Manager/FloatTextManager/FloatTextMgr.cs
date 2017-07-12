using UnityEngine;

/// <summary>
/// 支持的floatText类型
/// 对应FloatTextPath
/// </summary>
public enum FloatTextType
{
    None = 0,
    //对敌伤害
    Attack = 1,
    //对敌暴击
    AttackCrit =2,
    //治疗
    Treat = 3,
    //治疗暴击
    TreatCrit =4,
    //受到伤害
    Hurt = 5,
    //受到暴击
    HurtCrit = 6,
    //获得经验
    GetExp = 7,
    //对敌攻击未命中
    AttackMiss = 8,
    //受到攻击未命中（躲避）
    Dodge = 9,
}

/// <summary>
/// 飘血管理器
/// 管理全局的飘字
/// 包括掉血，补血，暴击，躲避，加经验等
/// </summary>
public class FloatTextMgr : MonoSingleton<FloatTextMgr>
{
    //Float3DText名称
    //对应FloatTextType枚举值
    private string[] FloatText3DPath = {
        "",
        "AttackDamageText",
        "AttackCritDamageText",
        "TreatText",
        "TreatCritText",
        "HurtDamageText",
        "HurtCritDamageText",
        "ExpText",
        "AttackMissText",
        "DodgeText"
    };

    //Float2DText名称
    //对应FloatTextType枚举值
    private string[] FloatText2DPath = {
        "",
        "AttackDamage2DText",
        "AttackCritDamage2DText",
        "Treat2DText",
        "TreatCrit2DText",
        "HurtDamage2DText",
        "HurtCritDamage2DText",
        "Exp2DText",
        "AttackMiss2DText",
        "Dodge2DText"
    };

    private string GetDisplayByFloatTextType(FloatTextType type, string value = "")
    {
        //Match with bmfont
        string result = "";
        switch (type)
        {
            case FloatTextType.Attack:
            case FloatTextType.Hurt:
            case FloatTextType.Treat:
            case FloatTextType.TreatCrit:
                result = value;
                break;
            case FloatTextType.AttackCrit:
            case FloatTextType.HurtCrit:
                result = "~" + value; 
                break;
            case FloatTextType.GetExp:
                result = "jy+" + value;
                break;
            case FloatTextType.Dodge:
                result = "sb";
                break;
            case FloatTextType.AttackMiss:
                result= "wmz";
                break;
        }
        return result;
    }
    
    /// <summary>
    /// 转换成可支持的类型;
    /// </summary>
    private FloatTextType ConvertType(/*需要定义*/)
    {
        FloatTextType value = FloatTextType.None;
        //转换FloatTextType
        return value;
    }

    //Canvas Transform
    private Transform floatTextCanvas = null;

    //根据类型转换
    public void SetFloatText(/*HitResultMacro type, RoleBase role,*/ string value = "")
    {
        FloatTextType floatTextType = ConvertType();

        bool is2D = true;
        if (is2D)
        {
            if (floatTextCanvas == null)
            {
                //载入Canvas
                //floatTextCanvas = UIManager.Instance.OpenUI("uiprefabs_fightui.unity3d", "FloatTextCanvas").transform;
            }

            //使用战斗资源管理器 load 2d prefab
            //BattleResBase go = BattleResPoolMgr.Instance.GetEffect("prefabs_other_blood.unity3d", FloatText2DPath[(int)floatTextType]);
            //go.m_rootObj.transform.SetParent(floatTextCanvas, false);
            //FloatText bt = go.m_resModel.GetComponent<Float2DText>();
            //bt.SetValue(role, GetDisplayByFloatTextType(floatTextType, value), go);
        }
        else
        {
            //使用战斗资源管理器 load 3d prefab
            //BattleResBase go = BattleResPoolMgr.Instance.GetEffect("prefabs_other_blood.unity3d", FloatText3DPath[(int)floatTextType]);
            //FloatText bt = go.m_resModel.GetComponent<Float3DText>();
            //bt.SetValue(role, GetDisplayByFloatTextType(floatTextType, value), go);
        }
    }
}
