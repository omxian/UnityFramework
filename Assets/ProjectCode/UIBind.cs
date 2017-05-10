using UnityEngine;
using UnityEngine.UI;
using Unity.Framework;
//临时解决方案，应该使用工具生成
partial class TicTacToe
{
    private Transform gameBG;
    private Button onePlayerButton;
    private Button twoPlayerButton;
    private Transform startPanel;
    private Transform gamePanel;
    private Text winText;
    private Button backButton;
    private Toggle offensiveToggle;

    public void InitUI()
    {
        startPanel = transform.Find("StartPanel");
        onePlayerButton = startPanel.Find("OnePlayerButton").GetComponent<Button>();
        twoPlayerButton = startPanel.Find("TwoPlayerButton").GetComponent<Button>();
        offensiveToggle = startPanel.Find("OffensiveToggle").GetComponent<Toggle>();
        gamePanel = transform.Find("GamePanel");
        gameBG = gamePanel.Find("GameBG");
        backButton = gamePanel.Find("BackButton").GetComponent<Button>();
        backButton.SetActive(false);
        winText = gamePanel.Find("WinText").GetComponent<Text>();
    }
}
