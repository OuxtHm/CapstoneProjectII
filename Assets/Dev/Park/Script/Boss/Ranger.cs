using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : MonoBehaviour
{
    int DirX;
    int enemy_Power = 2;
    Animator anim;
    BoxCollider2D BoxColliderSize;
    Transform AttackBox;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            anim.SetBool("Move", false);
            Atk();
        }
            
    }

    void Atk()    //몬스터 공격 함수
    {
        BoxColliderSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxColliderSize.size, 0);

        anim.SetTrigger("Attack");

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(enemy_Power);
            }
        }
    }
}
