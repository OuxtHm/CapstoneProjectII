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
    public GameObject optionUI;     // 옵션 창
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;

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
        SceneManager.sceneLoaded += FindOptionUiOpbject;        // 씬을 불러 올 때마다 실행이 되도록 함수 추가
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Title_Scene")
        {
            ShowOptionUI();
        }    
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FindOptionUiOpbject;       
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


    public void FindOptionUiOpbject(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainScene")
        {
            optionUI = GameObject.Find("Option_ui");
            GetComponent();
        }
    }

    private void GetComponent()      // 인게임씬에서 옵션 UI에 속한 컴포넌트 얻는 코루틴
    {
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
        optionUI.SetActive(false);
    }
}
