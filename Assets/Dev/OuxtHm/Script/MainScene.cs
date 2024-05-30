using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public static MainScene instance;
    DataManager dm;
    SoundManager sm;
    public Button[] btn = new Button[4];
    GameObject fadePrefab;      // ���̵� �ƿ� ������
    GameObject optionUi;        // �ɼ� UI
    string savePath;        // ������ ���� ���
    public AudioClip clickSounds;      // ��ư Ŭ�� ����
    public AudioClip mainBgm;       // ���� �����
    public string sceneName;        // �̵��� �� �̸�
    private void Awake()
    {
        instance = this;
        sceneName = "Merge_8";
        fadePrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        optionUi = transform.GetChild(1).gameObject;
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Button>();
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
        StartCoroutine(sm.FadeOutCoroutine());
        dm.NewGame();
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        sm.BGMStop(mainBgm);
        SceneManager.LoadScene(sceneName);

    }
    IEnumerator ClickGameStartBtn()
    {
        sm.SFXPlay(clickSounds);
        StartCoroutine(sm.FadeOutCoroutine());
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        sm.BGMStop(mainBgm);
        SceneManager.LoadScene(sceneName);
    }

    void ClickOptionBtn()
    {
        sm.SFXPlay(clickSounds);
        optionUi.SetActive(true);
    }

    void ClickGameOverBtn()
    {
        sm.SFXPlay(clickSounds);

        // ����Ƽ �����Ϳ��� ���� ���̶��
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // ����� ���ø����̼ǿ��� ���� ���̶��
            Application.Quit();
        #endif

    }

    public void FileExistence(Scene scene, LoadSceneMode mode)        // �̾��ϱ� ��ư ���� ����
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
                Debug.LogError("Continue ��ư�� null�Դϴ�.");
            }
        }
    }
}
