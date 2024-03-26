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
    Transform PbSpawn;
    public GameObject ArrowPb; // ȭ�� ������

    bool bossMoving = true;
    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float playerLoc; // player�� X��ǥ
    public float bossLoc;  // boss�� X��ǥ

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
        //rigid = this.GetComponent<Rigidbody2D>();
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
        {
            anim.SetBool("Move", false);
        }
    }

    public void bossAttack()
    {
        if (playerLoc < bossLoc)
        {
            spriteRenderer.flipX = true;
            DirX = -1;
        }
        else if(playerLoc > bossLoc)
        {
            spriteRenderer.flipX = false;
            DirX = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            bossMoving = false;
            anim.SetTrigger("Attack");
            Ranger_Arrowattack();
        }
            

        if (playerLoc - bossLoc < 0.2f)
            bossMoving = false;
        else
            bossMoving = true;
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

    void Ranger_Arrowattack()
    {
        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();

        APb.Power = boss_Power;
        APb.Dir = DirX;
        APb.DelTime = 3f;
    }

    public abstract void BossInitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)
}
