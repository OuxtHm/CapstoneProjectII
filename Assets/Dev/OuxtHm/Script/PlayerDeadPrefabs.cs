using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDeadPrefabs : MonoBehaviour
{
    Button mainBtn;     // ���� �̵� ��ư
    GameObject fadePrefab;      // ���̵� �ƿ� ������
    private void Awake()
    {
        fadePrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        mainBtn = GetComponentInChildren<Button>();
        mainBtn.onClick.AddListener(() => StartCoroutine(MoveMainScene()));
    }

    public IEnumerator MoveMainScene()
    {
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
    }
}
