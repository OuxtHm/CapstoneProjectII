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
        }
        else
        {
            Debug.LogError("스테이지 범위를 벗어났습니다.");
            stage[0].SetActive(true); // 첫 번째 스테이지 활성화
            nowStage = 1;
        }
    }
}