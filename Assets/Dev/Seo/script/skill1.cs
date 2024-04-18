using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill2 : MonoBehaviour
{
    public float damage = 30;
    Enemy enemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = collider.GetComponent<Enemy>();
            collider.GetComponent<Enemy>().StartCoroutine(enemy.Hurt(this.transform, damage));
        }
    }
}
