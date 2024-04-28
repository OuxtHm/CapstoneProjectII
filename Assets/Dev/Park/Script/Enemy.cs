using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    Player player;
    Teleport teleport;
    EnemyHpBar enemyHpbar;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform AttackBox;
    BoxCollider2D AttackBoxSize; //���� ���� �ݶ��̴� ������
    RaycastHit2D rayHit;    //�ٴ� ���� ����
    RaycastHit2D rayHitAtk; // ���� ���� ����
    RaycastHit2D rayHitfront;   //�� ���� ����

    public GameObject ExplosionPb; //������ ���� ���Ȱ� ������

    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    int enemy_OriginSpeed;  //������ ���� �ӵ�
    int Exdir;  //����ü ���Ⱚ
    float detectionRange = 7f;  //������ Ÿ�� �ν� ����
    float distanceToTarget; // ���Ϳ� Ÿ�� ������ �Ÿ�
    public bool istracking = false;    // ���� ���� ����
    bool isdie = false; //���� Ȯ��
    bool ishurt = false;    //�ǰ� ���� Ȯ��
    bool isattack = false;  //���� ���� Ȯ��

    [Header("�Ϲ� ���� �ɷ�ġ")]
    protected int enemy_Type; // ���� ������ ���� �з� ��ȣ 1: �Ϲ� ����, 2: ���� ����, 3: �浹 ����
    public float enemy_MaxHP; //�Ϲ� ���� �ִ�ü��
    public float enemy_CurHP; //�Ϲ� ���� ����ü��
    public int enemy_Power; //�Ϲ� ���� ���ݷ�
    public int enemy_Speed; //�Ϲ� ���� �̵��ӵ�
    public float enemy_AttackSensor;  //�Ϲ� ���� �÷��̾� ���� ����
    public float enemy_frontSensor; //�Ϲ� ���� ���� ���� ����

    private void Awake()
    {
        Instance = this;
        enemyHpbar = this.transform.GetChild(1).GetComponent<EnemyHpBar>();
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        teleport = Teleport.Instance.GetComponent<Teleport>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        StartCoroutine(NextMove());
    }

    public virtual void Short_Monster(Transform target)
    {
        TargetSensor(target);
        Sensor();
    }

    void TargetSensor(Transform target)  // �÷��̾� ����
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        float distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // ���Ϳ� Ÿ�� ������ x�� �Ÿ� ���(���밪)
        Vector2 direction = (target.position - transform.position).normalized;

        //���ͺ� �÷��̾ �����Ͽ� ������ �����ϴ� raycast ���� ����
        float dir = DirX > 0 ? 1 : -1; // DirX�� ����̸� 1, �����̸� -1�� direction���� ����
        Vector2 rayDirection = new Vector2(dir, 0);
        rayHitAtk = Physics2D.Raycast(rigid.position, rayDirection, enemy_AttackSensor, LayerMask.GetMask("Player"));
        Debug.DrawRay(rigid.position, rayDirection * enemy_AttackSensor, new Color(1, 0, 0));

        if (teleport.isTelepo == false) //�÷��̾ ��Ż�� ������Ͻ� �̵� ����
        {
            if (distanceToTarget <= detectionRange && !ishurt && !isdie && enemy_Type != 2) //���� ���� �̿��� ���Ͱ� Ÿ���� ���� �ȿ� ���� �� ����
            {
                istracking = true;
                if (rayHit.collider != null && istracking && !isattack)
                {
                    direction.y = 0; // y�� ��ġ ������ ���� �߰�
                    direction.Normalize();
                    if (direction.x >= 0)   // Ÿ���� �����ʿ� ���� ��
                    {
                        DirX = 1;
                        Exdir = 1;
                        spriteRenderer.flipX = false;
                        AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
                    }
                    else
                    {
                        DirX = -1;
                        Exdir = -1;
                        spriteRenderer.flipX = true;
                        AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
                    }

                    if ((target.position.y - transform.position.y) <= 4f)   //Ÿ�ٰ� ��������� ���� ���̰� ������� ���� ����
                    {
                        anim.SetBool("Move", true);
                        transform.Translate(direction * Time.deltaTime * enemy_Speed);
                    }
                    else
                    {
                        anim.SetBool("Move", false);
                    }


                    if (rayHitAtk.collider != null && !isattack && !ishurt && enemy_Type == 1)   //�Ϲ� ������ ���� ����
                    {
                        if (player.curHp > 0)
                            StartCoroutine(Attack());
                    }
                }
                else if (rayHit.collider == null)  //�ٴ��� ������ ���� ����
                {
                    istracking = false;
                    anim.SetBool("Move", false);
                }
                else //�����߿� �ٴ��� ������ Ÿ�� �ν� �������� �ݴ�������� �̵���
                {
                    Move();
                }
            }
            else if (enemy_Type == 2 && !istracking)  // ���� ���� �϶�
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
                Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
                Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
                transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
            }
            else if (distanceToTarget >= detectionRange && enemy_Type != 2) // Ÿ���� ���� �ۿ� ���� �� ����
            {
                istracking = false;
                Move();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)  //�浹 ����� �ֱ�
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(enemy_Power);
            }
            else
                Debug.Log("�÷��̾ �� �ҷ���");
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)   //���� ����� �� �ݴ�� ���ư�
    {
        if (collision.gameObject.CompareTag("wall") && enemy_Type != 2)
        {
            Turn();
        }
        

    }
    
    public void Move()  //���� �⺻ �̵� ����
    {
        if (teleport.isTelepo == false)
        {
            if (DirX != 0 && !isdie && !ishurt && !isattack && !istracking)
            {
                anim.SetBool("Move", true);
                gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * enemy_Speed);

                if (DirX == -1)
                {
                    spriteRenderer.flipX = true;
                    AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
                }
                else if (DirX == 1)
                {
                    spriteRenderer.flipX = false;
                    AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
                }
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
    }

    IEnumerator NextMove()  // ���Ͱ� ���� ������ �̵� ����
    {
        if(!istracking)
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
        Vector2 WallVec = new Vector2(rigid.position.x + DirX * enemy_frontSensor, rigid.position.y);

        // �������� ��â���� ������ Ȯ���� �� �ְ� �׷���
        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));
        Debug.DrawRay(WallVec, Vector3.down * 0.3f, new Color(0, 0, 1));

        // ���� ������� �������� �Ʒ��� ��Ƽ� �������� ������ ����, LayMask.GetMask("")�� �ش��ϴ� ���̾ ��ĵ��
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Ground"));
        rayHitfront = Physics2D.Raycast(WallVec, Vector3.down, 0.3f, LayerMask.GetMask("Ground"));
        if (rayHit.collider == null && enemy_CurHP >= 0 && enemy_Type != 2)
        {
            Turn();
        }
        if(rayHitfront.collider != null && enemy_CurHP >= 0 && enemy_Type != 2 && !istracking)
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

    IEnumerator Attack()    //���� ���� �Լ�
    {
        anim.SetTrigger("Attack");
        enemy_OriginSpeed = enemy_Speed;
        isattack = true;

        yield return new WaitForSeconds(0.7f);
        AttackBoxSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, AttackBoxSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = collider.GetComponent<Player>();
                player.Playerhurt(enemy_Power);
            }
        }
        yield return new WaitForSeconds(1.5f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        isattack = false;
    }

    void FrogExplosion() //������ ���� ����ü ���� - �ִϸ��̼ǿ��� �����
    {
        ExplosionPb ExPb = ExplosionPb.GetComponent<ExplosionPb>();
        ExPb.Power = 10;
        ExPb.Speed = 4;
        ExPb.Dir = Exdir;
        ExPb.DelTime = 2f;

        GameObject Explosion = Instantiate(ExplosionPb, AttackBox.position, AttackBox.rotation);
    }


    public IEnumerator Hurt(Transform target, float damage)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        if(enemy_CurHP > 0 && !isdie)
        {
            yield return new WaitForSeconds(0);
            ishurt = true;
            enemy_CurHP = enemy_CurHP - damage;
            StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 ������ �߰�
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");

            StartCoroutine(Blink());
            if (enemy_Speed > 0)
                enemy_OriginSpeed = enemy_Speed;
            enemy_Speed = 0;

            Invoke("OriginSpeed", 0.5f);
            if (enemy_Type != 3) //�浹 ���ʹ� �˹�ó�� ����
            {
                StartCoroutine(Knockback(target));
            }
            
            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 ������ �߰�
                StartCoroutine(Die());
                Debug.Log("�׾���");
            }
        }
        ishurt = false;
    }

    IEnumerator Die()  //���Ͱ� �׾��� ����
    {
        istracking = false;
        DirX = 0;
        anim.SetBool("Move", false);
        anim.SetTrigger("Die");
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Knockback(Transform target)
    {
        Vector2 knockbackDirection = (transform.position - target.position).normalized;  // �ǰݵ� ��ġ�� �����ϰ� ������ ����ȭ
        float maxKnockbackDistance = 2f;    // �˹� ������ �ִ� �Ÿ�
        float knockbackDistance = 2.0f;  // �˹� �Ÿ��� ��Ÿ���� ����
        rigid.AddForce(knockbackDirection * knockbackDistance, ForceMode2D.Impulse);  // �ǰݵ� ��ġ * �˹� �Ÿ���ŭ�� ���� �˹鿡 ���
        float distanceTravelled = 0f;  // �̹� �̵��� �Ÿ��� ��Ÿ���� ����

        while (distanceTravelled < maxKnockbackDistance)
        {
            distanceTravelled += knockbackDistance * Time.fixedDeltaTime;  // �̵��� �Ÿ��� ����
            yield return new WaitForFixedUpdate();  // Fixed Update���� üũ�Ͽ� ���� �Ÿ������� �̵��ϵ��� ��
        }
    }

    IEnumerator Blink() // �ǰ� ȿ��
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    void OriginSpeed()  //���� ���� �̵��ӵ��� �����ϴ� �Լ�
    {
        enemy_Speed = enemy_OriginSpeed;
    }

    private void OnDrawGizmos()
    {
        // ���� ������ �ð������� ��Ÿ���� ���� �ڵ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
