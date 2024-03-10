using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform target;
    RaycastHit2D rayHit;
    //Player player;

    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float detectionRange = 5f;  //������ Ÿ�� �ν� ����
    float distanceToTarget; // ���Ϳ� Ÿ�� ������ �Ÿ�

    [Header("�Ϲ� ���� �ɷ�ġ")]
    public int enemy_MaxHP; //�Ϲ� ���� �ִ�ü��
    public int enemy_CurHP; //�Ϲ� ���� ����ü��
    public int enemy_Power; //�Ϲ� ���� ���ݷ�
    public int enemy_Speed; //�Ϲ� ���� �̵��ӵ�
    public int ActionPattern; //�Ϲ� ���� ���� ����

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        StartCoroutine(NextMove());
    }

    public virtual void Short_Monster(Transform target)
    {
        TargetSenser(target);
        Sensor();
    }

    void TargetSenser(Transform target)  // �÷��̾� ����
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        distanceToTarget = Vector3.Distance(this.transform.position, target.position); // ���Ϳ� Ÿ�� ������ �Ÿ� ���

        if (distanceToTarget <= detectionRange) // Ÿ���� ���� �ȿ� ���� �� ����
        {
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = transform.position.y;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * enemy_Speed);
            Debug.Log("Ÿ���� �����߽��ϴ�!");
        }
        else // Ÿ���� ���� �ۿ� ���� �� ����
        {
            Move();
            Debug.Log("Ÿ���� ���� ���� �ۿ� �ֽ��ϴ�.");
        }
    }

    public void Move()
    {
        transform.Translate(new Vector2(DirX, transform.position.y).normalized * Time.deltaTime * enemy_Speed);

        if (DirX == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (DirX == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator NextMove()  // ���Ͱ� ���� ������ �̵� ����
    {
        DirX = Random.Range(-1, 2);
        Debug.Log(DirX);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    public void Sensor()    // Ÿ�� ���� �Լ�
    {
        rigid = this.GetComponent<Rigidbody2D>();

        // Enemy�� �� ĭ ���� ���� ��� ���� �ڱ� �ڽ��� ��ġ ���� (x)�� + nextDirX���� ���ϰ� 1.2f�� ���Ѵ�.
        Vector2 frontVec = new Vector2(rigid.position.x + DirX * 1.2f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));

        // �������� �Ʒ��� ��Ƽ� �������� ������ ����(�������), LayMask.GetMask("")�� �ش��ϴ� ���̾ ��ĵ��
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Tilemap", "UI"));
        if (rayHit.collider == null && enemy_CurHP >= 0)
        {
            Turn();
        }
    }

    void Turn() // �̹����� �ݴ�� �ٲٴ� �Լ�
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        DirX *= -1;   // nextDirX�� -1�� ���� ������ȯ
        if (DirX == 1 && distanceToTarget > detectionRange)  // distanceToTarget > detectionRange�� �߰����� ������ �÷��̾ ��Ÿ� ���� �ְ� rayHit=null�̶�� ���ڸ� ������
        {
            spriteRenderer.flipX = true; // DirX ���� 1�̸� x���� flip��
        }
    }

    private void OnDrawGizmos()
    {
        // ���� ������ �ð������� ��Ÿ���� ���� �ڵ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
