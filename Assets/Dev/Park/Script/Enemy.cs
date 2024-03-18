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

    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float detectionRange = 4f;  //������ Ÿ�� �ν� ����
    float distanceToTarget; // ���Ϳ� Ÿ�� ������ �Ÿ�
    bool istracking = false;    // ���� ���� ����
    int enemy_OriginSpeed;  //������ ���� �ӵ�
    protected int enemy_Type; // ���� ������ ���� �з� ��ȣ 1: �Ϲ� ���� ����, 2: �Ϲ� ���� ����
    bool isdie = false;
    bool ishurt = false;
    bool isattack = false;

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
        
            
        if (distanceToTarget <= detectionRange && !ishurt && !isdie && !isattack && enemy_Type != 2) // Ÿ���� ���� �ȿ� ���� �� ����
        {
            if(rayHit.collider != null && !istracking && enemy_Type == 1) // ���� ���� �϶�
            {
                direction.y = transform.position.y; // y�� ��ġ ������ ���� �߰�
                direction.Normalize();

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

                if(distanceToTarget <= 2.5f && !isattack && !ishurt)
                {
                    StartCoroutine(Attack());
                }
                //Debug.Log(distanceToTarget);
                //Debug.Log("���� ������");

            }
            else if(rayHit.collider == null)
            {
                istracking = true;
                anim.SetBool("Move", false);
            }
            else //�����߿� �ٴ��� ������ Ÿ�� �ν� �������� �ݴ�������� �̵���
            {
                Move();
            }

            
        }
        else if (enemy_Type == 2)  // ���� ���� �϶�
        {
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
            Vector2 targetPosition = new Vector2(target.position.x - 1, target.position.y - 2);
            Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
            Debug.Log("���� ������");
        }
        else if(distanceToTarget >= detectionRange) // Ÿ���� ���� �ۿ� ���� �� ����
        {
            istracking = false;
            Move();
        }
    }


    public void Move()
    {
        if (DirX != 0 && !isdie && !ishurt)
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
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Tilemap", "UI"));   //���̾�� �� ���̾ �������� �����ؾ���
        if (rayHit.collider == null && enemy_CurHP >= 0 && enemy_Type != 2)
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

    IEnumerator Attack()
    {
        enemy_OriginSpeed = enemy_Speed;
        enemy_Speed = 0;
        isattack = true;
        anim.SetBool("Move", false);
        anim.SetTrigger("Attack");
        Debug.Log("���� ����");
        yield return new WaitForSeconds(2f);
        isattack = false;
        enemy_Speed = enemy_OriginSpeed;
    }

   

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !isdie && !ishurt)
        {
            StartCoroutine(Hurt(collision.transform));
        }
    }

    IEnumerator Hurt(Transform target)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        if(enemy_CurHP > 0 && !isdie)
        {
            ishurt = true;
            Debug.Log(istracking);
            enemy_CurHP = enemy_CurHP - 1;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            enemy_OriginSpeed = enemy_Speed;
            enemy_Speed = 0;

            StartCoroutine(Blink());
            StartCoroutine(Knockback(target));

            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(Die());
                Debug.Log("�׾���");
            }
        }

        

        yield return new WaitForSeconds(0.5f);
        enemy_Speed = enemy_OriginSpeed;
        ishurt = false;
    }

    IEnumerator Die()  //���Ͱ� �׾��� ����
    {
        istracking = true;
        DirX = 0;
        anim.SetBool("Move", false);
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //StopAllCoroutines();
        //gameObject.SetActive(false);
        //Destroy(gameObject); �����ҰŸ� �� ����� ���
    }

    IEnumerator Knockback(Transform target)
    {
        Vector3 knockbackDirection = transform.position - target.position;  //�ǰݵ� ��ġ�� ����
        knockbackDirection.Normalize();
        rigid.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse); // �ǰݵ� ��ġ * ���ϴ� ���� ũ�⸸ŭ �˹�. ForceMode2D.Impulse�� ����ϸ� �������� ���� ���� �� �� ���� 
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Blink() // �ǰ� ȿ��
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmos()
    {
        // ���� ������ �ð������� ��Ÿ���� ���� �ڵ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
