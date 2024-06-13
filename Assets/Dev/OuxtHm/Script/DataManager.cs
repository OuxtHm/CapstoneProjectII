using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public float maxHpValue;        // �ִ� ü��
    public float curHpValue;        // ���� ü��
    public float atk;               // ���ݷ�
    public float moveSpeed;         // �̵��ӵ�
    public float dashCoolTime;      // �뽬 ��Ÿ��
    public int money;               // ��
    public Vector2 nowPosition;     // ���� ��ġ
    public int nowStage;            // ���� ��������
    public int nowStageLV;          // ���� �������� ����
    public int killCount;           // ���� óġ Ƚ��
    public float totalPlayTime;     // �÷��� Ÿ��
}

public class OptionData
{
    public float masterValue;       // ������ ����
    public float bgmValue;          // ����� ����
    public float sfxValue;          // ȿ���� ����
}

public class SkillData
{
    public int readySkill;          // ��� ���� ��ų
    public int nowSkill;            // ��� ���� ��ų
    public int ultSkill;            // �ñر�
    public int nowPassive;          // ���� �нú�
    public int readyPassive;        // ��� ���� �нú�
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

    string dataFolderPath;      // ������ ���� ���� ���
    public string playerDataPath;       // �÷��̾� ������ ���
    string optionDataPath;      // �ɼ� ������ ���
    string skillDataPath;       // ��ų ������ ���

    string playerFileName = "PlayerData";       // �÷��̾� ������ ���� �̸�
    string optionFileName = "OptionData";       // �ɼ� ������ ���� �̸�
    string skillFileName = "SkillData";         // ��ų ������ ���� �̸�

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

        dataFolderPath = Path.Combine(Application.persistentDataPath + "/Resources");  // ����Ƽ�� �˾Ƽ� ���� ����
        Directory.CreateDirectory(dataFolderPath);    // ���丮 ����
        playerDataPath = Path.Combine(dataFolderPath, playerFileName);      // �÷��̾� ������ ��� �缳��
        optionDataPath = Path.Combine(dataFolderPath, optionFileName);      // �ɼ� ������ ��� �缳�� 
        skillDataPath = Path.Combine(dataFolderPath, skillFileName);        // ��ų ������ ��� �缳��
    }
    void Start()
    {
        gm = GameManager.instance;
        sm = SoundManager.instance;

        if (!File.Exists(optionDataPath))       // �ɼ� �����Ͱ� ���ٸ� ����
        {
            SaveOptionData();
        }
        if(!File.Exists(playerDataPath))        // �÷��� �����Ͱ� ���ٸ� ����
        {
            SaveData();
        }
        OptionLoad();       // �ɼ� ������ �ҷ�����
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
    public void SaveData()      // �÷��̾� �� ��ų ������ ���� �Լ�
    {
        //Debug.Log("������ ���� ����");
        string pData = JsonUtility.ToJson(playerData, true);     // �÷��̾� ������ ���̺�
        string sData = JsonUtility.ToJson(skillData, true);     // ��ų ������ ���̺�

        // Json ���� ����
        File.WriteAllText(playerDataPath, pData);
        File.WriteAllText(skillDataPath, sData);
        //Debug.Log("���� �Ϸ�");
    }

    public void SaveOptionData()    // �ɼ� ������ ���� �Լ�
    {
        //Debug.Log("�ɼ� ������ ���� ����");
        string oData = JsonUtility.ToJson(optionData, true);        // �ɼ� ������ ���̺�

        File.WriteAllText(optionDataPath, oData);
        //Debug.Log("�ɼ� ������ ���� �Ϸ�");
    }
    
    public void PlayerLoad(Scene scene, LoadSceneMode mode)    // �ΰ��Ӿ����� �÷��̾� ������ �ε��ϴ� �Լ�
    {
        if (scene.name != "MainScene" && File.Exists(playerDataPath))
        {
            //Debug.Log("�÷��̾� ������ �� ��ų ������ �ε� ��....");
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
            Debug.Log(" ������ �ҷ�����");
            // �÷��̾� ���� �ҷ�����
            player.maxHp = playerData.maxHpValue;
            player.curHp = playerData.curHpValue;
            player.power = playerData.atk;
            player.money = playerData.money;
            player.transform.position = playerData.nowPosition;

            // ��ų ���� �ҷ�����
            skillM.commonSkillNum[0] = skillData.readySkill;
            skillM.commonSkillNum[1] = skillData.nowSkill;
            skillM.ultSkillNum = skillData.ultSkill;

            // �нú� ���� �ҷ�����
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

    public void OptionLoad()    // Ÿ��Ʋ������ �ɼ� ������ �ε��ϴ� �Լ�
    {
        //Debug.Log("�ɼ� ������ �ε� ��....");
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
    public void CreateSkillIcon()       // ��ų ������ �����Լ�
    {
        // ��ų ������ ����
        skillM.CreateSkill(skillM.commonSkillNum[0], skillUi.change.readyskill);
        skillM.CreateSkill(skillM.commonSkillNum[1], skillUi.change.nowskill);
        skillM.CreateSkill(skillM.ultSkillNum, skillUi.ult);

        // �нú� ������ ����
        skillM.CreateSkill(skillM.passiveNum[0], changePassvie.readyPassive);
        skillM.CreateSkill(skillM.passiveNum[1], changePassvie.nowPassive);

        // ��ų ������ ������Ʈ �ޱ�
        skillUi.GetSkillComponent();
        skillUi.GetUltComponent();
        skillUi.change.ImageGetComponent();
    }
    public void NewGame()       // �� ���� ��ư Ŭ���� �����ϴ� �Լ�
    {
        Debug.Log("������ ���� ����");
        DeleteFile();
        Debug.Log(playerDataPath);
        NewPlayerFile();
        NewSkillFile();
        SaveData();
    }
    public void DeleteFile()    // ������ ���� �����ϴ� �Լ�
    {
        Debug.Log("���� ���� �Լ� ����");
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