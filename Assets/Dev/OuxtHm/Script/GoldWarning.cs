using System.Collections;
using UnityEngine;

public class GoldWarning : MonoBehaviour
{
    float destroySec;       // �ı����� �ɸ��� �ð�
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
