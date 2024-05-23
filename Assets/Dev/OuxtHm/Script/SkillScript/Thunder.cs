using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public SkillScriptable skillInfo;
    BoxCollider2D boxCollider2D;
    public int thunderDamage;

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
        yield return new WaitForSeconds(0.5f);
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(0.4f);
        boxCollider2D.enabled = false;
        Destroy(this.gameObject);
    }
}
