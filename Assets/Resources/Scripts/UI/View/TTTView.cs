/* 
	UI View Gen From GenUITools
	Please Don't Modify!
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class TTTView : ViewComponent
{
    public GameObject GamePanel;
    public Text WinText;
    public Transform GameBG;
    public UnityAction OnBackButtonAction = DefaultAction;
    public Button BackButton;
    public GameObject StartPanel;
    public UnityAction OnOnePlayerButtonAction = DefaultAction;
    public Button OnePlayerButton;
    public UnityAction OnTwoPlayerButtonAction = DefaultAction;
    public Button TwoPlayerButton;
    public Toggle OffensiveToggle;

	public override IEnumerator BindUI()
	{
		base.BindUI();
        GamePanel = transform.Find("GamePanel").gameObject;
        WinText = transform.Find("GamePanel/WinText").GetComponent<Text>();
        GameBG = transform.Find("GamePanel/GameBG");
        BackButton = transform.Find("GamePanel/BackButton").GetComponent<Button>();
        BackButton.onClick.AddListener(()=>{ OnBackButtonAction();});
        StartPanel = transform.Find("StartPanel").gameObject;
        OnePlayerButton = transform.Find("StartPanel/OnePlayerButton").GetComponent<Button>();
        OnePlayerButton.onClick.AddListener(()=>{ OnOnePlayerButtonAction();});
        TwoPlayerButton = transform.Find("StartPanel/TwoPlayerButton").GetComponent<Button>();
        TwoPlayerButton.onClick.AddListener(()=>{ OnTwoPlayerButtonAction();});
        OffensiveToggle = transform.Find("StartPanel/OffensiveToggle").GetComponent<Toggle>();

		yield return null;
	}

	public override void Clear()
    {
		base.Clear();
        BackButton.onClick.RemoveAllListeners();
        OnePlayerButton.onClick.RemoveAllListeners();
        TwoPlayerButton.onClick.RemoveAllListeners();

		Destroy(gameObject);
    }
}