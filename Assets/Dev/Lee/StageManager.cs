using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int nowStage;
    public int nowStageLv;
    public static StageManager instance;
    public AudioSource audioSource; // 05.21 ������ �߰�
    [SerializeField] public GameObject[] stage = new GameObject[3];

    [Header("�������� BGM")]
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
            BGM_Sound();
        }
        else
        {
            Debug.LogError("�������� ������ ������ϴ�.");
            stage[0].SetActive(true); // ù ��° �������� Ȱ��ȭ
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
            case "Boss_stage1":   //1��������
                audioSource.clip = boss_stage1;
                break;
            case "Boss_stage2":   //1��������
                audioSource.clip = boss_stage2;
                break;
            case "Boss_stage3":   //1��������
                audioSource.clip = boss_stage3;
                break;
        }
    }
}