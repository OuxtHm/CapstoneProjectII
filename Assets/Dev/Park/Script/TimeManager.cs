using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    DataManager dm;
    Player player;
    public float totalPlayTime;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dm = DataManager.instance;
        totalPlayTime = dm.playerData.totalPlayTime;
        player = Player.instance;
    }

    void Update()
    {
        if (!player.isDead)
        {
            totalPlayTime += Time.deltaTime;
            dm.playerData.totalPlayTime = totalPlayTime;
        }
    }
        
}
