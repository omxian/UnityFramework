using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Framework;
using Game.TTT;
using UnityEngine;
public class TTTStage : StageComponent
{
    private TTTView view;
    private List<UISelectable> clickAbleList;
    private Player currentPlayer;
    private Game.TTT.PlayMode currentPlayMode;
    private WinState playerOneWinState;
    private WinState playerTwoWinState;
    private GameObject playerOneGo;
    private GameObject playerTwoGo;
    private TicTacToeAI singlePlayerAI;
    private Player[] gameState;
    private bool aiTurn = false;
    private bool gameover;

    protected override void Init()
    {
        base.Init();
        CreateView<TTTView>(OnTTTShowed);
    }

    private void OnTTTShowed(TTTView view)
    {
        this.view = view;

        clickAbleList = new List<UISelectable>();
        currentPlayer = Player.Player1;

        InitGameArea();

        // Init Button
        view.OnePlayerButton.AddClick(EnterOnePlayerMode);
        view.TwoPlayerButton.AddClick(EnterTwoPlayerMode);
        view.BackButton.AddClick(OnBackClick);
        // Init GamePanel
        InitGamePanel();
    }

    //初始化
    private void InitGamePanel()
    {
        currentPlayer = Player.Player1;
        InitPlayerWinState();
        InitGameAreaObject();
        InitGameBoard();
        InitGameAreaClickEvent();
    }

    //初始化WinState
    private void InitPlayerWinState()
    {
        gameover = false;
        playerOneWinState = new WinState();
        playerTwoWinState = new WinState();
    }

    //当前是不是玩家1
    private bool IfCurrentPlayerIsOne()
    {
        return currentPlayer == Player.Player1;
    }

    //获得当前WinState
    private WinState GetCurrentWinState()
    {
        if (IfCurrentPlayerIsOne())
        {
            return playerOneWinState;
        }
        else
        {
            return playerTwoWinState;
        }
    }

    //初始化判断数组
    private void InitGameBoard()
    {
        gameState = new Player[] {
            Player.None, Player.None, Player.None,
            Player.None, Player.None, Player.None,
            Player.None, Player.None, Player.None
        };
    }

    //为可点击区域添加点击组件
    private void InitGameArea()
    {
        for (int i = 0; i < 9; i++)
        {
            Transform gameArea = view.GameBG.Find("GameArea" + i.ToString());
            UISelectable gameAreaComponent = gameArea.GetComponent<UISelectable>();
            clickAbleList.Add(gameAreaComponent);
        }
    }

    //清除游戏区域的UI
    private void ClearGameArea()
    {
        for (int i = 0; i < clickAbleList.Count; i++)
        {
            UITools.DestoryAllChildren(clickAbleList[i].transform);
        }
    }

    //初始化点击事件
    private void InitGameAreaClickEvent()
    {
        for (int i = 0; i < clickAbleList.Count; i++)
        {
            UISelectable selectable = clickAbleList[i];
            int index = i;

            selectable.OnClick = () =>
            {
                OnGameAreaClick(index);
            };
        }
    }

    private void OnGameAreaClick(int index)
    {
        //空白处
        if (gameState[index] == Player.None && !gameover)
        {
            gameState[index] = currentPlayer;
            GetCurrentWinState().AddStep(index);
            AddGameAreaImage(index);

            if (GetCurrentWinState().Win())
            {
                //当前玩家胜利，停止游戏
                gameover = true;
                view.WinText.text = currentPlayer.ToString() + " Win!";
                ShowEndUI();
            }
            else // 切换玩家，继续游戏
            {
                if (CheckGameOver())
                {
                    view.WinText.text = "No Winer";
                    ShowEndUI();
                }
                else
                {
                    SwitchPlayer();
                    if (currentPlayMode == Game.TTT.PlayMode.OnePlayerMode && aiTurn == false)
                    {
                        aiTurn = true;
                        OnGameAreaClick(singlePlayerAI.NextStep(gameState));
                    }
                    else
                    {
                        aiTurn = false;
                    }
                }
            }
        }
    }

    //棋盘是不是没有位置再下
    private bool CheckGameOver()
    {
        for (int i = 0; i < gameState.Length; i++)
        {
            if (gameState[i] == Player.None)
            {
                gameover = false;
                return gameover;
            }
        }
        gameover = true;
        return gameover;
    }

    //缓存UI游戏对象
    private void InitGameAreaObject()
    {
        playerOneGo = ResourceManager.Instance.LoadPrefab("PlayerOneSelect", "TicTacToe");
        playerTwoGo = ResourceManager.Instance.LoadPrefab("PlayerTwoSelect", "TicTacToe");
    }

    //返回当前玩家的游戏对象
    private GameObject GetCurrentGo()
    {
        if (IfCurrentPlayerIsOne())
        {
            return (GameObject)Instantiate(playerOneGo);
        }
        else
        {
            return (GameObject)Instantiate(playerTwoGo);
        }
    }

    //添加游戏UI
    private void AddGameAreaImage(int index)
    {
        GetCurrentGo().transform.SetParent(clickAbleList[index].transform, false);
    }

    public void EnterOnePlayerMode()
    {
        StartGame(Game.TTT.PlayMode.OnePlayerMode);
    }

    public void EnterTwoPlayerMode()
    {
        StartGame(Game.TTT.PlayMode.TwoPlayerMode);
    }

    public void HideEndUI()
    {
        view.WinText.text = "";
        view.BackButton.SetActive(false);
    }

    public void ShowEndUI()
    {
        view.WinText.gameObject.SetActive(true);
        view.BackButton.SetActive(true);
    }

    //返回按钮点击
    public void OnBackClick()
    {
        ClearGameArea();
        InitGamePanel();
        view.StartPanel.gameObject.SetActive(true);
        view.GamePanel.gameObject.SetActive(false);
        HideEndUI();
    }

    //开始游戏
    public void StartGame(Game.TTT.PlayMode mode)
    {
        currentPlayMode = mode;
        view.StartPanel.gameObject.SetActive(false);
        view.GamePanel.gameObject.SetActive(true);
        if (mode == Game.TTT.PlayMode.OnePlayerMode)
        {
            aiTurn = !view.OffensiveToggle.isOn;
            if (aiTurn)
            {
                singlePlayerAI = new TicTacToeAI(Player.Player1);
                OnGameAreaClick(singlePlayerAI.NextStep(gameState));
            }
            else
            {
                singlePlayerAI = new TicTacToeAI(Player.Player2);
            }
        }
    }

    //切换玩家，一个玩家下完之后触发这个方法
    private void SwitchPlayer()
    {
        if (IfCurrentPlayerIsOne())
        {
            currentPlayer = Player.Player2;
        }
        else
        {
            currentPlayer = Player.Player1;
        }
    }
}
