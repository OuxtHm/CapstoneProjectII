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
    public float moveSpeed;         // 이동속도
    public float dashCoolTime;      // 대쉬 쿨타임
    public int money;               // 돈
    public Vector2 nowPosition;     // 현재 위치
    public int nowStage;            // 현재 스테이지
    public int nowStageLV;          // 현재 스테이지 레벨
    public int killCount;           // 몬스터 처치 횟수
    public float totalPlayTime;     // 플레이 타임
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
    public SkillManager skillM;
    ChangePassive changePassvie;
    SkillUI skillUi;
    public Player player;

    public PlayerData playerData = new PlayerData()
    {
        maxHpValue = 100,
        curHpValue = 100,
        atk = 10,
        moveSpeed = 5f,
        dashCoolTime = 3f,
        money = 0,
        nowPosition = new Vector2(0, 1),
        nowStage = 1,
        nowStageLV = 1,
        killCount = 0,
        totalPlayTime = 0,
    };

    public OptionData optionData = new OptionData()
    {
        masterValue = 0.5f,
        bgmValue = 0.5f,
        sfxValue = 0.5f,
    };

    public SkillData skillData = new SkillData()
    {
        readySkill = 7,
        nowSkill = 5,
        ultSkill = 8,
        nowPassive = 9,
        readyPassive = 11
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
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded += FindInstance;
        SceneManager.sceneLoaded += PlayerLoad;

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

        if (!File.Exists(optionDataPath))       // 옵션 데이터가 없다면 생성
        {
            SaveOptionData();
        }
        if(!File.Exists(playerDataPath))        // 플레이 데이터가 없다면 생성
        {
            SaveData();
        }
        OptionLoad();       // 옵션 데이터 불러오기
    }

    void FindInstance(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainScene")
        {
            player = Player.instance;
            skillM = SkillManager.instance;
            skillUi = SkillUI.instance;
            changePassvie = ChangePassive.instance;
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FindInstance;
        SceneManager.sceneLoaded -= PlayerLoad;
    }
    public void SaveData()      // 플레이어 및 스킬 데이터 저장 함수
    {
        //Debug.Log("데이터 저장 시작");
        string pData = JsonUtility.ToJson(playerData, true);     // 플레이어 데이터 세이브
        string sData = JsonUtility.ToJson(skillData, true);     // 스킬 데이터 세이브

        // Json 파일 쓰기
        File.WriteAllText(playerDataPath, pData);
        File.WriteAllText(skillDataPath, sData);
        //Debug.Log("저장 완료");
    }

    public void SaveOptionData()    // 옵션 데이터 저장 함수
    {
        //Debug.Log("옵션 데이터 저장 시작");
        string oData = JsonUtility.ToJson(optionData, true);        // 옵션 데이터 세이브

        File.WriteAllText(optionDataPath, oData);
        //Debug.Log("옵션 데이터 저장 완료");
    }
    
    public void PlayerLoad(Scene scene, LoadSceneMode mode)    // 인게임씬에서 플레이어 데이터 로드하는 함수
    {
        if (scene.name != "MainScene" && File.Exists(playerDataPath))
        {
            //Debug.Log("플레이어 데이터 및 스킬 데이터 로드 중....");
            string playData = File.ReadAllText(playerDataPath);
            string skData = File.ReadAllText(skillDataPath);

            playerData = JsonUtility.FromJson<PlayerData>(playData);
            skillData = JsonUtility.FromJson<SkillData>(skData);

            StartCoroutine(LoadData());
        }
    }

    public IEnumerator LoadData()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            Debug.Log(" 데이터 불러오기");
            // 플레이어 정보 불러오기
            player.maxHp = playerData.maxHpValue;
            player.curHp = playerData.curHpValue;
            player.power = playerData.atk;
            player.money = playerData.money;
            player.transform.position = playerData.nowPosition;

            // 스킬 정보 불러오기
            skillM.commonSkillNum[0] = skillData.readySkill;
            skillM.commonSkillNum[1] = skillData.nowSkill;
            skillM.ultSkillNum = skillData.ultSkill;

            // 패시브 정보 불러오기
            skillM.passiveNum[0] = skillData.readyPassive;
            skillM.passiveNum[1] = skillData.nowPassive;
            yield return null;

            CreateSkillIcon();
        }
        else
        {
            yield return null;
            StartCoroutine(LoadData());
        }
    }

    public void OptionLoad()    // 타이틀씬에서 옵션 데이터 로드하는 함수
    {
        //Debug.Log("옵션 데이터 로드 중....");
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
    public void CreateSkillIcon()       // 스킬 아이콘 생성함수
    {
        // 스킬 아이콘 생성
        skillM.CreateSkill(skillM.commonSkillNum[0], skillUi.change.readyskill);
        skillM.CreateSkill(skillM.commonSkillNum[1], skillUi.change.nowskill);
        skillM.CreateSkill(skillM.ultSkillNum, skillUi.ult);

        // 패시브 아이콘 생성
        skillM.CreateSkill(skillM.passiveNum[0], changePassvie.readyPassive);
        skillM.CreateSkill(skillM.passiveNum[1], changePassvie.nowPassive);

        // 스킬 아이콘 컴포넌트 받기
        skillUi.GetSkillComponent();
        skillUi.GetUltComponent();
        skillUi.change.ImageGetComponent();
    }
    public void NewGame()       // 새 게임 버튼 클릭시 실행하는 함수
    {
        Debug.Log("데이터 파일 삭제");
        DeleteFile();
        Debug.Log(playerDataPath);
        NewPlayerFile();
        NewSkillFile();
        SaveData();
    }
    public void DeleteFile()    // 데이터 파일 삭제하는 함수
    {
        Debug.Log("파일 삭제 함수 실행");
        File.Delete(playerDataPath);
        File.Delete(skillDataPath);
    }

    public void NewPlayerFile()
    {
        playerData.maxHpValue = 100;
        playerData.curHpValue = 100;
        playerData.atk = 10;
        playerData.moveSpeed = 5f;
        playerData.dashCoolTime = 3f;
        playerData.money = 0;
        playerData.nowPosition = new Vector2(0, 1);
        playerData.nowStage = 1;
        playerData.nowStageLV = 1;
        playerData.killCount = 0;
        playerData.totalPlayTime = 0;
    }
    public void NewSkillFile()
    {
        skillData.readySkill = 7;
        skillData.nowSkill = 5;
        skillData.ultSkill = 8;
        skillData.nowPassive = 9;
        skillData.readyPassive = 11;
        //CreateSkillIcon();
    }
}