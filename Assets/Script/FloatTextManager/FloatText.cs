using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Float Text 基类
/// </summary>
public abstract class FloatText : MonoBehaviour
{
    [SerializeField]
    protected AnimationCurve positionXCuve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    [SerializeField]
    protected AnimationCurve positionYCuve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    [SerializeField]
    protected AnimationCurve scaleCuve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });

    protected float _xCurveMaxTime;
    protected float _yCurveMaxTime;
    protected float _scaleCurveMaxTime;

    private float staytime = 1f;
    //资源管理器
    //protected BattleResBase battleRes;

    protected static Camera worldCamera;
    protected static Camera uiCamera;

    private float m_fBeginTime;

    protected Vector3 oriPos;
    protected Vector3 bloodTextPos;

    #region Use For Test
    protected bool testModel;
    [SerializeField]
    protected string testString;
    [SerializeField]
    protected Transform testTransform;
    [SerializeField]
    protected float testYAxisOffset;
    [SerializeField]
    protected Camera testWorldCamera;
    [SerializeField]
    protected Camera testUICamera;
    #endregion
    protected abstract void DoUpdate(float passTime);
    public abstract void SetValue(Vector3 position, string flowText/*, BattleResBase go*/);
    public abstract void SetValue(/*RoleBase follower,*/ string flowText/*, BattleResBase go*/);

    public Vector3 RandomPosition(Vector3 originPosition,float minX,float maxX, float minY,float maxY)
    {
        Vector3 randomPosition = originPosition;
        randomPosition.x += Random.Range(minX, maxX);
        randomPosition.y += Random.Range(minY, maxY);
        return randomPosition;
    }

    private void Awake()
    {
        //摄像机的赋值 ，如果是测试模式 就使用自己拖的摄像机，如果是框架环境则使用框架摄像机
        //if (worldCamera == null)
        //{
        //    if (CameraMgr.Instance != null && CameraMgr.Instance.mainCamera != null)
        //    {
        //        worldCamera = CameraMgr.Instance.mainCamera;
        //    }
        //}

        //if (uiCamera == null)
        //{
        //    if (CGameRoot.Instance != null && CGameRoot.Instance.uiCamera != null)
        //    {
        //        uiCamera = CGameRoot.Instance.uiCamera;
        //    }
        //}
    }

    public virtual void SetValue(Vector3 position/*, BattleResBase go*/)
    {
        //battleRes = go;
        _xCurveMaxTime = positionXCuve[positionXCuve.length - 1].time;
        _yCurveMaxTime = positionYCuve[positionYCuve.length - 1].time;
        _scaleCurveMaxTime = scaleCuve[scaleCuve.length - 1].time;
        staytime = Mathf.Max(_xCurveMaxTime, _yCurveMaxTime, _scaleCurveMaxTime);

        float scale = scaleCuve.Evaluate(0);
        transform.localScale = new Vector3(scale, scale, scale);

        //判断是否测试模式
        testModel = false /*!CUility.HasFrameworkEnvironment()*/;

        m_fBeginTime = Time.time;

        if (testModel)
        {
            gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        if (testModel)
        {
            worldCamera = testWorldCamera;
            uiCamera = testUICamera;
            SetValue(testTransform.position + new Vector3(0f, testYAxisOffset), testString/*, battleRes*/);
        }
    }

    void Update()
    {
        float timer = Time.time - m_fBeginTime;

        DoUpdate(timer);

        if (timer >= staytime)
        {
            transform.position = Vector3.zero;

            if (testModel)
            {
                gameObject.SetActive(false);
            }
            //if (battleRes != null)
            //{
            //    battleRes.Return();
            //    battleRes = null;
            //}
        }
    }
}
