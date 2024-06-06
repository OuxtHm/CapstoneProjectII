using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SoundManager sm;
    [Header("������Ʈ")]
    public GameObject playerUi;        // �÷��̾� ���� UI
    public GameObject optionUI;        // �ɼ� â
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;    // �÷��̾� ��� UI
    public GameObject fadeInPrefab;    // Fade In  UI
    public Slider[] slider = new Slider[3];            // ���� ���� �����̴�

    [Header("����")]
    public bool show;

    [Header("����")]
    public AudioClip mainBgm;       // ���� �����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");
        fadeInPrefab = Resources.Load<GameObject>("Prefabs/FadeIn_canvas");
    }

    private void Start()
    {
        sm = SoundManager.instance;
        SceneManager.sceneLoaded += FindOptionUiObject;  // ���� �ҷ� �� ������ ������ �ǵ��� �Լ� �߰�
        SceneManager.sceneLoaded += MainBgmPlay;

        MainBgmPlay(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainScene")
        {
            ShowOptionUI();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FindOptionUiObject;
        SceneManager.sceneLoaded -= MainBgmPlay;
    }

    public void ShowOptionUI()
    {
        if (show)
        {
            show = false;
            optionUI.SetActive(false);
        }
        else
        {
            show = true;
            optionUI.SetActive(true);
            btnArray.SetActive(true);
            soundOption.SetActive(false);
        }
    }

    public IEnumerator ShowDeadUI()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(deadUiPrefab);
    }

    public void FindOptionUiObject(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(CreateFadeIn());
        if (scene.name != "MainScene")
        {
            
            optionUI = GameObject.Find("Option_ui");
            RetrieveOptionUIComponents();
        }
    }
    public void MainBgmPlay(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(sm.FadeInCoroutine());
        if (scene.name == "MainScene")
        {
            sm.BGMPlay(mainBgm);
        }
    }
    private void RetrieveOptionUIComponents()  // �ΰ��Ӿ����� �ɼ� UI�� ���� ������Ʈ ��� �Լ�
    {
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
        for(int i = 0; i < slider.Length; i++)
        {
            slider[i] = soundOption.transform.GetChild(1).GetChild(i).GetComponent<Slider>();
        }
        slider[0].onValueChanged.AddListener(delegate { sm.MasterVolume(slider[0].value); });
        slider[1].onValueChanged.AddListener(delegate { sm.MasterVolume(slider[1].value); });
        slider[2].onValueChanged.AddListener(delegate { sm.MasterVolume(slider[2].value); });
        soundOption.SetActive(false);
        optionUI.SetActive(false);
    }

    IEnumerator CreateFadeIn()
    {
        // FadeIn ��ü�� �̹� �����ϴ��� Ȯ��
        if (GameObject.Find("FadeInCanvas") == null)
        {
            GameObject fadeIn = Instantiate(fadeInPrefab);
            fadeIn.name = "FadeInCanvas"; // ������ ��ü�� ���� �̸� �ο�

            yield return new WaitForSeconds(1f);

            Destroy(fadeIn);
        }
    }
}
