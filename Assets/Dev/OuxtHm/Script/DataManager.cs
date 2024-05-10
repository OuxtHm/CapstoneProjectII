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