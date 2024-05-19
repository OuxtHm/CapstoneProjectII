using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    StageManager stageManager;
    public static Teleport Instance;  
    DataManager dm;
    public GameObject targetObj;
    public pade fadeScript; // 'pade'가 올바른 클래스 이름인지 확인하세요. 일반적으로 클래스 이름은 대문자로 시작합니다.
    public GameObject toObj;
    GameObject keyX;
    StageUI stageUi;
    Animator animator;
    public int stageNumber; // 포탈이 속한 스테이지 번호
    public bool isActive = false; // 포탈이 활성화 상태인지 저장하는 필드
    public bool isTelepo = false;   

    private void Awake()
    {
        Instance = this;   
        keyX = this.transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        dm = DataManager.instance;
        stageUi = StageUI.instance;
        stageManager = StageManager.instance;
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (animator != null) // Animator 컴포넌트가 존재하는지 확인
            {
                animator.SetBool("gate", true);
            }
            keyX.SetActive(true);
            targetObj = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(TelepotyRoutine());
        }
    }



    IEnumerator TelepotyRoutine()
    {
        isTelepo = true;
        fadeScript.Fade(); // 페이드인 실행
        yield return new WaitForSeconds(1f); // 페이드인 완료 대기

        // 현재 스테이지 레벨이 5일 경우, 다음 스테이지로 이동
        if (stageManager.nowStageLv == 5)
        {
            ShopUI shopUi = ShopUI.instance;
            Destroy(shopUi);
            // 다음 스테이지 번호 계산
            int nextStage = stageManager.nowStage + 1;

            // 스테이지 배열 범위를 초과하지 않는 경우, 스테이지 변경
            if (nextStage - 1 < stageManager.stage.Length)
            {
                stageManager.ChangeStage(nextStage);
                stageManager.nowStageLv = 1; // 다음 스테이지의 레벨을 1로 설정
            }
            else
            {
                // 스테이지 배열 범위를 초과하는 경우, 첫 번째 스테이지로 돌아감
                stageManager.ChangeStage(1);
            }
        }
        else
        {
            // 현재 스테이지 레벨이 5가 아닐 경우, 레벨을 1 증가
            stageManager.nowStageLv++;
            if(stageManager.nowStageLv == 5)        // 2024-05-19 유재현 추가
            {
                Debug.Log("상점 UI 지우기");
                ShopUI shopUi = ShopUI.instance;
                Destroy(shopUi.gameObject);
            }
        }

        // 현재 스테이지와 레벨 출력
        stageUi.PrintStage(stageManager.nowStage, stageManager.nowStageLv);
        // 플레이어 위치 이동
        targetObj.transform.position = toObj.transform.position;
        // 추가적인 대기 시간 없이 바로 페이드 아웃 시작
        isTelepo = false;   
        dm.SaveData();     
    }

}
