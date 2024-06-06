using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDeadPrefabs : MonoBehaviour
{
    DataManager dm;
    Button mainBtn;     // ���� �̵� ��ư
    GameObject fadePrefab;      // ���̵� �ƿ� ������
    private void Awake()
    {
        fadePrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        mainBtn = GetComponentInChildren<Button>();
        
    }
    private void Start()
    {
        dm = DataManager.instance;
        mainBtn.onClick.AddListener(() => StartCoroutine(MoveMainScene()));
    }
    public IEnumerator MoveMainScene()
    {
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        dm.DeleteFile();
        SceneManager.LoadScene("MainScene");
    }
}
