using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public static Teleport Instance;    // 04.28 ������ �߰�
    DataManager dm;     // 2024-05-14 ������ �߰�
    public GameObject targetObj;
    public pade fadeScript; // 'pade'�� �ùٸ� Ŭ���� �̸����� Ȯ���ϼ���. �Ϲ������� Ŭ���� �̸��� �빮�ڷ� �����մϴ�.
    public GameObject toObj;
    GameObject keyX;

    Animator animator;

    public bool isTelepo = false;   // 04.28 ������ �߰� - �ڷ���Ʈ Ȯ�ο�

    private void Awake()
    {
        Instance = this;    // 04.28 ������ �߰�
        keyX = this.transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        dm = DataManager.instance;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (animator != null) // Animator ������Ʈ�� �����ϴ��� Ȯ��
        {
            animator.SetBool("gate", false);
            keyX.SetActive(false);
            StartCoroutine(TelepotyRoutine());
        }
    }

    IEnumerator TelepotyRoutine()
    {
        isTelepo = true;    //04.28 ������ �߰�
        // ���̵� �� ����
        fadeScript.Fade(); // 'pade' Ŭ������ 'Fade' �޼��� ȣ��. Ŭ���� �̸��� 'Fade'�� �ùٸ��� �Ǿ� �ִ��� Ȯ���ϼ���.
        // ���̵� ���� ������ ���� ������ ���
        yield return new WaitForSeconds(1f); // ���̵� �ΰ� ��� �ð��� ������ �� �ð�
        // �÷��̾� ��ġ �̵�
        targetObj.transform.position = toObj.transform.position;
        // �߰����� ��� �ð� ���� �ٷ� ���̵� �ƿ� ����
        isTelepo = false;   //04.28 ������ �߰�
        dm.SaveData();      // �ڷ���Ʈ �� ���̺� 2024-05-14 ������ �߰�
    }
}
