using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{   
        public int damage = 10; // ���� ������

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                // ������ ������ ������
                //collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    
}
