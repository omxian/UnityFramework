using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class Float2DText : FloatText
{
    private Text floatText;

    /// <summary>
    /// 设置2d Text 的起始位置
    /// </summary>
    public override void SetValue(/*RoleBase follower,*/ string flowText/*, BattleResBase go*/)
    {
        //if (follower != null && follower.GetEffectPos(ERolePos.e_head) != null)
        //{
        //    Vector3 headViewport = HeadBarMgr.Instance.GetReviseHeadViewport(follower);
        //    headViewport = uiCamera.ViewportToWorldPoint(headViewport);
        //    SetValue(headViewport, flowText, go);
        //}
        //else
        //{
        //    go.Return();
        //}
    }

    public override void SetValue(Vector3 position, string floatText/*, BattleResBase go*/)
    {
        if (this.floatText == null)
        {
            this.floatText = GetComponent<Text>();
        }
        this.floatText.text = floatText;

        transform.position = position;
        bloodTextPos = transform.localPosition;
        oriPos = transform.localPosition;

        //SetValue(position, go);
    }

    protected override void DoUpdate(float passTime)
    {
        if (worldCamera == null || uiCamera == null)
        {
            //if (battleRes != null)
            //{
            //    battleRes.Return();
            //    battleRes = null;
            //}
            //return;
        }

        if (passTime < _xCurveMaxTime)
        {
            bloodTextPos.x = oriPos.x + positionXCuve.Evaluate(passTime);
        }

        if (passTime < _yCurveMaxTime)
        {
            bloodTextPos.y = oriPos.y + positionYCuve.Evaluate(passTime);
        }

        transform.localPosition = bloodTextPos;

        if (passTime < _scaleCurveMaxTime)
        {
            float scale = scaleCuve.Evaluate(passTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
