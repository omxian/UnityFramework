using System;
using UnityEngine;

public class Float3DText : FloatText
{
    private TextMesh m_tMesh;

    public override void SetValue(/*RoleBase follower,*/ string flowText/*, BattleResBase go*/)
    {
        //if (follower != null)
        //{
        //    Vector3 headViewport = HeadBarMgr.Instance.GetReviseWorldPosition(follower);
        //    SetValue(headViewport, flowText, go);
        //}
        //else
        //{
        //    go.Return();
        //}
    }

    public override void SetValue(Vector3 position, string floatText/*, BattleResBase go*/)
    {
        if (m_tMesh == null)
        {
            m_tMesh = GetComponentInChildren<TextMesh>();
        }
        m_tMesh.text = floatText;

        position = RandomPosition(position, -0.5f, 0.5f, 0, 0.5f);
        bloodTextPos = position;
        transform.position = position;
        oriPos = bloodTextPos;

        //SetValue(position,go);
        transform.forward = worldCamera.transform.forward;
    }

    protected override void DoUpdate(float passTime)
    {
        if (worldCamera == null)
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

        transform.position = bloodTextPos;
        transform.forward = worldCamera.transform.forward;

        if (passTime < _scaleCurveMaxTime)
        {
            float scale = scaleCuve.Evaluate(passTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}