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
    public float boostedSpeed = 10f;
    public float originalSpeed;
    public float spawnDistance = 1f;
    public GameObject hitboxPrefab;
    public GameObject effect1;
    public GameObject effect2;
    public bool isAttacking = false;
    public bool move;
    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private Animator animator;
    public float moveSpeed = 5f;
    private float lastHorizontalInput = 1;
    private SpriteRenderer sr;
    public Transform groundCheck;//����
    public float groundCheckRadius = 0.2f;//����
    public int maxJumpCount = 1; //����
    private int currentJumpCount;//����
    public LayerMask whatIsGround;//����
    public bool isGround;//����
    GameObject hitboxClone;//����
    //public float knockbackStrength = 500f;//�ǰ�

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

        float horizontalInput = Input.GetAxis("Horizontal");

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


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentJumpCount > 0)
            {
                StartCoroutine(JumpToHeight(0.2f, 3f)); // ��, ����
                currentJumpCount--;
            }
        }

        if (isGround)
        {
            currentJumpCount = maxJumpCount; // ���� ������ ���� Ƚ�� �ʱ�ȭ
        }

        IEnumerator JumpToHeight(float duration, float height)
        {
            animator.SetTrigger("isJump");
            Vector3 startPosition = transform.position;
            Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                // �¿� �̵� �Է� ����
                float moveInput = Input.GetAxis("Horizontal");
                transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);

                // ���� ���� ���
                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
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

        if (Input.GetKeyDown(KeyCode.LeftShift))//�뽬
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration, lastHorizontalInput));
            animator.SetTrigger("isDash");
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

                isBoosted = false;
            }
        }
    }

    public void Playerhurt(int damage)//�ǰ�
    {
        animator.SetTrigger("isHit");
        curHp -= damage;      

        if (curHp <= 0)
        {
            animator.SetTrigger("isDie");
        }
    }

}
