﻿using System;
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
    }

    private void OnTTTClick()
    {
        StageManager.Instance.LoadStage<TTTStage>(true);
    }
}

