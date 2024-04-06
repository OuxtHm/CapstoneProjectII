using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    Player player;
    EnemyHpBar enemyHpbar;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform AttackBox;
    BoxCollider2D AttackBoxSize; //���� ���� �ݶ��̴� ������
    RaycastHit2D rayHit;
    RaycastHit2D rayHitAtk;

    public GameObject ExplosionPb; //������ ���� ���Ȱ� ������

    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float detectionRange = 7f;  //������ Ÿ�� �ν� ����
    float distanceToTarget; // ���Ϳ� Ÿ�� ������ �Ÿ�
    bool istracking = false;    // ���� ���� ����
    int enemy_OriginSpeed;  //������ ���� �ӵ�
    int atkPattern; //���� ���� ����
    bool isdie = false; //���� Ȯ��
    bool ishurt = false;    //�ǰ� ���� Ȯ��
    bool isattack = false;  //���� ���� Ȯ��
    bool giveDmg = false;   //������ ����ü ����� Ȯ��

    [Header("�Ϲ� ���� �ɷ�ġ")]
    protected int enemy_Type; // ���� ������ ���� �з� ��ȣ 1: �Ϲ� ����, 2: ���� ����, 3: �浹 ����
    public float enemy_MaxHP; //�Ϲ� ���� �ִ�ü��
    public float enemy_CurHP; //�Ϲ� ���� ����ü��
    public int enemy_Power; //�Ϲ� ���� ���ݷ�
    public int enemy_Speed; //�Ϲ� ���� �̵��ӵ�
    public float enemy_AttackSensor;  //�Ϲ� ���� �÷��̾� ���� ����

    private void Awake()
    {
        Instance = this;
        enemyHpbar = this.transform.GetChild(1).GetComponent<EnemyHpBar>();
    }

    private void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        StartCoroutine(NextMove());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Hurt(AttackBox));
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
        float distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // ���Ϳ� Ÿ�� ������ x�� �Ÿ� ���
        Vector2 direction = (target.position - transform.position).normalized;

        //���ͺ� �÷��̾ �����Ͽ� ������ �����ϴ� raycast ���� ����
        float dir = DirX > 0 ? 1 : -1; // DirX�� ����̸� 1, �����̸� -1�� direction���� ����
        Vector2 rayDirection = new Vector2(dir, 0);
        rayHitAtk = Physics2D.Raycast(rigid.position, rayDirection, enemy_AttackSensor, LayerMask.GetMask("Player"));
        Debug.DrawRay(rigid.position, rayDirection * enemy_AttackSensor, new Color(1, 0, 0));

        if (distanceToTarget <= detectionRange && !ishurt && !isdie && enemy_Type != 2) //���� ���� �̿��� ���Ͱ� Ÿ���� ���� �ȿ� ���� �� ����
        {
            if (rayHit.collider != null && !istracking && !isattack)
            {
                direction.y = 0; // y�� ��ġ ������ ���� �߰�
                direction.Normalize();
                if (direction.x >= 0)   // Ÿ���� �����ʿ� ���� ��
                {
                    DirX = 1;
                    spriteRenderer.flipX = false;
                    AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
                }
                else
                {
                    DirX = -1;
                    spriteRenderer.flipX = true;
                    AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
                }
                
                if((target.position.y - transform.position.y) <= 4f)   //Ÿ�ٰ� ��������� ���� ���̰� ������� ���� ����
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
                    StartCoroutine(Attack());
                }
            }
            else if(rayHit.collider == null)  //�ٴ��� ������ ���� ����
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
            Vector2 targetPosition = new Vector2(target.position.x - 1, target.position.y); // Ÿ���� �������� ���󰡱� ���� x-1, y-2�� ������
            Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
        }
        else if(distanceToTarget >= detectionRange) // Ÿ���� ���� �ۿ� ���� �� ����
        {
            istracking = false;
            Move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(enemy_Power);
                if (enemy_Type == 3)
                {

                }
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
        if (DirX != 0 && !isdie && !ishurt && !isattack)
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

        // �������� ��â���� ������ Ȯ���� �� �ְ� �׷���
        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));
        
        // ���� ������� �������� �Ʒ��� ��Ƽ� �������� ������ ����, LayMask.GetMask("")�� �ش��ϴ� ���̾ ��ĵ��
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Ground"));   //���̾�� �� ���̾ �������� �����ؾ���
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

    IEnumerator Attack()    //���� ���� �Լ�
    {
        atkPattern = Random.Range(1, 3);
        anim.SetTrigger("Attack");
        anim.SetFloat("Attackpatten", atkPattern);
        enemy_OriginSpeed = enemy_Speed;
        isattack = true;

        yield return new WaitForSeconds(0.7f);
        AttackBoxSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, AttackBoxSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(enemy_Power);
            }
        }
        yield return new WaitForSeconds(1.5f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        isattack = false;
    }

    IEnumerator FrogExplosion() //������ ���� ����ü ���� - �ִϸ��̼ǿ��� �����
    {
        int dirx = 1;
        GameObject Explosion = Instantiate(ExplosionPb, AttackBox.position, AttackBox.rotation);
        Animator anim = Explosion.GetComponent<Animator>();
        if (DirX != 0)
            dirx = DirX;
            Vector2 dir = new Vector2(dirx, 0);
        float DelTime = 2f;
        giveDmg = false;
        while (DelTime >= 0)
        {
            DelTime -= Time.deltaTime;
            if(!giveDmg)
                ExplosionGiveDamage(Explosion);
            Explosion.transform.Translate(dir * Time.deltaTime * 1); // �̵�
            yield return new WaitForEndOfFrame();
        }
        anim.SetBool("Explosion",true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Explosion", false);
        Destroy(Explosion); 
    }

    void ExplosionGiveDamage(GameObject explosion)  //����ü ����� ��� ����
    {
        AttackBoxSize = ExplosionPb.gameObject.transform.GetComponent<BoxCollider2D>();
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(explosion.transform.position, AttackBoxSize.size, 0);
        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(enemy_Power);
                giveDmg = true; //�� ���� ������� �ֱ� ���� ���
            }
        }
    }


    IEnumerator Hurt(Transform target)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        if(enemy_CurHP > 0 && !isdie && !ishurt)
        {
            ishurt = true;
            enemy_CurHP = enemy_CurHP - 1;
            StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 ������ �߰�
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            if(enemy_Speed > 0)
                enemy_OriginSpeed = enemy_Speed;
            enemy_Speed = 0;

            StartCoroutine(Blink());
            StartCoroutine(Knockback(target));
            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 ������ �߰�
                StartCoroutine(Die());
                Debug.Log("�׾���");
            }
        }

        yield return new WaitForSeconds(0.3f);
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
    }

    IEnumerator Knockback(Transform target)
    {
        Vector3 knockbackDirection = transform.position - target.position;  // �ǰݵ� ��ġ�� ����
        knockbackDirection.Normalize();

        float maxKnockbackDistance = 3f;    //�˹� ������ �ִ� �Ÿ�
        float knockbackDistance = 2.0f;  // �˹� �Ÿ��� ��Ÿ���� ����
        rigid.AddForce(knockbackDirection * knockbackDistance, ForceMode2D.Impulse);  // �ǰݵ� ��ġ * �˹� �Ÿ���ŭ�� ���� �˹鿡 ���
        float distanceTravelled = 0f;  // �̹� �̵��� �Ÿ��� ��Ÿ���� ����

        while (distanceTravelled < maxKnockbackDistance)
        {
            distanceTravelled += knockbackDistance * Time.fixedDeltaTime;  // �̵��� �Ÿ��� ����
            yield return new WaitForFixedUpdate();  // Fixed Update���� üũ�Ͽ� ���� �Ÿ������� �̵��ϵ��� ��
        }
        yield return new WaitForSeconds(0.3f);
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
