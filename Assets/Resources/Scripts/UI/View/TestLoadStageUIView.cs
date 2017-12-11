/* 
	UI View Gen From GenUITools
	Please Don't Modify!
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class TestLoadStageUIView : ViewComponent
{
    public UnityAction OnLoadAnotherBtnAction = DefaultAction;
    public Button LoadAnotherBtn;
    public UnityAction OnClearAllLoadBtnAction = DefaultAction;
    public Button ClearAllLoadBtn;
    public UnityAction OnClearCurrentBtnAction = DefaultAction;
    public Button ClearCurrentBtn;

	public override IEnumerator BindUI()
	{
		base.BindUI();
        LoadAnotherBtn = transform.Find("LoadAnotherBtn").GetComponent<Button>();
        LoadAnotherBtn.onClick.AddListener(OnLoadAnotherBtnAction);
        ClearAllLoadBtn = transform.Find("ClearAllLoadBtn").GetComponent<Button>();
        ClearAllLoadBtn.onClick.AddListener(OnClearAllLoadBtnAction);
        ClearCurrentBtn = transform.Find("ClearCurrentBtn").GetComponent<Button>();
        ClearCurrentBtn.onClick.AddListener(OnClearCurrentBtnAction);

		yield return null;
	}

	public override void Clear()
    {
		base.Clear();
        LoadAnotherBtn.onClick.RemoveAllListeners();
        ClearAllLoadBtn.onClick.RemoveAllListeners();
        ClearCurrentBtn.onClick.RemoveAllListeners();

		Destroy(gameObject);
    }
}