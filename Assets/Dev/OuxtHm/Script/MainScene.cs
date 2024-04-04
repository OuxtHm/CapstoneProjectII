using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Button[] btn = new Button[4];
    GameObject faedPrefab;      // 페이드 아웃 프리펩
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
        Debug.Log("이어하기");
    }

    void ClickOptionBtn()
    {

    }

    void ClickGameOverBtn()
    {
        // 유니티 에디터에서 실행 중이라면
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 빌드된 애플리케이션에서 실행 중이라면
            Application.Quit();
        #endif

    }
}
