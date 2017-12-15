namespace Game.TTT
{ 
    //游戏模式
    public enum PlayMode
    {
        OnePlayerMode = 0, 
        TwoPlayerMode = 1,
    }

    //角色
    public enum Player
    {
        None,
        Player1,
        Player2
    }

    //线匹配类型
    public enum MatchType
    {
        None,
        Vertical,
        Horizontal,
        LeftSlant,
        RightSlant
    }
}