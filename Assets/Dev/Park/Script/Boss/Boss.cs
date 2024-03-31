using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Boss : MonoBehaviour
{
    public static Boss Instance;
    Player player;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform PbSpawn;  //������ ���� ��ġ ������Ʈ
    Transform AttackBox;    // ���� ���� ���� ������Ʈ
    BoxCollider2D BoxCollider2DSize;    //Attackbox ������Ʈ�� boxcollider2D

    bool ishurt = false;
    bool bossMoving = true;
    bool isdie = false;
    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float playerLoc; // player�� X��ǥ
    public float bossLoc;  // boss�� X��ǥ
    public int atkPattern; //boss ���� ����
    int boss_OriginSpeed;   //������ ���� �ӵ��� ����

    [Header("���� ���� �ɷ�ġ")]
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Hurt(AttackBox));
    }

    public virtual void BossUpdate(Transform target)  // boss�� Update��
    {
        playerLoc = target.position.x;
        bossLoc = this.gameObject.transform.position.x;
        bossMove();
        bossAttack();
    }

    public void bossMove()  // boss�� �����̵��� �ϴ� �Լ�
    {
        if (bossMoving && !isdie)
        {
            gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
            
    }
    
    /*
    void bossRoll()
    {
        anim.SetTrigger("Roll");
        float moveDistance = DirX * boss_Speed * 5 * Time.deltaTime;
        gameObject.transform.Translate(new Vector2(moveDistance, 0));
        Debug.Log("������");
    }
    */
    public void bossAttack()
    {
        if(!isdie)
        {
            if (playerLoc < bossLoc && bossMoving)
            {
                spriteRenderer.flipX = true;
                DirX = -1;
                AttackBox.position = new Vector2(transform.position.x - 1.6f, transform.position.y - 3f);
            }
            else if (playerLoc > bossLoc && bossMoving)
            {
                spriteRenderer.flipX = false;
                DirX = 1;
                AttackBox.position = new Vector2(transform.position.x + 1.6f, transform.position.y - 3f);
            }

            switch (atkPattern)
            {
                case 1: //1�� ������������ �������� ����� ���� ����
                    bossMoving = false;
                    anim.SetTrigger("Attack");
                    anim.SetFloat("Attackpatten", 1);
                    Ranger_Normalattack();
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player")) //�浹 ����� ó��
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(boss_BumpPower);
            }
        }
    }
    public void randomAtk() // ���� ���� �������� ���ϱ�
    {
        atkPattern = Random.Range(2, 5);    // 2~4 ������ ���� �������� ����
        if (DirX == 1 && (playerLoc - bossLoc) <= 4f || (DirX == -1 && (playerLoc - bossLoc) >= -4f))    //������ �÷��̾��� �Ÿ��� 4��ŭ �̳��� ������ ���� ���� Ȯ��
            atkPattern = 1;
        if (!isdie && boss_CurHP > boss_MaxHP / 2)  // 1������� 2�������� ���� ���� �ð��� �ٸ�
            Invoke("randomAtk", 5f);
        else
            Invoke("randomAtk", 3.5f);
    }

    void Ranger_Normalattack()  //���� ����
    {
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(boss_OnePattenPower);
            }
        }
        Invoke("bossRoll", 0.5f);
        Invoke("MoveOn", 2f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    void Ranger_Arrowattack()   //Ȱ ��� ����
    {
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();
        APb.Power = boss_TwoPattenPower;
        APb.Dir = DirX;
        APb.DelTime = 3f;
        APb.Arrowpatten = 1;

        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);

        Invoke("MoveOn", 2.5f);
    }

    IEnumerator Ranger_Arrowrain()  //ȭ��� ����
    {
        ArrowPb ArPb = ArrowrainPb.GetComponent<ArrowPb>();
        Vector2 newPosition = new Vector2(player.transform.position.x, PbSpawn.position.y + 1.1f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        ArPb.Power = boss_ThreePattenPower;
        ArPb.Dir = DirX;
        ArPb.DelTime = 1.5f;
        ArPb.Arrowpatten = 2;

        yield return new WaitForSeconds(1.5f);
        GameObject arrowrain = Instantiate(ArrowrainPb, newPosition, PbSpawn.rotation);
       
        Invoke("MoveOn", 2.5f);
    }

    IEnumerator Ranger_Laserattack()    //������ ����
    {
        Vector2 newPosition = new Vector2(PbSpawn.position.x + (DirX * 9.4f), PbSpawn.position.y - 0.07f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        ArrowPb LrPb = LaserPb.GetComponent<ArrowPb>();
        LrPb.Power = boss_FourPattenPower;
        LrPb.Dir = DirX;
        LrPb.DelTime = 0.3f;
        LrPb.Arrowpatten = 3;

        yield return new WaitForSeconds(0.9f);
        GameObject arrowrain = Instantiate(LaserPb, newPosition, PbSpawn.rotation);

        Invoke("MoveOn", 2.5f);
    }
    
    IEnumerator Hurt(Transform target)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        if (boss_CurHP > 0 && !ishurt)
        {
            ishurt = true;
            boss_CurHP = boss_CurHP - 10;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");

            if (boss_Speed > 0)
                boss_OriginSpeed = boss_Speed;
            boss_Speed = 0;

            if (boss_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(Die());
                Debug.Log("�׾���");
            }
        }

        yield return new WaitForSeconds(0.3f);
        boss_Speed = boss_OriginSpeed;
        ishurt = false;
    }

    IEnumerator Die()  //���Ͱ� �׾��� ����
    {
        DirX = 0;
        bossMoving = false;
        anim.SetBool("Move", false);
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    void MoveOn()
    {
        bossMoving = true;
    }

    public abstract void BossInitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
