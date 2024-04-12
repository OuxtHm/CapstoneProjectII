using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyPillar : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    public int PillarDamage = 20;

    private void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        StartCoroutine(ChangeBoxOffsetSize());
    }

    public IEnumerator ChangeBoxOffsetSize()
    {
        yield return new WaitForSeconds(0.1f);
        boxCollider2D.enabled = true;
        boxCollider2D.offset = new Vector2(0, -1.77f);
        boxCollider2D.size = new Vector2(6, 3);
        yield return new WaitForSeconds(0.1f);
        boxCollider2D.offset = new Vector2(0, -0.5f);
        boxCollider2D.size = new Vector2(3, 5);
        yield return new WaitForSeconds(0.1f);
        boxCollider2D.offset = new Vector2(0, -0.2f);
        boxCollider2D.size = new Vector2(1.5f, 5.5f);
        boxCollider2D.enabled = false;

        yield return new WaitForSeconds(1.3f);
        Destroy(this.gameObject);
    }
}
