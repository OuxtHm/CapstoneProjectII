using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public float maxHpValue;        // 최대 체력
    public float curHpValue;        // 현재 체력
    public float atk;               // 공격력
    public int money;               // 돈
}

public class OptionData
{
    public float masterValue;       // 마스터 볼륨
    public float bgmValue;          // 배경음 볼륨
    public float sfxValue;          // 효과음 볼륨
}

public class SkillData
{
    public int readySkill;          // 대기 중인 스킬
    public int nowSkill;            // 사용 중인 스킬
    public int ultSkill;            // 궁극기
    public int nowPassive;          // 현재 패시브
    public int readyPassive;        // 대기 중인 패시브
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public GameManager gm;
    public SoundManager sm;
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
        masterValue = 0.5f,
        bgmValue = 0.5f,
        sfxValue = 0.5f,
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
    public string playerDataPath;       // 플레이어 데이터 경로
    string optionDataPath;      // 옵션 데이터 경로
    string skillDataPath;       // 스킬 데이터 경로

    string playerFileName = "PlayerData";       // 플레이어 데이터 파일 이름
    string optionFileName = "OptionData";       // 옵션 데이터 파일 이름
    string skillFileName = "SkillData";         // 스킬 데이터 파일 이름

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
    }
    void Start()
    {
        gm = GameManager.instance;
        sm = SoundManager.instance;
        StartCoroutine(FindPlayer());

        if (!File.Exists(optionDataPath))
        {
            SaveOptionData();
        }
        if(!File.Exists(playerDataPath))
        {
            SaveData();
        }
        PlayerLoad();
        OptionLoad();
        StartCoroutine(FirstSaveFile());
    }
    IEnumerator FindPlayer()
    {
        while(player == null)
        {
            player = Player.instance;
            yield return null;
        }
    }
    public void SaveData()      // 플레이어 및 스킬 데이터 저장 함수
    {
        Debug.Log("데이터 저장 시작");
        if (!player.isDead)
        {
            string pData = JsonUtility.ToJson(playerData, true);     // 플레이어 데이터 세이브
            string sData = JsonUtility.ToJson(skillData, true);     // 스킬 데이터 세이브

            // Json 파일 쓰기
            File.WriteAllText(playerDataPath, pData);
            File.WriteAllText(skillDataPath, sData);
            Debug.Log("저장 완료");
        }
    }

    public void SaveOptionData()    // 옵션 데이터 저장 함수
    {
        Debug.Log("옵션 데이터 저장 시작");
        string oData = JsonUtility.ToJson(optionData, true);        // 옵션 데이터 세이브

        File.WriteAllText(optionDataPath, oData);
        Debug.Log("옵션 데이터 저장 완료");
    }
    
    public void PlayerLoad()    // 인게임씬에서 플레이어 데이터 로드하는 함수
    {
        Debug.Log("플레이어 데이터 및 스킬 데이터 로드 중....");
        string playData = File.ReadAllText(playerDataPath);
        string skData = File.ReadAllText(skillDataPath);

        playerData = JsonUtility.FromJson<PlayerData>(playData);
        skillData = JsonUtility.FromJson<SkillData>(skData);

        StartCoroutine(LoadData());
    }

    public IEnumerator LoadData()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            player.maxHp = playerData.maxHpValue;
            player.curHp = playerData.curHpValue;
            player.power = playerData.atk;
            player.money = playerData.money;
        }
        else
        {
            yield return null;
            StartCoroutine(LoadData());
        }
    }

    public void OptionLoad()    // 타이틀씬에서 옵션 데이터 로드하는 함수
    {
        Debug.Log("옵션 데이터 로드 중....");
        string optData = File.ReadAllText(optionDataPath);
        optionData = JsonUtility.FromJson<OptionData>(optData);
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            OptionDataLoad();
        }
    }
    public void OptionDataLoad()
    {
        sm.MasterVolume(optionData.masterValue);
        sm.BGMVolume(optionData.bgmValue);
        sm.SFXVolume(optionData.sfxValue);
    }
    public IEnumerator FirstSaveFile()
    {
        if(SceneManager.GetActiveScene().name != "MainScene")
        {
            SaveData();
        }
        else
        {
            yield return null;
            StartCoroutine(FirstSaveFile());
        }
    }

}