using System.Collections.Generic;
using Game.TTT;
/// <summary>
/// 在这个类中，不考虑棋盘情况，只考虑占据了什么位置,以及胜负。
/// </summary>
public class WinState
{
    public List<List<int>> verticalWin;
    public List<List<int>> horizontalWin;
    private List<int> leftSlantWin;
    private List<int> rightSlantWin;

    private static int willWinNumber = 2;
    public static int centerPosition = 4;
    public static int[] cornerArr = new int[] { 0, 2, 6, 8 };
    public static int[] sideArr = new int[] { 1, 3, 5, 7 };
    public static int[] rightSlantArr = new int[] { 0, 4, 8 };
    public static int[] leftSlantArr = new int[] { 2, 4, 6 };
    //+n代表第n列 base0
    public static int[] verticalArr = new int[] { 0, 3, 6 };
    //+n*3代表第几行 base0
    public static int[] horizontalArr = new int[] { 0, 1, 2 };

    public WinState()
    {
        Init();
    }

    public void Init()
    {
        verticalWin = new List<List<int>>();
        verticalWin.Add(new List<int>());
        verticalWin.Add(new List<int>());
        verticalWin.Add(new List<int>());

        horizontalWin = new List<List<int>>();
        horizontalWin.Add(new List<int>());
        horizontalWin.Add(new List<int>());
        horizontalWin.Add(new List<int>());

        leftSlantWin = new List<int>();
        rightSlantWin = new List<int>();
    }

    public void Reset()
    {
        verticalWin.Clear();
        horizontalWin.Clear();
        leftSlantWin.Clear();
        rightSlantWin.Clear();
    }

    public void AddGameState(Player[] gameState, Player player)
    {
        for (int i = 0; i < gameState.Length; i++)
        {
            if (gameState[i] == player)
            {
                AddStep(i);
            }
        }
    }

    /// <summary>
    /// Add Player Action
    /// </summary>
    /// <param name="line">base 0</param>
    /// <param name="column">base 0</param>
    public void AddStep(int index)
    {
        int line = index / 3;
        int column = index % 3;

        // 垂直胜利点
        verticalWin[column].Add(index);

        // 水平胜利点
        horizontalWin[line].Add(index);

        // 左斜胜利点 (0,2),(1,1),(2,0)
        if (LeftSlant(index))
        {
            leftSlantWin.Add(index);
        }

        // 右斜胜利点 (0,0),(1,1),(2,2)
        if (RightSlant(index))
        {
            rightSlantWin.Add(1);
        }
    }

    //返回竖直方向的列表
    public List<int> GetVerticalList()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < verticalWin.Count; i++)
        {
            if (verticalWin[i].Count == willWinNumber)
            {
                result.Add(i);
            }
        }
        return result;
    }

    //返回水平方向的列表
    public List<int> GetHorizontalList()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < horizontalWin.Count; i++)
        {
            if (horizontalWin[i].Count == willWinNumber)
            {
                result.Add(i);
            }
        }
        return result;
    }

    //检查左斜方向是否将要胜利，2个
    public bool CheckLeftSlantWillWin()
    {
        return leftSlantWin.Count == willWinNumber;
    }

    //检查右斜方向是否将要胜利，2个
    public bool CheckRightSlantWillWin()
    {
        return rightSlantWin.Count == willWinNumber;
    }

    public bool Win()
    {
        int lineNumber = 3;
        int winNumber = 3;

        for (int i = 0; i < lineNumber; i++)
        {
            if (verticalWin[i].Count == winNumber)
            {
                return true;
            }
            if (horizontalWin[i].Count == winNumber)
            {
                return true;
            }
        }

        if (leftSlantWin.Count == winNumber)
        {
            return true;
        }

        if (rightSlantWin.Count == winNumber)
        {
            return true;
        }

        return false;
    }

    public bool LeftSlant(int index)
    {
        for (int i = 0; i < leftSlantArr.Length; i++)
        {
            if (leftSlantArr[i] == index)
            {
                return true;
            }
        }
        return false;
    }
    public bool RightSlant(int index)
    {
        for (int i = 0; i < rightSlantArr.Length; i++)
        {
            if (rightSlantArr[i] == index)
            {
                return true;
            }
        }
        return false;
    }
}