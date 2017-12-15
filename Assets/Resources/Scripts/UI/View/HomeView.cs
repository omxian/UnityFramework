/* 
	UI View Gen From GenUITools
	Please Don't Modify!
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class HomeView : ViewComponent
{
    public UnityAction OnTicTacToeBtnAction = DefaultAction;
    public Button TicTacToeBtn;

	public override IEnumerator BindUI()
	{
		base.BindUI();
        TicTacToeBtn = transform.Find("TicTacToeBtn").GetComponent<Button>();
        TicTacToeBtn.onClick.AddListener(()=>{ OnTicTacToeBtnAction();});

		yield return null;
	}

	public override void Clear()
    {
		base.Clear();
        TicTacToeBtn.onClick.RemoveAllListeners();

		Destroy(gameObject);
    }
}