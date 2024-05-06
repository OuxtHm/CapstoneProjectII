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

    string dataFolderPath;      // ������ ���� ���� ���
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

        dataFolderPath = Path.Combine(Application.persistentDataPath + "/Resources");  // ����Ƽ�� �˾Ƽ� ���� ����
        Directory.CreateDirectory(dataFolderPath);    // ���丮 ����
        playerDataPath = Path.Combine(dataFolderPath, playerFileName);      // �÷��̾� ������ ��� �缳��
        optionDataPath = Path.Combine(dataFolderPath, optionFileName);      // �ɼ� ������ ��� �缳�� 
        skillDataPath = Path.Combine(dataFolderPath, skillFileName);        // ��ų ������ ��� �缳��
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
        Debug.Log("������ ����");
        Debug.Log(player);
        if (!player.isDead)
        {
            string pData = JsonUtility.ToJson(playerData, true);     // �÷��̾� ������ ���̺�
            string sData = JsonUtility.ToJson(skillData, true);     // ��ų ������ ���̺�

            // Json ���� ����
            File.WriteAllText(playerDataPath, pData);
            File.WriteAllText(skillDataPath, sData);
            Debug.Log("�����");
        }
    }
}
