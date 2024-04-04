using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Button[] btn = new Button[4];
    GameObject faedPrefab;      // ���̵� �ƿ� ������
    private void Awake()
    {
        faedPrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>();
        }
        btn[0].onClick.AddListener(() => StartCoroutine(ClickGameStartBtn()));
        btn[1].onClick.AddListener(() => ClickContinueBtn());
        btn[2].onClick.AddListener(() => ClickOptionBtn());
        btn[3].onClick.AddListener(() => ClickGameOverBtn());
    }

    IEnumerator  ClickGameStartBtn()
    {
        GameObject fade = Instantiate(faedPrefab);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("Merge_2");
    }

    void ClickContinueBtn()
    {
        Debug.Log("�̾��ϱ�");
    }

    void ClickOptionBtn()
    {

    }

    void ClickGameOverBtn()
    {
        // ����Ƽ �����Ϳ��� ���� ���̶��
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // ����� ���ø����̼ǿ��� ���� ���̶��
            Application.Quit();
        #endif

    }
}
