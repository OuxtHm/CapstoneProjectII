using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("오브젝트")]
    public GameObject playerUi;        // 플레이어 관련 UI
    public GameObject optionUI;        // 옵션 창
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;    // 플레이어 사망 UI
    public GameObject fadeInPrefab;    // Fade In  UI

    [Header("변수")]
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
        SceneManager.sceneLoaded += FindOptionUiObject;  // 씬을 불러 올 때마다 실행이 되도록 함수 추가
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

    private void RetrieveOptionUIComponents()  // 인게임씬에서 옵션 UI에 속한 컴포넌트 얻는 함수
    {
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
        optionUI.SetActive(false);
    }

    IEnumerator CreateFadeIn()
    {
        // FadeIn 객체가 이미 존재하는지 확인
        if (GameObject.Find("FadeInCanvas") == null)
        {
            GameObject fadeIn = Instantiate(fadeInPrefab);
            fadeIn.name = "FadeInCanvas"; // 생성된 객체에 고유 이름 부여

            yield return new WaitForSeconds(1f);

            Destroy(fadeIn);
        }
    }
}
