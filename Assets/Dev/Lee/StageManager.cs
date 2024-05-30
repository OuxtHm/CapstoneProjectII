using UnityEngine;

public class StageManager : MonoBehaviour
{
    DataManager dm;
    public int nowStage;
    public int nowStageLv;
    public static StageManager instance;
    [SerializeField] public GameObject[] stage = new GameObject[3];

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        dm = DataManager.instance;
        nowStage = dm.playerData.nowStage;
        nowStageLv = dm.playerData.nowStageLV;
        LoadMapData();
    }

    void LoadMapData()      // �� ������ �ҷ�����
    {
        stage[dm.playerData.nowStage - 1].SetActive(true);

        for (int i = 0; i < stage.Length; i++)
        {
            if (i != dm.playerData.nowStage - 1)
            {
                stage[i].SetActive(false);
            }
        }
    }
    public void ChangeStage(int num)
    {
        stage[nowStage - 1].SetActive(false); // ���� ���������� ��Ȱ��ȭ

        nowStage = num; // �������� ��ȣ ������Ʈ
        nowStageLv = 1; // �������� ���� �ʱ�ȭ

        if (nowStage - 1 < stage.Length)
        {
            stage[nowStage - 1].SetActive(true); // �� �������� Ȱ��ȭ

            // ��� Teleport ��ũ��Ʈ�� ã�Ƽ� �ش� ���������� ��Ż�� Ȱ��ȭ
            Teleport[] teleports = FindObjectsOfType<Teleport>();
            foreach (Teleport teleport in teleports)
            {
                teleport.enabled = (teleport.stageNumber <= nowStage);
            }
        }
        else
        {
            Debug.LogError("�������� ������ ������ϴ�.");
            stage[0].SetActive(true); // ù ��° �������� Ȱ��ȭ
            nowStage = 1;
        }
    }
}