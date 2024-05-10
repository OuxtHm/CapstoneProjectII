using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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

    public GameManager gm;
    public SoundManager sm;
    Player player;
    SoundOption sound;

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
    public void WriteOptionData()
    {
        optionData.masterValue = sound.masterSlider.value;
        optionData.bgmValue = sound.bgmSlider.value;
        optionData.sfxValue = sound.sfxSlider.value;
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