using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Boss : MonoBehaviour
{
    public static Boss Instance;
    Player player;
    BossHpBar bossHpBar;        // 2024-04-10 ������ �߰�
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform PbSpawn;  //������ ���� ��ġ ������Ʈ
    Transform AttackBox;    // ���� ���� ���� ������Ʈ
    BoxCollider2D BoxCollider2DSize;    //Attackbox ������Ʈ�� boxcollider2D

    public bool ishurt = false;
    bool bossMoving = true;
    bool isdie = false;
    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float playerLoc; // player�� X��ǥ
    public float bossLoc;  // boss�� X��ǥ
    public int atkPattern = 0; //boss ���� ����
    int boss_OriginSpeed;   //������ ���� �ӵ��� ����
    float distanceToTarget; //�÷��̾�� ���� ������ �Ÿ�
    public int turnPoint = 1;    // ���� ���� �� �̵� ��� ���� ����
    int countRange; //���� �� ���� ����
    int totalDamage;    // ���� �������

    [Header("���� ���� �ɷ�ġ")]
    public int boss_stage;  //������ �������� ����
    public float boss_MaxHP; //���� �ִ�ü��
    public float boss_CurHP; //���� ����ü��
    public int boss_Power; //���� ���ݷ�
    public int boss_Speed; //���� �̵��ӵ�
    public int boss_BumpPower;    //�浹 �����
    public int boss_OnePattenPower;   //ù��° ���� ���� �����
    public int boss_TwoPattenPower;   //�ι�° ���� ���� �����
    public int boss_ThreePattenPower;    //����° ���� ���� �����
    public int boss_FourPattenPower;   //�׹�° ���� ���� �����

    [Header("1�������� ���� ������")]
    public GameObject ArrowPb; // 1�������� ���� ȭ�� ������
    public GameObject ArrowrainPb; // 1�������� ���� ȭ��� ������
    public GameObject LaserPb; // 1�������� ���� ȭ��� ������
    public GameObject WarringPb;  //���� �� ����ǥ�� ������

    [Header("2�������� ���� ������")]
    public GameObject SwordEffectPb; // 2�������� ���� ���κ��� ������
    public GameObject FireEffectPb; // 2�������� ���� ���̾ ������

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        bossHpBar = BossHpBar.instance;     // 2024-04-10 ������ �߰�
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
    }

    public virtual void BossUpdate(Transform target)  // boss�� Update��
    {
        playerLoc = target.position.x;
        bossLoc = this.gameObject.transform.position.x;
        distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // �Ÿ��� ���̸� ���밪���� ����
        bossMove();
        bossAttack();
    }

    public void bossMove()  // boss�� �����̵��� �ϴ� �Լ�
    {
        if(boss_stage == 1)   //���� ���� �� �÷��̾������� �̵�
        {
            if (bossMoving && !isdie && !ishurt)
            {
                if(turnPoint == 1 && distanceToTarget <= 15f)
                {
                    gameObject.transform.Translate(new Vector2(-DirX, 0) * Time.deltaTime * boss_Speed);
                    if (DirX == 1)
                        spriteRenderer.flipX = true;
                    else
                        spriteRenderer.flipX = false;
                }
                else if(turnPoint == -1)
                    gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);

                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
        else if(boss_stage == 2)
        {
            if(bossMoving)
            {
                if (playerLoc < bossLoc)
                {
                    spriteRenderer.flipX = true;
                    DirX = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    DirX = 1;
                }
            }
        }
    }
    public void bossAttack()
    {
        if(!isdie)
        {
            if (playerLoc < bossLoc && !bossMoving)
            {
                spriteRenderer.flipX = true;
                DirX = -1;
                if(boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x - 1.6f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
            }
            else if (playerLoc > bossLoc && !bossMoving)
            {
                spriteRenderer.flipX = false;
                DirX = 1;
                if (boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x + 1.6f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
            }
            if(boss_stage == 1)
            {
                if (atkPattern == 1)
                    atkPattern = Random.Range(2, countRange);
                switch (atkPattern)
                {
                    case -1: //1�� �������� �������� ����� ���� ����
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 1);
                        totalDamage = boss_OnePattenPower;
                        atkPattern = 0;
                        break;

                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        //Ranger_Arrowattack(); �ִϸ��̼ǿ��� ����ǰ� ������
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        StartCoroutine(Ranger_Arrowrain());
                        atkPattern = 0;
                        break;

                    case 4:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 4);
                        StartCoroutine(Ranger_Laserattack());
                        atkPattern = 0;
                        break;
                }
            }
            if (boss_stage == 2)
            {
                switch (atkPattern)
                {
                    case 1:
                        bossMoving = false;
                        anim.SetTrigger("Move");
                        Invoke("MoveOn", 3f);
                        atkPattern = 0;
                        break;
                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        atkPattern = 0;
                        break;

                    case 4:
                        //bossMoving = false;
                        Debug.Log(atkPattern);
                        atkPattern = 0;
                        break;
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player")) //�浹 ����� ó��
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null && boss_CurHP > 0)
            {
                player.Playerhurt(boss_BumpPower);
            }
        }

        if (collision != null && collision.gameObject.CompareTag("wall"))   //�� �浹 ó��
        {
            turnPoint *= -1;
            DirX *= -1;
        }
    }
    
    public void randomAtk() // ���� ���� �������� ���ϱ�
    {
        atkPattern = Random.Range(1, countRange);    // 2 ~ (countRange - 1) ������ ���� �������� ����
        if (boss_stage == 1)    //������ �÷��̾��� �Ÿ��� 4��ŭ �̳��� ������ ���� ���� Ȯ��
            if(DirX == 1 && (playerLoc - bossLoc) <= 4f || (DirX == -1 && (playerLoc - bossLoc) >= -4f))
                atkPattern = -1;
        if (!isdie && boss_CurHP > boss_MaxHP / 2)  // 1������� 2�������� ���� ���� �ð��� �ٸ�
        {
            Invoke("randomAtk", 5f);
            countRange = 4;
        }  
        else
        {
            Invoke("randomAtk", 4f);
            countRange = 5;
        }
            
    }

    void Ranger_Arrowattack()   //1stage Ȱ ��� ����
    {
        EffectPb APb = ArrowPb.GetComponent<EffectPb>();
        APb.Power = boss_TwoPattenPower;
        APb.dir = DirX;
        APb.DelTime = 3f;
        APb.movecheck = 1;
        APb.speed = 20;

        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);

        Invoke("MoveOn", 3f);
    }

    IEnumerator Ranger_Arrowrain()  //1stage ȭ��� ����
    {
        EffectPb ArPb = ArrowrainPb.GetComponent<EffectPb>();
        Vector2 Targetpos = new Vector2(player.transform.position.x, PbSpawn.position.y + 1.1f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        Vector2 Warringpos = new Vector2(player.transform.position.x, PbSpawn.position.y - 2.1f);  //���� ǥ�� ���� ��ġ
        ArPb.Power = boss_ThreePattenPower;
        ArPb.dir = DirX;
        ArPb.DelTime = 1.1f;
        ArPb.movecheck = 0;

        GameObject Warring = Instantiate(WarringPb, Warringpos, PbSpawn.rotation);  //���� ǥ�� ����
        yield return new WaitForSeconds(1.5f);
        GameObject arrowrain = Instantiate(ArrowrainPb, Targetpos, PbSpawn.rotation);//ȭ��� ���� ����

        Destroy(Warring);
        Invoke("MoveOn", 4.5f);
    }

    IEnumerator Ranger_Laserattack()    //1stage ������ ����
    {
        Vector2 newPosition = new Vector2(this.transform.position.x + (DirX * 9.8f), this.transform.position.y + 0.45f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        EffectPb LrPb = LaserPb.GetComponent<EffectPb>();
        LrPb.Power = boss_FourPattenPower;
        LrPb.dir = DirX;
        LrPb.DelTime = 0.3f;
        LrPb.movecheck = 0;

        yield return new WaitForSeconds(0.9f);
        GameObject arrowlaser = Instantiate(LaserPb, newPosition, PbSpawn.rotation);

        Invoke("MoveOn", 5f);
    }

    void giveDamage()   //�ִϸ��̼ǿ��� ����Ǵ� ������� �Ѱ��ִ� �Լ�
    {
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = collider.GetComponent<Player>();
                player.Playerhurt(totalDamage);
            }
        }
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator Knight_Transparent()  //2stage ���� ���� , SwordAttack 1
    {
        float fadeDuration = 0.8f;  // ��ȭ�� �ɸ��� �ð�
        float elapsedTime = 0f;
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0f, elapsedTime / fadeDuration));  // ���İ� ������ ����
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.position = new Vector2(player.transform.position.x + (DirX > 0 ? -2 : 2), player.transform.position.y);
        yield return new WaitForSeconds(2f);
        this.gameObject.layer = LayerMask.NameToLayer("Boss");
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetTrigger("Attack");
        anim.SetFloat("Attackpatten", 1);
        totalDamage = boss_OnePattenPower;
    }

    IEnumerator Knight_SwordAttack2()  //2stage ���κ��� ����, SwordAttack 2
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x , this.PbSpawn.position.y);
        this.gameObject.transform.position = new Vector2(transform.position.x + (DirX > 0 ? 10 : -10), transform.position.y);

        yield return new WaitForSeconds(0.4f);
        EffectPb SEfPb = SwordEffectPb.GetComponent<EffectPb>();
        SEfPb.dir = DirX;
        SEfPb.Power = boss_TwoPattenPower;
        SEfPb.DelTime = 0.6f;
        SEfPb.movecheck = 0;

        GameObject effect = Instantiate(SwordEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }
    IEnumerator Knight_LightAttack()
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x + DirX, this.PbSpawn.position.y);

        yield return new WaitForSeconds(0.1f);
        EffectPb FirePb = FireEffectPb.GetComponent<EffectPb>();
        FirePb.dir = DirX;
        FirePb.Power = boss_ThreePattenPower;
        FirePb.DelTime = 1f;
        FirePb.movecheck = 1;
        FirePb.speed = 15;

        GameObject effect = Instantiate(FireEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }
    
    public IEnumerator Hurt(Transform target, float Damage)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        yield return new WaitForSeconds(0);
        if (boss_CurHP > 0 && !ishurt)
        {
            ishurt = true;
            boss_CurHP = boss_CurHP - Damage;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 ������ �߰�
            //bossHpBar.anim.SetTrigger("Damage");
            StartCoroutine(Blink());

            if (boss_Speed > 0)
                boss_OriginSpeed = boss_Speed;
            boss_Speed = 0;

            if (boss_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 ������ �߰�
                //bossHpBar.anim.SetTrigger("Remove");
                StartCoroutine(Die());
            }
            Invoke("OriginSpeed", 0.3f);
        }
        ishurt = false;
    }
    IEnumerator Blink() // �ǰ� ȿ��
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    IEnumerator Die()  //������ �׾��� ����
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        DirX = 0;
        bossMoving = false;
        anim.SetTrigger("Die");
        anim.SetBool("Move", false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    void MoveOn()
    {
        if(!isdie)
            bossMoving = true;
    }
    void OriginSpeed()  //���� ���� �̵��ӵ��� �����ϴ� �Լ�
    {
        boss_Speed = boss_OriginSpeed;
    }



    public abstract void BossInitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
