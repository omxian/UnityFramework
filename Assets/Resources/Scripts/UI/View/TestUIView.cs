/* 
	UI View Gen From GenUITools
	Please Don't Modify!
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestUIView : ViewComponent
{
public UnityAction OnTttBtnAction = DefaultAction;
public Button tttBtn;
public UnityAction OnBAction = DefaultAction;
public Button b;

	public override void BindUI()
	{
tttBtn = transform.Find("tttBtn").GetComponent<Button>();
tttBtn.onClick.AddListener(OnTttBtnAction);
b = transform.Find("b").GetComponent<Button>();
b.onClick.AddListener(OnBAction);

	}
}