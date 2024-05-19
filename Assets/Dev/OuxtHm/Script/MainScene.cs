using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public static MainScene instance;
    public DataManager dm;
    public Button[] btn = new Button[4];
    GameObject fadePrefab;      // ���̵� �ƿ� ������
    GameObject optionUi;        // �ɼ� UI
    string savePath;        // ������ ���� ���
    private void Awake()
    {
        instance = this;
        fadePrefab = Resources.Load<GameObject>("Prefabs/FadeOut_canvas");
        optionUi = transform.GetChild(1).gameObject;
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>();
        }
        btn[0].onClick.AddListener(() => StartCoroutine(ClickNewGameBtn()));
        btn[1].onClick.AddListener(() => ClickContinueBtn());
        btn[2].onClick.AddListener(() => ClickOptionBtn());
        btn[3].onClick.AddListener(() => ClickGameOverBtn());
    }
    private void Start()
    {
        dm = DataManager.instance;
        savePath = dm.playerDataPath;
        FileExistence(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    IEnumerator ClickNewGameBtn()
    {
        GameObject fade = Instantiate(fadePrefab);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Merge_7");
    }

    void ClickContinueBtn()
    {
        Debug.Log("�̾��ϱ�");
    }

    void ClickOptionBtn()
    {
        optionUi.SetActive(true);
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
