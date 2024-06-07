using TMPro;
using UnityEngine;

public class ShowPlayData : MonoBehaviour
{
    DataManager dm;
    TimeManager tm;
    TextMeshProUGUI[] playDataTxt = new TextMeshProUGUI[3];

    private void Awake()
    {
        for (int i = 0; i < playDataTxt.Length; i++)
        {
            playDataTxt[i] = transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }
    private void Start()
    {
        dm = DataManager.instance;
        ShowAllText();
    }
    void ShowAllText()
    {
        ShowPlayTime();
        ShowKillCount();
        ShowGold();
    }
    void ShowPlayTime()
    {
        int minutes;
        int seconds;

        minutes = Mathf.FloorToInt(dm.playerData.totalPlayTime / 60);
        seconds = Mathf.FloorToInt(dm.playerData.totalPlayTime % 60);

        string playTime = minutes + " :" + seconds;
        playDataTxt[0].text = playTime;
    }
    void ShowKillCount()
    {
        playDataTxt[1].text = dm.playerData.killCount.ToString();
    }
    void ShowGold()
    {
        playDataTxt[2].text = dm.playerData.money.ToString();
    }

}
