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
    public UnityAction OnCarouselBtnAction = DefaultAction;
    public Button CarouselBtn;

	public override IEnumerator BindUI()
	{
		base.BindUI();
        TicTacToeBtn = transform.Find("TicTacToeBtn").GetComponent<Button>();
        TicTacToeBtn.onClick.AddListener(()=>{ OnTicTacToeBtnAction();});
        CarouselBtn = transform.Find("CarouselBtn").GetComponent<Button>();
        CarouselBtn.onClick.AddListener(()=>{ OnCarouselBtnAction();});

		yield return null;
	}

	public override void Clear()
    {
		base.Clear();
        TicTacToeBtn.onClick.RemoveAllListeners();
        CarouselBtn.onClick.RemoveAllListeners();

		Destroy(gameObject);
    }
}