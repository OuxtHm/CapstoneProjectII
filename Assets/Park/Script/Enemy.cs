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
    public float detectionRange = 4f;  //������ Ÿ�� �ν� ����
    float distanceToTarget; // ���Ϳ� Ÿ�� ������ �Ÿ�
    bool istracking = false;    // ���� ���� ����

    [Header("�Ϲ� ���� �ɷ�ġ")]
    public int enemy_MaxHP; //�Ϲ� ���� �ִ�ü��
    public int enemy_CurHP; //�Ϲ� ���� ����ü��
    public int enemy_Power; //�Ϲ� ���� ���ݷ�
    public int enemy_Speed; //�Ϲ� ���� �̵��ӵ�
    //public int ActionPattern; //�Ϲ� ���� ���� ����

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
        TargetSensor(target, rayHit);
        Sensor();
    }

    void TargetSensor(Transform target, RaycastHit2D rayHit)  // �÷��̾� ����
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        distanceToTarget = Vector3.Distance(this.transform.position, target.position); // ���Ϳ� Ÿ�� ������ �Ÿ� ���
        Vector2 direction = (target.position - transform.position).normalized;
        direction.y = transform.position.y; // y�� ��ġ ������ ���� �߰�
        direction.Normalize();

        if (target.position.y > direction.y + 1)
            istracking = true;

        if (distanceToTarget <= detectionRange) // Ÿ���� ���� �ȿ� ���� �� ����
        {
            if(rayHit.collider != null && !istracking)
            {
                Debug.Log("null�� �ƴ�");
                if (direction.x >= 0)   // Ÿ���� �����ʿ� ���� ��
                {
                    DirX = 1;
                    spriteRenderer.flipX = false;
                }
                else
                {
                    DirX = -1;
                    spriteRenderer.flipX = true;
                }
                anim.SetBool("Move", true);
                transform.Translate(direction * Time.deltaTime * enemy_Speed);


            }
            else if(rayHit.collider == null)
            {
                //Debug.Log("null��");
                istracking = true;
                anim.SetBool("Move", false);
            }
            else //�����߿� �ٴ��� ������ Ÿ�� �ν� �������� �ݴ�������� �̵���
            {
                Move();
            }
        }
        else // Ÿ���� ���� �ۿ� ���� �� ����
        {
            istracking = false;
            Move();
        }
    }


    public void Move()
    {
        if (DirX != 0)
        {
            anim.SetBool("Move", true);
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
        else
        {
            anim.SetBool("Move", false);
        }
    }

    IEnumerator NextMove()  // ���Ͱ� ���� ������ �̵� ����
    {
        DirX = Random.Range(-1, 2);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    public void Sensor()    // Ÿ�� ���� �Լ�
    {
        rigid = this.GetComponent<Rigidbody2D>();

        // Enemy�� �� ĭ ���� ���� ��� ���� �ڱ� �ڽ��� ��ġ ���� (x)�� + DirX���� ���ϰ� 1.2f�� ���Ѵ�.
        Vector2 frontVec = new Vector2(rigid.position.x + DirX * 1.2f, rigid.position.y);

        // �������� ��â���� ������ Ȯ���� �� �ְ� �׷���
        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));
        
        // ���� ������� �������� �Ʒ��� ��Ƽ� �������� ������ ����, LayMask.GetMask("")�� �ش��ϴ� ���̾ ��ĵ��
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Tilemap", "UI"));
        if (rayHit.collider == null && enemy_CurHP >= 0)
        {
            Turn();
        }
    }

    void Turn() // �̹����� �ݴ�� �ٲٴ� �Լ�
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        DirX *= -1;   // nextDirX�� -1�� ���� ���� ��ȯ
        if (DirX == 1 && distanceToTarget > detectionRange)  // distanceToTarget > detectionRange�� �߰����� ������ �÷��̾ ��Ÿ� ���� �ְ� rayHit=null�̶�� ���ڸ� ������
        {
            spriteRenderer.flipX = false; // DirX ���� 1�̸� x���� flip��
        }
        else
        {
            spriteRenderer.flipX = true; // DirX ���� 1�� �ƴϸ� x�� flip�� ������
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