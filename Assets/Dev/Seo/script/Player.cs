using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private bool canChangeDirDuringDash = true;
    [SerializeField] float jumpforce = 500f;   
    //private float healingDuration = 5.0f;
    float moveX;
    public int JumpCount = 2;
    public bool isGround = false;
    public static Player instance;
    GameManager gm;
    Enemy enemy;
    Boss boss;
    Dash dashSc;
    HpBar hpBar;
    public Hitbox hitbox;
    public float maxHp = 100f;
    public float curHp = 100f;
    public float power;        // �÷��̾� ���ݷ�      // 2024-04-14 ������ �߰�
    public bool isBoosted = false;
    public float boostDuration = 0.1f;
    public float boostedSpeed = 10f;
    public float originalSpeed;
    public float spawnDistance = 1f;
    public GameObject hitboxPrefab;
    public GameObject effect1;
    public GameObject effect2;
    public bool isHealingActive = false;
    public bool isAttacking = false;
    public bool move;
    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private Animator animator;
    public float moveSpeed = 5f;
    private float lastHorizontalInput = 1;
    public SpriteRenderer sr;
    public Transform groundCheck;//����
    public float groundCheckRadius = 0.2f;//����
    private int currentJumpCount;//����
    public LayerMask whatIsGround;//����
    //����
    GameObject hitboxClone;//����
    //public float knockbackStrength = 500f;//�ǰ�
    GameObject holyArrowPrefab;         // 2024-04-13 ������ �߰� HolyArrow Skill Prefabs
    GameObject holyPillarPrefab;        // 2024-04-13 ������ �߰� HolyPillar Skill Prefabs 
    GameObject thunderPrefab;           // 2024-04-13 ������ �߰� Thunder Skill Prefabs
    GameObject atkBuffPrefab;           // 2024-04-14 ������ �߰� atkBuff Skill Prefabs    
    GameObject slashPrefab;            // 2024-04-14 ������ �߰� Slash1 Skill Prefabs    
    public int money;       // �÷��̾� ��� ������
    public bool isDead;     // �÷��̾� ��� ����

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
        currentJumpCount = JumpCount;
    }

    void StartDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void EndDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    /*void StartAttack()
    {
        hitboxClone = Instantiate(hitboxPrefab, transform.position, Quaternion.identity) as GameObject;
    }

    void StartAttackAnimation()
    {
        isAttacking = true;
        animator.SetTrigger("isAttack");
    }

    void EndSkillAnimation()
    {
        isAttacking = false;
    }*/
    void EndAttackAnimation()
    {
       isAttacking = false;

    }

    public void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Movement();
        float horizontalInput = Input.GetAxis("Horizontal");
        TestSkill();

        /*if (canChangeDirDuringDash)
        {
            lastHorizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            // �뽬 ���� �ƴ� ���� ���� ��ȯ �Ұ�
            lastHorizontalInput = Mathf.Clamp(lastHorizontalInput, -1f, 1f);
        }*/

        if (horizontalInput != 0)
        {
            lastHorizontalInput = horizontalInput;
        }

        if (Input.GetKey(KeyCode.DownArrow))//��ũ����
        {
            animator.SetBool("isCrawl", true);
        }
        else
        {
            animator.SetBool("isCrawl", false);
        }


        if (isGround)
        {
            currentJumpCount = JumpCount; // ���� ������ ���� Ƚ�� �ʱ�ȭ
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
        
        // �̵� ���¿� ���� �ִϸ����� ���� ����
        animator.SetBool("isRun", move);

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking)//����
        {
            StartCoroutine(ShowHitboxForDuration(0.5f));
        }

        if (!isAttacking) //�����̸� ����,��ųx
        {
            // Rigidbody�� �ӵ� �������� �̵� ����              
            Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }

        IEnumerator ShowHitboxForDuration(float duration)
        {
            isAttacking = true;
            animator.SetTrigger("isAttack");
            yield return new WaitForSeconds(duration);           

            Vector3 direction = lastHorizontalInput < 0 ? Vector3.left : Vector3.right;
            Vector3 spawnPosition = transform.position + direction * 2.0f;

            if (lastHorizontalInput < 0)
            {
                hitboxPrefab.transform.localScale = new Vector3(-Mathf.Abs(hitboxPrefab.transform.localScale.x), hitboxPrefab.transform.localScale.y, hitboxPrefab.transform.localScale.z);
            }
            else if (lastHorizontalInput > 0)
            {
                hitboxPrefab.transform.localScale = new Vector3(Mathf.Abs(hitboxPrefab.transform.localScale.x), hitboxPrefab.transform.localScale.y, hitboxPrefab.transform.localScale.z);
            }
            hitboxPrefab.transform.position = spawnPosition;
            hitboxPrefab.SetActive(true);

            yield return new WaitForSeconds(duration);
            hitboxPrefab.SetActive(false);


            if (isHealingActive)
            {
                float healAmount = hitbox.damage * 0.1f;
                curHp = Mathf.Clamp(curHp + healAmount, 0f, maxHp);
            }

            isAttacking = false;
        }

        if (Input.GetKey(KeyCode.E) && !isAttacking)//��ų1
        {
            StartCoroutine(ShowEffect1ForDuration(1.0f));
        }

        IEnumerator ShowEffect1ForDuration(float duration)
        {
            isAttacking = true;
            animator.SetTrigger("isSkill1");
            Vector3 direction = lastHorizontalInput < 0 ? Vector3.left : Vector3.right;
            Vector3 spawnPosition = transform.position + direction * 2.0f;

            if (lastHorizontalInput < 0)
            {
                effect1.transform.localScale = new Vector3(-Mathf.Abs(effect1.transform.localScale.x), effect1.transform.localScale.y, effect1.transform.localScale.z);
            }           
            else if (lastHorizontalInput > 0)
            {
                effect1.transform.localScale = new Vector3(Mathf.Abs(effect1.transform.localScale.x), effect1.transform.localScale.y, effect1.transform.localScale.z);
            }
            effect1.transform.position = spawnPosition;
            effect1.SetActive(true);

            yield return new WaitForSeconds(duration);
            effect1.SetActive(false);
            isAttacking = false;
        }

        if (Input.GetKey(KeyCode.D) && !isAttacking)//��ų2
        {
            StartCoroutine(ShowEffect2ForDuration(1.0f));
        }

        IEnumerator ShowEffect2ForDuration(float duration)
        {
            isAttacking = true;
            animator.SetTrigger("isSkill2");
            Vector3 direction = lastHorizontalInput < 0 ? Vector3.left : Vector3.right;
            Vector3 spawnPosition = transform.position + direction * 2.0f;

            effect2.transform.position = spawnPosition;
            effect2.SetActive(true);

            if (lastHorizontalInput < 0)
            {
                effect2.transform.localScale = new Vector3(-Mathf.Abs(effect2.transform.localScale.x), effect2.transform.localScale.y, effect2.transform.localScale.z);
            }
            else if (lastHorizontalInput > 0)
            {
                effect2.transform.localScale = new Vector3(Mathf.Abs(effect2.transform.localScale.x), effect2.transform.localScale.y, effect2.transform.localScale.z);
            }

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                effect2.transform.position += direction * Time.deltaTime * 10.0f; 
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            effect2.SetActive(false);
            isAttacking = false;
        }

        if (Input.GetKey(KeyCode.Alpha6) && !isAttacking)//��ų3
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isSkill3");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))//��ų4
        {
            isHealingActive = true;
            StartCoroutine(StopHealing());
        }

        IEnumerator StopHealing()
        {
            yield return new WaitForSeconds(5f);
            isHealingActive = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashSc.isFillingFirst) // �뽬
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isDash");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            //canChangeDirDuringDash = false;
        }

        IEnumerator BoostSpeedForDuration(float duration, float direction)
        {
            if (!isBoosted)
            {
                isBoosted = true;
                float dashDistance = 5f; // �뽬�� �Ÿ�
                Vector2 dashDirection = new Vector2(direction, 0f).normalized; // �뽬 ����
                Vector3 startPosition = transform.position; // ���� ��ġ
                Vector3 endPosition = startPosition + new Vector3(dashDirection.x, dashDirection.y, 0) * dashDistance; // ��ǥ ��ġ

                float elapsedTime = 0;
                while (elapsedTime < duration)
                {
 

                    transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                gameObject.layer = LayerMask.NameToLayer("Player");
       
               
                isBoosted = false;
                //canChangeDirDuringDash = true;
            }
        }     
    }
    public void Movement()
    {
        if (currentJumpCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.AddForce(Vector2.up * jumpforce);
                currentJumpCount--;
                animator.SetTrigger("isJump"); // ���� �ִϸ��̼� Ʈ���� ����
            }        
        }

        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }
    public void Playerhurt(int damage)//�ǰ�
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        hpBar.ChangeHp((int)curHp);
        isDead = true;
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
        yield return new WaitForSeconds(0);
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
        GameObject slash = Instantiate(slashPrefab, spawnPosition, Quaternion.identity, transform);
        yield return new WaitForSeconds(0.4f);
        BoxCollider2D boxCollider2D = slash.GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(0.3f);
        direction = sr.flipX ? 0.3f : -0.3f;
        slash.transform.position = new Vector2(direction, 0.8f);
        yield return new WaitForSeconds(0.3f);
        Destroy(slash);
    }
}
