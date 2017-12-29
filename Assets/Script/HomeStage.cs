using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HomeStage: StageComponent
{
    HomeView home;
    protected override void Init()
    {
        base.Init();
        CreateView<HomeView>(OnHomeShowed);
    }

    private void OnHomeShowed(HomeView view)
    {
        home = view;
        home.OnTicTacToeBtnAction = OnTTTClick;
        TriggerNotify(NotifyIds.PLAY_AUDIO, new AudioNotifyArg(true,"BGM_0")); 
    }

    private void OnTTTClick()
    {
        TriggerNotify(NotifyIds.PLAY_AUDIO, new AudioNotifyArg(true, "BGM_0"));
        //StageManager.Instance.LoadStage<TTTStage>(true);
    }

    public override void Clear()
    {
        home.Clear();
        base.Clear();
    }
}

