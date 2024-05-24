using System.Collections;
using UnityEngine;

public class GoldWarning : MonoBehaviour
{
    float destroySec;       // 파괴까지 걸리는 시간
    private void Awake()
    {
        destroySec = 1.4f;
    }
    void Start()
    {
        StartCoroutine(GoDestroy());
    }

    private IEnumerator GoDestroy()
    {
        yield return new WaitForSeconds(destroySec);
        Destroy(this.gameObject);
    }

}
