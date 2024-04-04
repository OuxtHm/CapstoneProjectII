using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{   
        public int damage = 10; // 입힐 데미지

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                // 적에게 데미지 입히기
                //collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    
}
