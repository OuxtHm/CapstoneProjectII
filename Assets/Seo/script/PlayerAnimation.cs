using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool move;
    private Rigidbody2D rb;
    private Animator  animator;
    public float jumpForce = 250f;
    public float moveSpeed = 2f;
    private SpriteRenderer sr;
    //private bool isGrounded;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //CheckGrounded();

        if (Input.GetKeyDown(KeyCode.DownArrow))       
        {         
            animator.SetBool("isCrawl", true);
        }
        else
        {           
            animator.SetBool("isCrawl", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetTrigger("isJump");
            rb.AddForce(new Vector2(0, jumpForce));
            //isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.A)) 
        {
            animator.SetTrigger("isAttack");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move = true;

            if (horizontalInput < 0)
            {

            }
        }
        // ���� ȭ��ǥ�� ������ �������� �̵�
        else 
        {
            move = false; 
            if (horizontalInput < 0) 
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }

        // Rigidbody�� �ӵ� �������� �̵� ����              
        Vector2 movement = new Vector2(horizontalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

        // �̵� ���¿� ���� �ִϸ����� ���� ����
        animator.SetBool("isRun", move);
    }
    /*private void CheckGrounded()
    {
        
    }*/
}
