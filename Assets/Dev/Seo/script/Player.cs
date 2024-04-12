using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float maxHp = 100f;
    public float curHp = 100f;
    public bool isBoosted = false;
    public float boostDuration = 0.1f;
    public float boostedSpeed = 20f; // 증가된 이동 속도 값
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
    private SpriteRenderer sr;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public int maxJumpCount = 1; 
    private int currentJumpCount;
    public LayerMask whatIsGround;
    public bool isGround;
    GameObject hitboxClone;

    public int money;

    public void Awake()
    {
        instance = this;
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

    private void OnCollisionEnter2D(Collision2D collision) //피격
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
            currentJumpCount = maxJumpCount; // 땅에 닿으면 점프 횟수 초기화
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentJumpCount > 0)
            {
                animator.SetTrigger("isJump");
                rb.AddForce(new Vector2(0, jumpForce));
                currentJumpCount--; // 점프하면 점프 횟수 감소
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking)
        {
            StartAttackAnimation();
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

        // Rigidbody의 속도 설정으로 이동 구현              
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // 이동 상태에 따라 애니메이터 변수 설정
        animator.SetBool("isRun", move);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput)); // 여기에 lastHorizontalInput을 전달
            animator.SetTrigger("isDash");
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
    }
    public IEnumerator Playerhurt(int damage)
    {
        animator.SetTrigger("isHit");
        curHp -= damage;
        Debug.Log("입은 피해:" + damage);

        if (curHp <= 0)
        {
            animator.SetTrigger("isDie");
        }
        yield return new WaitForSeconds(0);
    }

}
