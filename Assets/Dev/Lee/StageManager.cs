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
    [SerializeField] private GameObject portal1; // 1스테이지
    [SerializeField] private GameObject portal2; // 2스테이지
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
        portal1.SetActive(false); // 초기에는 포탈을 비활성화
        portal2.SetActive(false);
    }

    private void Update()
    {
        CheckAndActivateBoss();
    }

    void LoadMapData()      // 맵 데이터 불러오기
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
        stage[nowStage - 1].SetActive(false); // 현재 스테이지를 비활성화

        nowStage = num; // 스테이지 번호 업데이트
        nowStageLv = 1; // 스테이지 레벨 초기화

        if (nowStage - 1 < stage.Length)
        {
            stage[nowStage - 1].SetActive(true); // 새 스테이지 활성화

            // 모든 Teleport 스크립트를 찾아서 해당 스테이지의 포탈만 활성화
            Teleport[] teleports = FindObjectsOfType<Teleport>();
            foreach (Teleport teleport in teleports)
            {
                teleport.enabled = (teleport.stageNumber <= nowStage);
            }
            Debug.Log(nowStageLv);
        }
        else
        {
            Debug.LogError("스테이지 범위를 벗어났습니다.");
            stage[0].SetActive(true); // 첫 번째 스테이지 활성화
            nowStage = 1;
        }
    }

    private void CheckAndActivateBoss()
    {
        // nowStage와 nowStageLv가 각각 1, 2, 3과 5일 때 boss 오브젝트를 활성화
        if (nowStageLv == 5)
        {
            if (nowStage - 1 < boss.Length)
            {
                StartCoroutine(ActivateBossWithDelay(boss[nowStage - 1]));
            }
            else
            {
                Debug.LogError("보스 배열의 범위를 벗어났습니다.");
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
        portal1.SetActive(true); // 1스테이지 포탈을 활성화
    }

    public void ActivatePortal2()
    {
        portal1.SetActive(true); // 2스테이지 포탈을 활성화
    }

    public void ActivatePortal3()
    {
        portal1.SetActive(true); // 3스테이지 포탈을 활성화
    }
}
