using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public float maxHpValue;
    public float curHpValue;
    public float atk;
    public int money;
}

public class OptionData
{
    public float masterValue;
    public float bgmValue;
    public float sfxValue;
}

public class SkillData
{
    public int readySkill;
    public int nowSkill;
    public int ultSkill;
    public int nowPassive;
    public int readyPassive;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    GameManager gm;
    Player player;


    public PlayerData playerData = new PlayerData()
    {
        maxHpValue = 100,
        curHpValue = 100,
        atk = 10,
        money = 0
    };

    public OptionData optionData = new OptionData()
    {
        masterValue = 1f,
        bgmValue = 1f,
        sfxValue = 1f,
    };

    public SkillData skillData = new SkillData()
    {
        readySkill = -1,
        nowSkill = -1,
        ultSkill = -1,
        nowPassive = -1,
        readyPassive = -1
    };

    string dataFolderPath;      // 데이터 저장 폴더 경로
    public string playerDataPath;
    string optionDataPath;
    string skillDataPath;

    string playerFileName = "PlayerData";
    string optionFileName = "OptionData";
    string skillFileName = "SkillData";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }

        dataFolderPath = Path.Combine(Application.persistentDataPath + "/Resources");  // 유니티가 알아서 폴더 생성
        Directory.CreateDirectory(dataFolderPath);    // 디렉토리 생성
        playerDataPath = Path.Combine(dataFolderPath, playerFileName);      // 플레이어 데이터 경로 재설정
        optionDataPath = Path.Combine(dataFolderPath, optionFileName);      // 옵션 데이터 경로 재설정 
        skillDataPath = Path.Combine(dataFolderPath, skillFileName);        // 스킬 데이터 경로 재설정
        print(dataFolderPath);
    }
    void Start()
    {
        gm = GameManager.instance;
        player = Player.instance;

        SaveData();
    }

    public void SaveData()
    {
        Debug.Log("데이터 저장");
        Debug.Log(player);
        if (!player.isDead)
        {
            string pData = JsonUtility.ToJson(playerData, true);     // 플레이어 데이터 세이브
            string sData = JsonUtility.ToJson(skillData, true);     // 스킬 데이터 세이브

            // Json 파일 쓰기
            File.WriteAllText(playerDataPath, pData);
            File.WriteAllText(skillDataPath, sData);
            Debug.Log("저장됨");
        }
    }
}
