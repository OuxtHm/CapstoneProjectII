using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private bool canChangeDirDuringDash = true;
    [SerializeField] float jumpforce = 500f;
    //public float knockbackForce = 5f; // 넉백 힘
    //public float knockbackDuration = 0.2f;
    //private float healingDuration = 5.0f;
    //private float increasedMoveSpeed;
    public float damageAbsorptionBuffStartTime = 0f;
    public float damageAbsorptionBuffDuration = 5f;
    public float damageAbsorptionBuffCooldown = 30f;
    public bool isDamageAbsorptionBuffActive = false;
    private bool isDamageAbsorptionBuffOnCooldown = false;
    public float damageAbsorptionRate = 0.5f; // 데미지 흡수 비율
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
    public float power;        // 플레이어 공격력      
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
    public Transform groundCheck;//점프
    public float groundCheckRadius = 0.2f;//점프
    private int currentJumpCount;//점프
    public LayerMask whatIsGround;//점프
    //public float knockbackStrength = 500f;//피격
    GameObject holyArrowPrefab;         //  HolyArrow Skill Prefabs
    GameObject holyPillarPrefab;        //  HolyPillar Skill Prefabs 
    GameObject thunderPrefab;           //  Thunder Skill Prefabs
    GameObject atkBuffPrefab;           //  atkBuff Skill Prefabs    
    GameObject slashPrefab;            //  Slash1 Skill Prefabs    
    public int money;       // 플레이어 골드 보유량
    public bool isDead;     // 플레이어 사망 여부

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
        moveSpeed = dm.playerData.moveSpeed;        // 데이터매니저에서 값 받기
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
            // 대쉬 중이 아닐 때는 방향 전환 불가
            lastHorizontalInput = Mathf.Clamp(lastHorizontalInput, -1f, 1f);
        }*/

        if (horizontalInput != 0)
        {
            lastHorizontalInput = horizontalInput;
        }

        if (Input.GetKey(KeyCode.DownArrow))//웅크리기
        {
            animator.SetBool("isCrawl", true);
        }
        else
        {
            animator.SetBool("isCrawl", false);
        }


        if (isGround)
        {
            currentJumpCount = JumpCount; // 땅에 닿으면 점프 횟수 초기화
        }

        // 이동 상태 설정
        move = horizontalInput != 0;

        // 방향 전환 로직
        if (horizontalInput < 0)
        {
            sr.flipX = true; // 왼쪽을 바라보게 함
            cc.offset = new Vector2(Mathf.Abs(cc.offset.x), cc.offset.y);
        }
        else if (horizontalInput > 0)
        {
            sr.flipX = false; // 오른쪽을 바라보게 함
            cc.offset = new Vector2(-Mathf.Abs(cc.offset.x), cc.offset.y);
        }

        // 이동 상태에 따라 애니메이터 변수 설정
        animator.SetBool("isRun", move);

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking)//공격
        {
            StartCoroutine(ShowHitboxForDuration(0.5f));
        }

        if (!isAttacking) //움직이며 공격,스킬x
        {
            // Rigidbody의 속도 설정으로 이동 구현              
            Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }

        if (Input.GetKey(KeyCode.E) && !isAttacking)//스킬1
        {
            StartCoroutine(ShowEffect1ForDuration(1.0f));
        }



        if (Input.GetKey(KeyCode.D) && !isAttacking)//스킬2
        {
            StartCoroutine(ShowEffect2ForDuration(1.0f));
        }



        if (Input.GetKey(KeyCode.Alpha6) && !isAttacking)//스킬3
        {
            StartCoroutine(BoostSpeedForDurationskill(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isSkill3");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        if (Input.GetKey(KeyCode.Alpha8) && !isAttacking)//스킬4
        {
            StartCoroutine(ShowEffect4ForDuration(1.0f));
        }

        if (Input.GetKey(KeyCode.Alpha9) && !isAttacking)//스킬5
        {
            RecoverHealth();
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))//스킬6
        {
            isHealingActive = true;
            StartCoroutine(StopHealing());
        }

        IEnumerator StopHealing()
        {
            yield return new WaitForSeconds(5f);
            isHealingActive = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashSc.isFillingFirst) // 대쉬
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isDash");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            //canChangeDirDuringDash = false;
        }

        /*if (effect3.gameObject.activeSelf)
        {
            Debug.Log("effect3 오브젝트가 활성화되어 있습니다.");
        }
        else
        {
            Debug.Log("effect3 오브젝트가 비활성화되어 있습니다.");
        }*/


    }

    IEnumerator ShowHitboxForDuration(float duration)//평타
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
                animator.SetTrigger("isJump"); // 점프 애니메이션 트리거 실행
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
    public void Playerhurt(int damage)//피격
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        StartCoroutine(ChangeLayerToPlayer());
        dm.playerData.curHpValue -= damage;
        hpBar.ChangeHp((int)curHp);

        // 데미지 흡수 패시브 효과 적용
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
            float dashDistance = 5f; // 대쉬할 거리
            Vector2 dashDirection = new Vector2(direction, 0f).normalized; // 대쉬 방향
            Vector3 startPosition = transform.position; // 시작 위치
            Vector3 endPosition = startPosition + new Vector3(dashDirection.x, dashDirection.y, 0) * dashDistance; // 목표 위치

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                // 대쉬 중 충돌 체크
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, LayerMask.GetMask("Wall"));
                RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

                if (wallHit.collider != null || groundHit.collider != null)
                {
                    // 벽이나 바닥에 부딪히면 대쉬 중지
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
    IEnumerator ShowEffect1ForDuration(float duration)//피의칼날
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
    IEnumerator ShowEffect2ForDuration(float duration)//검기
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
    IEnumerator BoostSpeedForDurationskill(float duration, float direction)//대쉬어택
    {
        if (!isBoosted)
        {
            isBoosted = true;
            moveSpeed = boostedSpeed;

            // 대쉬 기능 추가
            float dashDistance = 5f; // 대쉬할 거리
            Vector2 dashDirection = new Vector2(direction, 0f).normalized; // 대쉬 방향
            Vector3 startPosition = transform.position; // 시작 위치
            Vector3 endPosition = startPosition + new Vector3(dashDirection.x, dashDirection.y, 0) * dashDistance; // 목표 위치

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                // 대쉬 중 충돌 체크
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, LayerMask.GetMask("Wall"));
                RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

                if (wallHit.collider != null || groundHit.collider != null)
                {
                    // 벽이나 바닥에 부딪히면 대쉬 중지
                    transform.position = wallHit.collider != null ? wallHit.point : groundHit.point;
                    break;
                }

                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;

            }

            // effect3 오브젝트 활성화
            effect3.gameObject.SetActive(true);
            effect3.transform.position = transform.position;

            float effect3Duration = 0.5f; // effect3의 지속시간을 1초로 설정
            float effect3ElapsedTime = 0;
            while (effect3ElapsedTime < effect3Duration)
            {
                effect3ElapsedTime += Time.deltaTime;
                effect3.transform.position = transform.position; // effect3 오브젝트 위치 업데이트
                yield return null;
            }

            // effect3 오브젝트 비활성화
            effect3.gameObject.SetActive(false);

            // 부스트 상태 초기화
            isBoosted = false;
            moveSpeed = originalSpeed;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    IEnumerator ShowEffect4ForDuration(float duration)//파이어볼트
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

    void RecoverHealth()//힐스킬
    {
        curHp = Mathf.Min(maxHp, curHp + 10f);
    }

    IEnumerator RegenerateHealth()//힐 패시브
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            curHp = Mathf.Min(maxHp, curHp + 5f);
        }
    }

    IEnumerator IncreaseMovementSpeed()//이속 패시브
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            moveSpeed += 2f;
        }
    }

    private IEnumerator DamageAbsorptionBuff()//데미지 흡수
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
    public IEnumerator HolyArrowSkill() // HolyArrow 스킬 생성 함수 
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

    public IEnumerator HolyPillarSkill()    // HolyPillar 스킬 생성 함수 
    {
        isAttacking = true;
        float direction = sr.flipX ? -3.5f : 3.5f;

        Vector3 spawnPosition = this.transform.position + new Vector3(direction, 1.5f, 0f);
        animator.SetTrigger("isPillar");
        yield return new WaitForSeconds(0.5f);
        Instantiate(holyPillarPrefab, spawnPosition, Quaternion.identity);
        isAttacking = false;
    }

    public IEnumerator ThunderSkill()       // Thunder 스킬 생성 함수 
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

    public IEnumerator AtkBuffSkill()      // AtkBuff 스킬 생성 함수 
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

    public IEnumerator SlashSkill()     // Slash 패시브 생성 함수 
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
            Debug.Log("획득");
            money += Random.Range(1, 11); // 코인과 충돌 시 돈이 1에서 10까지 랜덤하게 증가
            Destroy(collision.gameObject); // 코인 오브젝트 제거
        }
        else if (collision.gameObject.CompareTag("Potion"))
        {
            curHp = Mathf.Min(curHp + 20f, maxHp); // 포션과 충돌 시 체력 회복, 최대 체력 초과 방지
            Destroy(collision.gameObject); // 포션 오브젝트 제거
        }
        else if (collision.gameObject.CompareTag("SkillItem"))
        {
            int randomStat = Random.Range(0, 3);
            switch (randomStat)
            {
                case 0:
                    power += 5f; // 공격력 증가
                    break;
                case 1:
                    moveSpeed += 1f; // 이동 속도 증가
                    break;
                case 2:
                    groundCheckRadius += 1; // 점프 횟수 증가
                    break;
            }
            Destroy(collision.gameObject); // 스킬 아이템 오브젝트 제거
        }
    }// 5.22 이경규추가
}
