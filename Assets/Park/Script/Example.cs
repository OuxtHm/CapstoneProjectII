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

    int DirX;   //몬스터가 바라보는 방향값
    public float enemy_Speed = 1f;  //몬스터 이동속도
    public float detectionRange = 5f;  //몬스터의 타겟 인식 범위

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

    void TargetSenser()  // 플레이어 추적
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        target = Player.instance.gameObject.transform;

        float distanceToTarget = Vector3.Distance(this.transform.position, target.position); // 몬스터와 타겟 사이의 거리 계산

        if (distanceToTarget <= detectionRange) // 타겟이 범위 안에 있을 때 수행
        {
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = transform.position.y;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * enemy_Speed);
            Debug.Log("타겟을 감지했습니다!");
        }
        else // 타겟이 범위 밖에 있을 때 수행
        {
            Move();
            Debug.Log("타겟이 감지 범위 밖에 있습니다.");
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

    IEnumerator NextMove()  // 몬스터가 다음 실행할 이동 방향
    {
        DirX = Random.Range(-1, 2);
        Debug.Log(DirX);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 시각적으로 나타내기 위한 코드
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
