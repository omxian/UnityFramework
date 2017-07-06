using System;
using System.Collections.Generic;

public abstract class WinCheck
{
    protected Player[] currentGameState;
    protected WinState playerState;
    protected int winIndex = -1;
    protected bool countBefore = false;

    public WinCheck(Player[] currentGameState, WinState playerState)
    {
        this.currentGameState = currentGameState;
        this.playerState = playerState;
    }

    public bool IfWin()
    {
        if (!countBefore)
        {
            WinIndex();
        }
        return !(winIndex == -1);
    }
    public abstract int WinIndex();
}

public class HorizontalWin : WinCheck
{
    public HorizontalWin(Player[] currentGameState, WinState playerState) : base(currentGameState, playerState)
    {
    }

    public override int WinIndex()
    {
        countBefore = true;

        List<int> horizontalList = playerState.GetHorizontalList();
        for (int line = 0; line < horizontalList.Count; line++)
        {
            int winLine = horizontalList[line];
            for (int j = 0; j < WinState.horizontalArr.Length; j++)
            {
                int targetIndex = WinState.horizontalArr[j] + winLine * 3;

                //位置没有被占据，返回索引
                if (currentGameState[targetIndex] == Player.None)
                {
                    winIndex = targetIndex;
                    return winIndex;
                }
            }
        }
        return winIndex;
    }
}

public class VerticalWin : WinCheck
{
    public VerticalWin(Player[] currentGameState, WinState playerState) : base(currentGameState, playerState)
    {
    }

    public override int WinIndex()
    {
        countBefore = true;

        List<int> verticalList = playerState.GetVerticalList();
        for (int column = 0; column < verticalList.Count; column++)
        {
            int winLine = verticalList[column];
            for (int j = 0; j < WinState.verticalArr.Length; j++)
            {
                int targetIndex = WinState.verticalArr[j] + winLine;

                //位置没有被占据，返回索引
                if (currentGameState[targetIndex] == Player.None)
                {
                    winIndex = targetIndex;
                    return winIndex;
                }
            }
        }
        return winIndex;
    }
}
public class LeftSlantWin : WinCheck
{
    public LeftSlantWin(Player[] currentGameState, WinState playerState) : base(currentGameState, playerState)
    {
    }

    public override int WinIndex()
    {
        countBefore = true;
        if (playerState.CheckLeftSlantWillWin())
        {
            for (int j = 0; j < WinState.leftSlantArr.Length; j++)
            {
                int targetIndex = WinState.leftSlantArr[j];

                //位置没有被占据，返回索引
                if (currentGameState[targetIndex] == Player.None)
                {
                    winIndex = targetIndex;
                    return winIndex;
                }
            }
        }
        return winIndex;
    }
}
public class RightSlantWin : WinCheck
{
    public RightSlantWin(Player[] currentGameState, WinState playerState) : base(currentGameState, playerState)
    {
    }

    public override int WinIndex()
    {
        countBefore = true;
        if (playerState.CheckRightSlantWillWin())
        {
            for (int j = 0; j < WinState.rightSlantArr.Length; j++)
            {
                int targetIndex = WinState.rightSlantArr[j];

                //位置没有被占据，返回索引
                if (currentGameState[targetIndex] == Player.None)
                {
                    winIndex = targetIndex;
                    return winIndex;
                }
            }
        }
        return winIndex;
    }
}

public class AllWinWay
{

    protected Player[] currentGameState;
    protected WinState playerState;

    public AllWinWay(Player[] currentGameState, WinState playerState)
    {
        this.currentGameState = currentGameState;
        this.playerState = playerState;
    }

    public int WinIndex()
    {
        int horizontalWinIndex = new HorizontalWin(currentGameState, playerState).WinIndex();
        if (horizontalWinIndex != -1)
        {
            return horizontalWinIndex;
        }

        int verticalWinIndex = new VerticalWin(currentGameState, playerState).WinIndex();
        if (verticalWinIndex != -1)
        {
            return verticalWinIndex;
        }

        int leftSlantWinIndex = new LeftSlantWin(currentGameState, playerState).WinIndex();
        if (leftSlantWinIndex != -1)
        {
            return leftSlantWinIndex;
        }

        int rightSlantWinIndex = new RightSlantWin(currentGameState, playerState).WinIndex();
        if (rightSlantWinIndex != -1)
        {
            return rightSlantWinIndex;
        }

        return -1;
    }
}