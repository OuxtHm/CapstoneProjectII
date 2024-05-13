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
    public int money;               // ��
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
            Destroy(instance.gameObject);
        }

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
    public void SaveData()      // �÷��̾� �� ��ų ������ ���� �Լ�
    {
        Debug.Log("������ ���� ����");
        if (!player.isDead)
        {
            string pData = JsonUtility.ToJson(playerData, true);     // �÷��̾� ������ ���̺�
            string sData = JsonUtility.ToJson(skillData, true);     // ��ų ������ ���̺�

            // Json ���� ����
            File.WriteAllText(playerDataPath, pData);
            File.WriteAllText(skillDataPath, sData);
            Debug.Log("���� �Ϸ�");
        }
    }

    public void SaveOptionData()    // �ɼ� ������ ���� �Լ�
    {
        Debug.Log("�ɼ� ������ ���� ����");
        string oData = JsonUtility.ToJson(optionData, true);        // �ɼ� ������ ���̺�

        File.WriteAllText(optionDataPath, oData);
        Debug.Log("�ɼ� ������ ���� �Ϸ�");
    }
    
    public void PlayerLoad()    // �ΰ��Ӿ����� �÷��̾� ������ �ε��ϴ� �Լ�
    {
        Debug.Log("�÷��̾� ������ �� ��ų ������ �ε� ��....");
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

    public void OptionLoad()    // Ÿ��Ʋ������ �ɼ� ������ �ε��ϴ� �Լ�
    {
        Debug.Log("�ɼ� ������ �ε� ��....");
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