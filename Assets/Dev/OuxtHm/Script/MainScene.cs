using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Button[] btn = new Button[4];

    private void Awake()
    {
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>();
        }
        btn[0].onClick.AddListener(() => ClickGameStartBtn());
        btn[1].onClick.AddListener(() => ClickContinueBtn());
        btn[2].onClick.AddListener(() => ClickOptionBtn());
        btn[3].onClick.AddListener(() => ClickGameOverBtn());
    }

    void ClickGameStartBtn()
    {
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
