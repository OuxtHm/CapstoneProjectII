using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("������Ʈ")]
    public GameObject playerUi;        // �÷��̾� ���� UI
    public GameObject optionUI;        // �ɼ� â
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;    // �÷��̾� ��� UI
    public GameObject fadeInPrefab;    // Fade In  UI

    [Header("����")]
    public bool show;

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
        SceneManager.sceneLoaded += FindOptionUiObject;  // ���� �ҷ� �� ������ ������ �ǵ��� �Լ� �߰�
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");
        fadeInPrefab = Resources.Load<GameObject>("Prefabs/FadeIn_canvas");
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

    private void RetrieveOptionUIComponents()  // �ΰ��Ӿ����� �ɼ� UI�� ���� ������Ʈ ��� �Լ�
    {
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
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
