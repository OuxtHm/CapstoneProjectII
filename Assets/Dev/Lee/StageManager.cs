using UnityEngine;
using System.Collections;
public class StageManager : MonoBehaviour
{
    DataManager dm;
    public int nowStage;
    public int nowStageLv;
    public static StageManager instance;

    [SerializeField] public GameObject[] stage = new GameObject[3];
    [SerializeField] public GameObject[] boss = new GameObject[3];
    [SerializeField] private GameObject portal1; // 1��������
    [SerializeField] private GameObject portal2; // 2��������
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
        portal1.SetActive(false); // �ʱ⿡�� ��Ż�� ��Ȱ��ȭ
        portal2.SetActive(false);
    }

    private void Update()
    {
        CheckAndActivateBoss();
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
            Debug.Log(nowStageLv);
        }
        else
        {
            Debug.LogError("�������� ������ ������ϴ�.");
            stage[0].SetActive(true); // ù ��° �������� Ȱ��ȭ
            nowStage = 1;
        }
    }

    private void CheckAndActivateBoss()
    {
        // nowStage�� nowStageLv�� ���� 1, 2, 3�� 5�� �� boss ������Ʈ�� Ȱ��ȭ
        if (nowStageLv == 5)
        {
            if (nowStage - 1 < boss.Length)
            {
                StartCoroutine(ActivateBossWithDelay(boss[nowStage - 1]));
            }
            else
            {
                Debug.LogError("���� �迭�� ������ ������ϴ�.");
            }
        }
    }

    private IEnumerator ActivateBossWithDelay(GameObject bossObject)
    {
        yield return new WaitForSeconds(3.0f);
        bossObject.SetActive(true);
    }

    public void ActivatePortal1()
    {
        portal1.SetActive(true); // 1�������� ��Ż�� Ȱ��ȭ
    }

    public void ActivatePortal2()
    {
        portal1.SetActive(true); // 2�������� ��Ż�� Ȱ��ȭ
    }

    public void ActivatePortal3()
    {
        portal1.SetActive(true); // 3�������� ��Ż�� Ȱ��ȭ
    }
}
