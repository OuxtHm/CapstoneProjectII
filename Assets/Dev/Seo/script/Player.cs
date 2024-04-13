using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    GameManager gm;
    Dash dashSc;
    HpBar hpBar;
    public float maxHp = 100f;
    public float curHp = 100f;
    public float power;        // �÷��̾� ���ݷ�      // 2024-04-14 ������ �߰�
    public bool isBoosted = false;
    public float boostDuration = 0.1f;
    public float boostedSpeed = 20f; // ������ �̵� �ӵ� ��
    public float originalSpeed;
    public GameObject hitboxPrefab;
    public bool isAttacking = false;
    public bool move;
    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private Animator animator;
    public float jumpForce = 550f;
    public float moveSpeed = 4f;
    private float lastHorizontalInput = 1;       
    public SpriteRenderer sr;       // 2024-04-13 private -> public ������
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public int maxJumpCount = 1; 
    private int currentJumpCount;
    public LayerMask whatIsGround;
    public bool isGround;
    GameObject hitboxClone;
    GameObject holyArrowPrefab;         // 2024-04-13 ������ �߰� HolyArrow Skill Prefabs
    GameObject holyPillarPrefab;        // 2024-04-13 ������ �߰� HolyPillar Skill Prefabs 
    GameObject thunderPrefab;           // 2024-04-13 ������ �߰� Thunder Skill Prefabs
    GameObject atkBuffPrefab;           // 2024-04-14 ������ �߰� atkBuff Skill Prefabs    
    GameObject slashPrefab;            // 2024-04-14 ������ �߰� Slash1 Skill Prefabs    
    public int money;           // �÷��̾� ��� ������
    

    public void Awake()
    {
        instance = this;
        holyArrowPrefab = Resources.Load<GameObject>("Prefabs/Skill/HolyArrow");
        holyPillarPrefab = Resources.Load<GameObject>("Prefabs/Skill/HolyPillar");
        thunderPrefab = Resources.Load<GameObject>("Prefabs/Skill/Thunder");
        atkBuffPrefab = Resources.Load<GameObject>("Prefabs/Skill/AtkBuff");
        slashPrefab = Resources.Load<GameObject>("Prefabs/Skill/Slash");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        maxHp = 100f;
        curHp = 100f;
        money = 9999;

    }
    public void Start()
    {
        gm = GameManager.instance;
        dashSc = Dash.instance;
        hpBar = HpBar.instance;
        originalSpeed = moveSpeed;
        currentJumpCount = maxJumpCount;
    }
    void StartDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void EndDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    void StartAttack()
    {
        hitboxClone = Instantiate(hitboxPrefab, transform.position, Quaternion.identity) as GameObject;
    }
    void StartAttackAnimation()
    {
        isAttacking = true;
        animator.SetTrigger("isAttack");
    }

    void EndAttackAnimation()
    {
       isAttacking = false;
       Destroy(hitboxClone);
    }

    private void OnCollisionEnter2D(Collision2D collision) //�ǰ�
    {
        if (collision.gameObject.tag == "Enemy")
        {
            curHp -= 10;
            if (curHp > 0)
            {
                animator.SetTrigger("isHit");
            }
            else
            {
                animator.SetTrigger("isDie");
            }
            Vector2 pushDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
            rb.AddForce(pushDirection * 500);
        }
    }
    // Update is called once per frame
    public void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        float horizontalInput = Input.GetAxis("Horizontal");
        TestSkill();

        if (horizontalInput != 0)
        {
            lastHorizontalInput = horizontalInput;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isCrawl", true);
        }
        else
        {
            animator.SetBool("isCrawl", false);
        }

        if (isGround)
        {
            currentJumpCount = maxJumpCount; // ���� ������ ���� Ƚ�� �ʱ�ȭ
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentJumpCount > 0)
            {
                animator.SetTrigger("isJump");
                rb.AddForce(new Vector2(0, jumpForce));
                currentJumpCount--; // �����ϸ� ���� Ƚ�� ����
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking)
        {
            StartAttackAnimation();
        }
        // �̵� ���� ����
        move = horizontalInput != 0;

        // ���� ��ȯ ����
        if (horizontalInput < 0)
        {
            sr.flipX = true; // ������ �ٶ󺸰� ��
            cc.offset = new Vector2(Mathf.Abs(cc.offset.x), cc.offset.y);
        }
        else if (horizontalInput > 0)
        {
            sr.flipX = false; // �������� �ٶ󺸰� ��
            cc.offset = new Vector2(-Mathf.Abs(cc.offset.x), cc.offset.y);
        }

        // Rigidbody�� �ӵ� �������� �̵� ����              
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // �̵� ���¿� ���� �ִϸ����� ���� ����
        animator.SetBool("isRun", move);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashSc.isFillingFirst)
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput)); // ���⿡ lastHorizontalInput�� ����
            animator.SetTrigger("isDash");
        }

    }
    public void Playerhurt(int damage)
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        hpBar.ChangeHp((int)curHp);
        Debug.Log("���� ����:" + damage);

        if (curHp <= 0)
        {
            animator.SetTrigger("isDie");
            StartCoroutine(gm.ShowDeadUI());        // 2024-04-13 ������ �߰� *******************************************
        }
    }
    IEnumerator BoostSpeedForDuration(float duration, float direction) // ���� �߰��� �޼ҵ�
    {
        if (!isBoosted)
        {
            isBoosted = true;
            moveSpeed = boostedSpeed; // �̵� �ӵ��� ������Ŵ

            StartDash();

            Vector2 dashDirection = new Vector2(direction, 0f); // �뽬 ���� ����
            rb.velocity = dashDirection.normalized * boostedSpeed;

            yield return new WaitForSeconds(duration);

            moveSpeed = originalSpeed;
            isBoosted = false;

            EndDash();
        }
    }
    void TestSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(HolyArrowSkill());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(HolyPillarSkill());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(ThunderSkill());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(AtkBuffSkill());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(SlashSkill());
        }
    }
    public IEnumerator HolyArrowSkill() // HolyArrow ��ų ���� �Լ� 2024-04-13 ������ �߰�
    {
        float direction = sr.flipX ? -3.5f : 3.5f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, 0f, 0f);

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.5f); 

        for (int i = 0; i < 2; i++)
        {
            Instantiate(holyArrowPrefab, spawnPosition, Quaternion.identity);
            if (i < 1) 
            {
                yield return new WaitForSeconds(0.4f); 
            }
        }
    }

    public IEnumerator HolyPillarSkill()    // HolyPillar ��ų ���� �Լ� 2024-04-13 ������ �߰�
    {
        float direction = sr.flipX ? -3.5f : 3.5f;

        Vector3 spawnPosition = this.transform.position + new Vector3(direction, 1.5f, 0f);
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.5f);
        Instantiate(holyPillarPrefab, spawnPosition, Quaternion.identity);
    }

    public IEnumerator ThunderSkill()       // Thunder ��ų ���� �Լ� 2024-04-13 ������ �߰�
    {
        float direction = sr.flipX ? -3.5f : 3.5f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, 0.4f, 0);
        Vector3 addPosition = new Vector3(direction, 0, 0);

        animator.SetTrigger("isAttack");

        for (int i = 0; i < 3; i++)
        {
            Instantiate(thunderPrefab, spawnPosition + (addPosition * i), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public IEnumerator AtkBuffSkill()      // AtkBuff ��ų ���� �Լ� 2024-04-14 ������ �߰�
    {
        float direction = sr.flipX ? 0.3f : -0.3f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, 0, 0);
        GameObject atkBuff = Instantiate(atkBuffPrefab, spawnPosition, Quaternion.identity, transform);
        power += 20f;
        yield return new WaitForSeconds(0.8f);
        Destroy(atkBuff);
        yield return new WaitForSeconds(19.2f);
        power -= 20f;
    }

    public IEnumerator SlashSkill()     // Slash �нú� ���� �Լ� 2024-04-14 ������ �߰�
    {
        float direction = sr.flipX ? 1f : -1f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, -0.6f, 0);
        animator.SetTrigger("isAttack");
        GameObject slash = Instantiate(slashPrefab, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);
        direction = sr.flipX ? 0.3f : -0.3f;
        slash.transform.position = new Vector2(direction, 0.8f);
        yield return new WaitForSeconds(0.3f);
        Destroy(slash);
    }
}
