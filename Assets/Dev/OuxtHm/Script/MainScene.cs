using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public static MainScene instance;
    public DataManager dm;
    SoundManager sm;
    public Button[] btn = new Button[4];
    GameObject fadePrefab;      // 페이드 아웃 프리펩
    GameObject optionUi;        // 옵션 UI
    string savePath;        // 데이터 저장 경로
    public AudioClip clickSounds;      // 버튼 클릭 사운드
    private void Awake()
    {
        instance = this;
        fadePrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        optionUi = transform.GetChild(1).gameObject;
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>();
        }
        
    }
    private void Start()
    {
        dm = DataManager.instance;
        sm = SoundManager.instance;
        savePath = dm.playerDataPath;
        btn[0].onClick.AddListener(() => StartCoroutine(ClickNewGameBtn()));
        btn[1].onClick.AddListener(() => StartCoroutine(ClickGameStartBtn()));
        btn[2].onClick.AddListener(() => ClickOptionBtn());
        btn[3].onClick.AddListener(() => ClickGameOverBtn());
        FileExistence(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    IEnumerator ClickNewGameBtn()
    {
        sm.SFXPlay(clickSounds);
        dm.NewGame();
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        Destroy(fade);     //--------------삭제 할 때 로드가 느려서 화면 이상하게 전환됨
        SceneManager.LoadScene("Merge_7");

    }
    IEnumerator ClickGameStartBtn()
    {
        sm.SFXPlay(clickSounds);
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        Destroy(fade);     //--------------삭제 할 때 로드가 느려서 화면 이상하게 전환됨
        SceneManager.LoadScene("Merge_7");
    }

    void ClickOptionBtn()
    {
        sm.SFXPlay(clickSounds);
        optionUi.SetActive(true);
    }

    void ClickGameOverBtn()
    {
        sm.SFXPlay(clickSounds);

        // 유니티 에디터에서 실행 중이라면
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 빌드된 애플리케이션에서 실행 중이라면
            Application.Quit();
        #endif

    }

    public void FileExistence(Scene scene, LoadSceneMode mode)        // 이어하기 버튼 유무 결정
    {
        
        if (scene.name == "MainScene")
        {
            if (btn[1] != null)
            {
                bool active = File.Exists(savePath) ? true : false;
                btn[1].gameObject.SetActive(active);
            }
            else
            {
                Debug.LogError("Continue 버튼이 null입니다.");
            }
        }
    }

}
