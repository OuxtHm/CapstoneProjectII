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
    public GameObject ArrowPb; // ȭ�� ������
    public GameObject ArrowrainPb; // ȭ��� ������

    bool bossMoving = true;
    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float playerLoc; // player�� X��ǥ
    public float bossLoc;  // boss�� X��ǥ
    public int atkPattern; //boss ���� ����

    [Header("���� ���� �ɷ�ġ")]
    public int boss_MaxHP; //���� �ִ�ü��
    public int boss_CurHP; //���� ����ü��
    public int boss_Power; //���� ���ݷ�
    public int boss_Speed; //���� �̵��ӵ�

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
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
        if (bossMoving)
        {
            gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);
            anim.SetBool("Move", true);
        }
        else
            anim.SetBool("Move", false);
    }

    public void bossAttack()
    {
        if (playerLoc < bossLoc)
        {
            spriteRenderer.flipX = true;
            DirX = -1;
            AttackBox.position = new Vector2(transform.position.x - 1.6f, -2);

        }
        else if(playerLoc > bossLoc)
        {
            spriteRenderer.flipX = false;
            DirX = 1;
            AttackBox.position = new Vector2(transform.position.x + 1.6f, -2);
        }

        switch (atkPattern)
        {
            case 1:
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 1);
                //Ranger_Arrowattack(); �ִϸ��̼ǿ��� ����ǰ� ������
                atkPattern = 0;
                break;

            case 2:
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 2);
                Ranger_Normalattack();
                atkPattern = 0;
                break;

            case 3:
                bossMoving = false;
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 3);
                Ranger_Arrowrain();
                atkPattern = 0;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(boss_Power);
            }
            else
                Debug.Log("�÷��̾ �� �ҷ���");
        }
    }
    public void randomAtk() // ���� ���� �������� ���ϱ�
    {
        Debug.Log("atkPattern");
        atkPattern = Random.Range(1, 4);
        Invoke("randomAtk", 4f);
    }

    void Ranger_Arrowattack()   //Ȱ ��� ����
    {
        bossMoving = false;
        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();

        APb.Power = boss_Power;
        APb.Dir = DirX;
        APb.DelTime = 3f;
        Invoke("MoveOn", 3f);
    }

    void Ranger_Normalattack()  //ȭ��� ��� ���� ����
    {
        bossMoving = false;
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(boss_Power);
            }
        }
        Invoke("MoveOn", 3f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    void Ranger_Arrowrain()
    {
        bossMoving = false;

        Vector3 newPosition = new Vector3(PbSpawn.position.x + 6f, PbSpawn.position.y + 1.2f, PbSpawn.position.z);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        GameObject arrowrain = Instantiate(ArrowrainPb, newPosition, PbSpawn.rotation);
        ArrowPb ArPb = arrowrain.GetComponent<ArrowPb>(); // ���� ����: ArrowPb -> arrowrain

        ArPb.Power = boss_Power;
        ArPb.Dir = DirX;
        ArPb.DelTime = 10f;
        Invoke("MoveOn", 3f);
    }

    void MoveOn()
    {
        bossMoving = true;
    }

    public abstract void BossInitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
