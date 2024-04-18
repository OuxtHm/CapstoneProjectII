using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slash2 : MonoBehaviour
{
    BoxCollider2D box;

    private void Awake()
    {
        box = this.gameObject.GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        StartCoroutine(ChangeBoxSize());
    }
    public IEnumerator ChangeBoxSize()
    {
        yield return new WaitForSeconds(0.1f);
        box.offset = new Vector2(0.7f, 0.65f);
        box.size = new Vector2(3.75f, 2f);
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }
}
