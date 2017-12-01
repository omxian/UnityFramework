/* 
	UI View Gen From GenUITools
	Please Don't Modify!
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestUIView : ViewComponent
{
public UnityAction OntttBtnAction = DefaultAction;
public Button tttBtn;
public UnityAction OnmlBtnAction = DefaultAction;
public Button mlBtn;

	public override void BindUI()
	{
tttBtn = transform.Find("tttBtn").GetComponent<Button>();
tttBtn.onClick.AddListener(OntttBtnAction);
mlBtn = transform.Find("mlBtn").GetComponent<Button>();
mlBtn.onClick.AddListener(OnmlBtnAction);

	}
}