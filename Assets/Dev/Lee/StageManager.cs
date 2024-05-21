using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int nowStage;
    public int nowStageLv;
    public static StageManager instance;
    public AudioSource audioSource; // 05.21 박지우 추가
    [SerializeField] public GameObject[] stage = new GameObject[3];

    [Header("스테이지 BGM")]
    public AudioClip boss_stage1;
    public AudioClip boss_stage2;
    public AudioClip boss_stage3;

    private void Awake()
    {
        instance = this;
        audioSource = this.gameObject.GetComponentInParent<AudioSource>();
    }
    private void Start()
    {
        nowStage = 1;
        nowStageLv = 1;
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
            BGM_Sound();
        }
        else
        {
            Debug.LogError("스테이지 범위를 벗어났습니다.");
            stage[0].SetActive(true); // 첫 번째 스테이지 활성화
            nowStage = 1;
        }
    }

    public void BGM_Sound()
    {
        if(nowStage == 1)
            Sounds("Boss_stage1");
        else if(nowStage == 2)
            Sounds("Boss_stage2");
        else
            Sounds("Boss_stage3");
    }

    public void Sounds(string sounds)
    {
        switch (sounds)
        {
            case "Boss_stage1":   //1스테이지
                audioSource.clip = boss_stage1;
                break;
            case "Boss_stage2":   //1스테이지
                audioSource.clip = boss_stage2;
                break;
            case "Boss_stage3":   //1스테이지
                audioSource.clip = boss_stage3;
                break;
        }
    }
}