using System.Collections.Generic;
using System;
using UnityEngine;
public class TicTacToeAI
{
    private Player currentPlayer = Player.None;
    private Player opponentPlayer = Player.None;
    private Player[] currentGameState;
    
    //初始化设置对手和自己
    public TicTacToeAI(Player player)
    {
        currentPlayer = player;
        if (Player.Player1 == player)
        {
            opponentPlayer = Player.Player2;
        }
        else
        {
            opponentPlayer = Player.Player1;
        }
    }

    //返回AI下一步
    public int NextStep(Player[] gameState)
    {
        currentGameState = gameState;

        WinState currentWinState = new WinState();
        currentWinState.AddGameState(gameState, currentPlayer);

        WinState opponentWinState = new WinState();
        opponentWinState.AddGameState(gameState, opponentPlayer);

        //检查自己能否三连
        int playerWinIndex = new AllWinWay(currentGameState, currentWinState).WinIndex();
        if (playerWinIndex != -1)
        {
            return playerWinIndex;
        }

        //检查对手能否三连
        int opponentWinIndex = new AllWinWay(currentGameState, opponentWinState).WinIndex();
        if (opponentWinIndex != -1)
        {
            return opponentWinIndex;
        }

        //先手，下角位
        if (OnTheOffensive())
        {
            return 0;
        }

        //后手，抢中否则角位
        if (OnTheDefensive())
        {
            return DefensivePosition();
        }

        //简单延伸
        int extendInde = Extend(currentWinState);
        if (extendInde != -1)
        {
            return extendInde;
        }

        //填满其它区域
        int fillIndex = Fill();
        if (fillIndex != -1)
        {
            return fillIndex;
        }

        //不应该走到这里，走到这里的话抛出异常
        throw new Exception("Unexpected Error!");
    }

    //检查是否开局先手Offensive
    private bool OnTheOffensive()
    {
        for (int i = 0; i < currentGameState.Length; i++)
        {
            if (currentGameState[i] != Player.None)
            {
                return false;
            }
        }
        return true;
    }

    //检查是否后手开局Defensive
    //遍历棋盘有两子以上就不是后手
    private bool OnTheDefensive()
    {
        int count = 0;
        for (int i = 0; i < currentGameState.Length; i++)
        {
            if (currentGameState[i] != Player.None)
            {
                count++;
            }
            if (count >= 2)
            {
                return false;
            }
        }
        return true;
    }

    private int GetOppentPlayerFirstStep()
    {
        for (int i = 0; i < currentGameState.Length; i++)
        {
            if (currentGameState[i] != Player.None)
            {
                return i;
            }
        }
        return -1;
    }

    //对角位
    private int DefensivePosition()
    {
        int oppentPlayerPosition = GetOppentPlayerFirstStep();
        if (oppentPlayerPosition == WinState.centerPosition)
        {
            return 0;
        }
        else
        {
            return WinState.centerPosition;
        }
    }

    //简单延长做3,行列数尽量相差大
    private int Extend(WinState winState)
    {
        //中间没人占就占中间
        if (currentGameState[WinState.centerPosition] == Player.None)
        {
            return WinState.centerPosition;
        }

        for (int i = 0; i < 3; i++)
        {
            //第n列
            List<int> positionList = winState.verticalWin[i];
            if (positionList.Count == 1)
            {
                //检查是否有对方的存在
                int target = GetVerticalTargetPosition(i, positionList[0]);
                if (target != -1)
                {
                    return target;
                }
            }

            //第n行
            positionList = winState.horizontalWin[i];
            if (positionList.Count == 1)
            {
                //是否有对方的存在
                int target = GetHorizontalTargetPosition(i, positionList[0]);
                if (target != -1)
                {
                    return target;
                }
            }
        }
        

        return -1;
    }

    //这一列是否有胜利的可能，有的话返回索引，否则返回-1
    private int GetVerticalTargetPosition(int column, int currentIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentGameState[WinState.verticalArr[i] + column] == opponentPlayer)
            {
                return -1;
            }
        }

        int currentIndexLine = currentIndex / 3;

        //在中间，返回靠近的索引
        if (currentIndexLine == 1)
        {
            //currentIndex + 3 or currentIndex - 3
            return currentIndex + 3;
        }
        //在角落，返回远的索引
        else
        {
            if (currentIndexLine == 0)
            {
                return currentIndex + 6;
            }
            else
            {
                return currentIndex - 6;
            }
        }
    }

    //这一列是否有胜利的可能，有的话返回索引，否则返回-1
    private int GetHorizontalTargetPosition(int line, int currentIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentGameState[WinState.horizontalArr[i] + (line * 3)] == opponentPlayer)
            {
                return -1;
            }
        }

        int currentIndexColumn = currentIndex % 3;

        //在中间，返回靠近的索引
        if (currentIndexColumn == 1)
        {
            //currentIndex + 1 or currentIndex - 1
            return currentIndex + 1;
        }
        //在角落，返回远的索引
        else
        {
            if (currentIndexColumn == 0)
            {
                return currentIndex + 2;
            }
            else
            {
                return currentIndex - 2;
            }
        }
    }

    //和局，填满
    private int Fill()
    {
        for (int i = 0; i < currentGameState.Length; i++)
        {
            if (currentGameState[i] == Player.None)
            {
                return i;
            }
        }
        return -1;
    }
}
