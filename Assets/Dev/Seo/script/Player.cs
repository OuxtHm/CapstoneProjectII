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
    public float power;        // 플레이어 공격력      // 2024-04-14 유재현 추가
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
    public Transform groundCheck;//점프
    public float groundCheckRadius = 0.2f;//점프
    private int currentJumpCount;//점프
    public LayerMask whatIsGround;//점프
    //점프
    GameObject hitboxClone;//공격
    //public float knockbackStrength = 500f;//피격
    GameObject holyArrowPrefab;         // 2024-04-13 유재현 추가 HolyArrow Skill Prefabs
    GameObject holyPillarPrefab;        // 2024-04-13 유재현 추가 HolyPillar Skill Prefabs 
    GameObject thunderPrefab;           // 2024-04-13 유재현 추가 Thunder Skill Prefabs
    GameObject atkBuffPrefab;           // 2024-04-14 유재현 추가 atkBuff Skill Prefabs    
    GameObject slashPrefab;            // 2024-04-14 유재현 추가 Slash1 Skill Prefabs    
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

        if (Input.GetKey(KeyCode.E) && !isAttacking)//스킬1
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

        if (Input.GetKey(KeyCode.D) && !isAttacking)//스킬2
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

        if (Input.GetKey(KeyCode.Alpha6) && !isAttacking)//스킬3
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isSkill3");
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))//스킬4
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
                animator.SetTrigger("isJump"); // 점프 애니메이션 트리거 실행
            }        
        }

        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }
    public void Playerhurt(int damage)//피격
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        hpBar.ChangeHp((int)curHp);
        isDead = true;
        if (curHp <= 0)
        {
            animator.SetTrigger("isDie");
            StartCoroutine(gm.ShowDeadUI());        // 2024-04-13 유재현 추가 *******************************************
        }
    }
    IEnumerator BoostSpeedForDuration(float duration, float direction) // 새로 추가된 메소드
    {
        if (!isBoosted)
        {
            isBoosted = true;
            moveSpeed = boostedSpeed; // 이동 속도를 증가시킴

            StartDash();

            Vector2 dashDirection = new Vector2(direction, 0f); // 대쉬 방향 설정
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
    public IEnumerator HolyArrowSkill() // HolyArrow 스킬 생성 함수 2024-04-13 유재현 추가
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

    public IEnumerator HolyPillarSkill()    // HolyPillar 스킬 생성 함수 2024-04-13 유재현 추가
    {
        float direction = sr.flipX ? -3.5f : 3.5f;

        Vector3 spawnPosition = this.transform.position + new Vector3(direction, 1.5f, 0f);
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.5f);
        Instantiate(holyPillarPrefab, spawnPosition, Quaternion.identity);
    }

    public IEnumerator ThunderSkill()       // Thunder 스킬 생성 함수 2024-04-13 유재현 추가
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
    
    public IEnumerator AtkBuffSkill()      // AtkBuff 스킬 생성 함수 2024-04-14 유재현 추가
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

    public IEnumerator SlashSkill()     // Slash 패시브 생성 함수 2024-04-14 유재현 추가
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
