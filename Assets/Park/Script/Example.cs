using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    public Transform target;
    Player player;

    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float enemy_Speed = 1f;  //���� �̵��ӵ�
    public float detectionRange = 5f;  //������ Ÿ�� �ν� ����

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        StartCoroutine(NextMove());
    }

    private void Update()
    {
        TargetSenser();
    }

    void TargetSenser()  // �÷��̾� ����
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        target = Player.instance.gameObject.transform;

        float distanceToTarget = Vector3.Distance(this.transform.position, target.position); // ���Ϳ� Ÿ�� ������ �Ÿ� ���

        if (distanceToTarget <= detectionRange) // Ÿ���� ���� �ȿ� ���� �� ����
        {
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = transform.position.y;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * enemy_Speed);
            Debug.Log("Ÿ���� �����߽��ϴ�!");
        }
        else // Ÿ���� ���� �ۿ� ���� �� ����
        {
            Move();
            Debug.Log("Ÿ���� ���� ���� �ۿ� �ֽ��ϴ�.");
        }
    }

    public void Move()
    {
        transform.Translate(new Vector2(DirX, transform.position.y).normalized * Time.deltaTime * enemy_Speed);

        if (DirX == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (DirX == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator NextMove()  // ���Ͱ� ���� ������ �̵� ����
    {
        DirX = Random.Range(-1, 2);
        Debug.Log(DirX);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    private void OnDrawGizmos()
    {
        // ���� ������ �ð������� ��Ÿ���� ���� �ڵ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
