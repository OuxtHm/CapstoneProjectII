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
    public float boostedSpeed = 40f; // ������ �̵� �ӵ� ��
    public float originalSpeed; // ���� �̵� �ӵ��� ������ ����
    public bool isAttacking = false;
    public bool move;
    private Rigidbody2D rb;
    private Animator  animator;
    public float jumpForce = 550f;
    public float moveSpeed = 4f;
    private SpriteRenderer sr;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    public bool isGround;


    public int money;

    public void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        maxHp = 100f;
        curHp = 100f;
        money = 9999;

    }
    public void Start()
    {
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    public void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isCrawl", true);
        }
        else
        {
            animator.SetBool("isCrawl", false);
        }

        if (isGround && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            animator.SetTrigger("isJump");
            rb.AddForce(new Vector2(0, jumpForce));
            // isGround = false; //
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("isAttack");
        }

        // �̵� ���� ����
        move = horizontalInput != 0;

        // ���� ��ȯ ����
        if (horizontalInput < 0)
        {
            sr.flipX = true; // ������ �ٶ󺸰� ��
        }
        else if (horizontalInput > 0)
        {
            sr.flipX = false; // �������� �ٶ󺸰� ��
        }   

        // Rigidbody�� �ӵ� �������� �̵� ����              
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // �̵� ���¿� ���� �ִϸ����� ���� ����
        animator.SetBool("isRun", move);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(BoostSpeedForDuration(boostDuration));
            animator.SetTrigger("isDash");
        }

        IEnumerator BoostSpeedForDuration(float duration) // ���� �߰��� �޼ҵ�
        {
            if (!isBoosted)
            {
                isBoosted = true;
                moveSpeed = boostedSpeed; // �̵� �ӵ��� ������Ŵ

                yield return new WaitForSeconds(duration); // ���� �ð� ���

                moveSpeed = originalSpeed; // �̵� �ӵ��� ������� ����
                isBoosted = false;
            }
        }
        /*
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Enemy")
            { 
                curHp -= 5; 
                if (curHp <= 0)
                {
                    animator.SetTrigger("isDie");
                }
                else if(curHp > 0)
                {
                    animator.SetTrigger("isHit");
                }
            }
        }
        */
    }
}
