using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    StageManager stageManager;
    public static Teleport Instance;  
    DataManager dm;
    public GameObject targetObj;
    public pade fadeScript; // 'pade'�� �ùٸ� Ŭ���� �̸����� Ȯ���ϼ���. �Ϲ������� Ŭ���� �̸��� �빮�ڷ� �����մϴ�.
    public GameObject toObj;
    GameObject keyX;
    StageUI stageUi;
    Animator animator;
    public int stageNumber; // ��Ż�� ���� �������� ��ȣ
    public bool isActive = false; // ��Ż�� Ȱ��ȭ �������� �����ϴ� �ʵ�
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
            if (animator != null) // Animator ������Ʈ�� �����ϴ��� Ȯ��
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
        fadeScript.Fade(); // ���̵��� ����
        yield return new WaitForSeconds(1f); // ���̵��� �Ϸ� ���

        // ���� �������� ������ 5�� ���, ���� ���������� �̵�
        if (stageManager.nowStageLv == 5)
        {
            ShopUI shopUi = ShopUI.instance;
            Destroy(shopUi);
            // ���� �������� ��ȣ ���
            int nextStage = stageManager.nowStage + 1;

            // �������� �迭 ������ �ʰ����� �ʴ� ���, �������� ����
            if (nextStage - 1 < stageManager.stage.Length)
            {
                stageManager.ChangeStage(nextStage);
                stageManager.nowStageLv = 1; // ���� ���������� ������ 1�� ����
            }
            else
            {
                // �������� �迭 ������ �ʰ��ϴ� ���, ù ��° ���������� ���ư�
                stageManager.ChangeStage(1);
            }
        }
        else
        {
            // ���� �������� ������ 5�� �ƴ� ���, ������ 1 ����
            stageManager.nowStageLv++;
            if(stageManager.nowStageLv == 5)        // 2024-05-19 ������ �߰�
            {
                Debug.Log("���� UI �����");
                ShopUI shopUi = ShopUI.instance;
                Destroy(shopUi.gameObject);
            }
        }

        // ���� ���������� ���� ���
        stageUi.PrintStage(stageManager.nowStage, stageManager.nowStageLv);
        // �÷��̾� ��ġ �̵�
        targetObj.transform.position = toObj.transform.position;
        // �߰����� ��� �ð� ���� �ٷ� ���̵� �ƿ� ����
        isTelepo = false;   
        dm.SaveData();     
    }

}
