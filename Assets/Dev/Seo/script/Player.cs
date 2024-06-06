using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private bool canChangeDirDuringDash = true;
    [SerializeField] float jumpforce = 500f;
    //public float knockbackForce = 5f; // �˹� ��
    //public float knockbackDuration = 0.2f;
    //private float healingDuration = 5.0f;
    //private float increasedMoveSpeed;
    public float damageAbsorptionBuffStartTime = 0f;
    public float damageAbsorptionBuffDuration = 5f;
    public float damageAbsorptionBuffCooldown = 30f;
    public bool isDamageAbsorptionBuffActive = false;
    private bool isDamageAbsorptionBuffOnCooldown = false;
    public float damageAbsorptionRate = 0.5f; // ������ ��� ����
    public float healingAmount;
    float moveX;
    public int JumpCount = 2;
    public bool isGround = false;
    public static Player instance;
    GameManager gm;
    DataManager dm;
    //Enemy enemy;
    //Boss boss;
    Dash dashSc;
    HpBar hpBar;
    public Hitbox hitbox;
    public float maxHp = 100f;
    public float curHp = 100f;
    public float power;        // �÷��̾� ���ݷ�      
    public bool isBoosted = false;
    public float boostDuration = 0.1f;
    public float boostedSpeed = 10f;
    public float originalSpeed;
    public float spawnDistance = 1f;
    public GameObject hitboxPrefab;
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4;
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
    //public float knockbackStrength = 500f;//�ǰ�
    GameObject holyArrowPrefab;         //  HolyArrow Skill Prefabs
    GameObject holyPillarPrefab;        //  HolyPillar Skill Prefabs 
    GameObject thunderPrefab;           //  Thunder Skill Prefabs
    GameObject atkBuffPrefab;           //  atkBuff Skill Prefabs    
    GameObject slashPrefab;            //  Slash1 Skill Prefabs    
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
    }
    public void Start()
    {
        gm = GameManager.instance;
        dm = DataManager.instance;
        dashSc = Dash.instance;
        hpBar = HpBar.instance;
        moveSpeed = dm.playerData.moveSpeed;        // �����͸Ŵ������� �� �ޱ�
        originalSpeed = moveSpeed;
        currentJumpCount = JumpCount;
        //StartCoroutine(RegenerateHealth());
        //StartCoroutine(IncreaseMovementSpeed());
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

        if (Input.GetKey(KeyCode.E) && !isAttacking)//��ų1
        {
            StartCoroutine(ShowEffect1ForDuration(1.0f));
        }



        if (Input.GetKey(KeyCode.D) && !isAttacking)//��ų2
        {
            StartCoroutine(ShowEffect2ForDuration(1.0f));
        }



        if (Input.GetKey(KeyCode.Alpha6) && !isAttacking)//��ų3
        {
            StartCoroutine(BoostSpeedForDurationskill(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isSkill3");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        if (Input.GetKey(KeyCode.Alpha8) && !isAttacking)//��ų4
        {
            StartCoroutine(ShowEffect4ForDuration(1.0f));
        }

        if (Input.GetKey(KeyCode.Alpha9) && !isAttacking)//��ų5
        {
            RecoverHealth();
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))//��ų6
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

        /*if (effect3.gameObject.activeSelf)
        {
            Debug.Log("effect3 ������Ʈ�� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
        }
        else
        {
            Debug.Log("effect3 ������Ʈ�� ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
        }*/


    }

    IEnumerator ShowHitboxForDuration(float duration)//��Ÿ
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
    /*private IEnumerator Knockback(Vector3 direction)
    {
        float elapsedTime = 0f;
        while (elapsedTime < knockbackDuration)
        {
            transform.Translate(-direction * knockbackForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }*/
    public void Playerhurt(int damage)//�ǰ�
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        StartCoroutine(ChangeLayerToPlayer());
        dm.playerData.curHpValue -= damage;
        hpBar.ChangeHp((int)curHp);

        // ������ ��� �нú� ȿ�� ����
        if (isDamageAbsorptionBuffActive)
        {
            healingAmount = damage * damageAbsorptionRate;
            curHp = Mathf.Clamp(curHp + healingAmount, 0, maxHp);
            dm.playerData.curHpValue = curHp;
            hpBar.ChangeHp((int)curHp);
        }

        if (curHp <= 0)
        {
            animator.SetTrigger("isDie");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(gm.ShowDeadUI());
        }
        else if (!isDamageAbsorptionBuffActive || Time.time - damageAbsorptionBuffStartTime >= damageAbsorptionBuffDuration)
        {
            if (!isDamageAbsorptionBuffOnCooldown)
            {
                StartCoroutine(DamageAbsorptionBuff());
            }
        }
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
                // �뽬 �� �浹 üũ
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, LayerMask.GetMask("Wall"));
                RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

                if (wallHit.collider != null || groundHit.collider != null)
                {
                    // ���̳� �ٴڿ� �ε����� �뽬 ����
                    transform.position = wallHit.collider != null ? wallHit.point : groundHit.point;
                    break;
                }

                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.layer = LayerMask.NameToLayer("Player");
            isBoosted = false;
        }
    }
    private IEnumerator ChangeLayerToPlayer()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    IEnumerator ShowEffect1ForDuration(float duration)//����Į��
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
    IEnumerator ShowEffect2ForDuration(float duration)//�˱�
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
    IEnumerator BoostSpeedForDurationskill(float duration, float direction)//�뽬����
    {
        if (!isBoosted)
        {
            isBoosted = true;
            moveSpeed = boostedSpeed;

            // �뽬 ��� �߰�
            float dashDistance = 5f; // �뽬�� �Ÿ�
            Vector2 dashDirection = new Vector2(direction, 0f).normalized; // �뽬 ����
            Vector3 startPosition = transform.position; // ���� ��ġ
            Vector3 endPosition = startPosition + new Vector3(dashDirection.x, dashDirection.y, 0) * dashDistance; // ��ǥ ��ġ

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                // �뽬 �� �浹 üũ
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, LayerMask.GetMask("Wall"));
                RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

                if (wallHit.collider != null || groundHit.collider != null)
                {
                    // ���̳� �ٴڿ� �ε����� �뽬 ����
                    transform.position = wallHit.collider != null ? wallHit.point : groundHit.point;
                    break;
                }

                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;

            }

            // effect3 ������Ʈ Ȱ��ȭ
            effect3.gameObject.SetActive(true);
            effect3.transform.position = transform.position;

            float effect3Duration = 0.5f; // effect3�� ���ӽð��� 1�ʷ� ����
            float effect3ElapsedTime = 0;
            while (effect3ElapsedTime < effect3Duration)
            {
                effect3ElapsedTime += Time.deltaTime;
                effect3.transform.position = transform.position; // effect3 ������Ʈ ��ġ ������Ʈ
                yield return null;
            }

            // effect3 ������Ʈ ��Ȱ��ȭ
            effect3.gameObject.SetActive(false);

            // �ν�Ʈ ���� �ʱ�ȭ
            isBoosted = false;
            moveSpeed = originalSpeed;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    IEnumerator ShowEffect4ForDuration(float duration)//���̾Ʈ
    {
        isAttacking = true;
        animator.SetTrigger("isSkill4");
        Vector3 direction = lastHorizontalInput < 0 ? Vector3.left : Vector3.right;
        Vector3 spawnPosition = transform.position + direction * 2.0f;

        effect4.transform.position = spawnPosition;
        effect4.SetActive(true);

        if (lastHorizontalInput < 0)
        {
            effect4.transform.localScale = new Vector3(-Mathf.Abs(effect4.transform.localScale.x), effect4.transform.localScale.y, effect4.transform.localScale.z);
        }
        else if (lastHorizontalInput > 0)
        {
            effect4.transform.localScale = new Vector3(Mathf.Abs(effect4.transform.localScale.x), effect4.transform.localScale.y, effect4.transform.localScale.z);
        }

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            effect4.transform.position += direction * Time.deltaTime * 10.0f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effect4.SetActive(false);
        isAttacking = false;
    }

    void RecoverHealth()//����ų
    {
        curHp = Mathf.Min(maxHp, curHp + 10f);
    }

    IEnumerator RegenerateHealth()//�� �нú�
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            curHp = Mathf.Min(maxHp, curHp + 5f);
        }
    }

    IEnumerator IncreaseMovementSpeed()//�̼� �нú�
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            moveSpeed += 2f;
        }
    }

    private IEnumerator DamageAbsorptionBuff()//������ ���
    {
        isDamageAbsorptionBuffActive = true;
        isDamageAbsorptionBuffOnCooldown = true;
        damageAbsorptionBuffStartTime = Time.time;

        yield return new WaitForSeconds(damageAbsorptionBuffDuration);

        isDamageAbsorptionBuffActive = false;

        yield return new WaitForSeconds(damageAbsorptionBuffCooldown);
        isDamageAbsorptionBuffOnCooldown = false;
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
    public IEnumerator HolyArrowSkill() // HolyArrow ��ų ���� �Լ� 
    {
        isAttacking = true;
        float direction = sr.flipX ? -3.5f : 3.5f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, 0f, 0f);

        animator.SetTrigger("isArrow");
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
        isAttacking = false;
    }

    public IEnumerator HolyPillarSkill()    // HolyPillar ��ų ���� �Լ� 
    {
        isAttacking = true;
        float direction = sr.flipX ? -3.5f : 3.5f;

        Vector3 spawnPosition = this.transform.position + new Vector3(direction, 1.5f, 0f);
        animator.SetTrigger("isPillar");
        yield return new WaitForSeconds(0.5f);
        Instantiate(holyPillarPrefab, spawnPosition, Quaternion.identity);
        isAttacking = false;
    }

    public IEnumerator ThunderSkill()       // Thunder ��ų ���� �Լ� 
    {
        isAttacking = true;
        float direction = sr.flipX ? -3.5f : 3.5f;
        Vector3 spawnPosition = transform.position + new Vector3(direction, 0.4f, 0);
        Vector3 addPosition = new Vector3(direction, 0, 0);

        animator.SetTrigger("isThunder");

        for (int i = 0; i < 3; i++)
        {
            Instantiate(thunderPrefab, spawnPosition + (addPosition * i), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        isAttacking = false;
    }

    public IEnumerator AtkBuffSkill()      // AtkBuff ��ų ���� �Լ� 
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

    public IEnumerator SlashSkill()     // Slash �нú� ���� �Լ� 
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log("ȹ��");
            money += Random.Range(1, 11); // ���ΰ� �浹 �� ���� 1���� 10���� �����ϰ� ����
            Destroy(collision.gameObject); // ���� ������Ʈ ����
        }
        else if (collision.gameObject.CompareTag("Potion"))
        {
            curHp = Mathf.Min(curHp + 20f, maxHp); // ���ǰ� �浹 �� ü�� ȸ��, �ִ� ü�� �ʰ� ����
            Destroy(collision.gameObject); // ���� ������Ʈ ����
        }
        else if (collision.gameObject.CompareTag("SkillItem"))
        {
            int randomStat = Random.Range(0, 3);
            switch (randomStat)
            {
                case 0:
                    power += 5f; // ���ݷ� ����
                    break;
                case 1:
                    moveSpeed += 1f; // �̵� �ӵ� ����
                    break;
                case 2:
                    groundCheckRadius += 1; // ���� Ƚ�� ����
                    break;
            }
            Destroy(collision.gameObject); // ��ų ������ ������Ʈ ����
        }
    }// 5.22 �̰���߰�
}
