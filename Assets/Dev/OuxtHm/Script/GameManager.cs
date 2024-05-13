using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    /*[Header("연결된 스크립트")]
    public SoundOption sound;
    public SkillUI skillUi;
    public ChangeSkill changeSkill;*/

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
            Destroy(instance.gameObject);
        }
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");

        //StartCoroutine(FindPlayerUi());
        StartCoroutine(FindOptionUiOpbject());
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Title_Scene")
        {
            ShowOptionUI();
        }    
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

    private IEnumerator GetComponent()      // 인게임씬에서 옵션 UI에 속한 컴포넌트 얻는 코루틴
    {
        if(SceneManager.GetActiveScene().name != "MainScene")
        {
            btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
            soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
            optionUI.SetActive(false);
        }
        else
        {
            yield return null;
            StartCoroutine(GetComponent());
        }
    }

    public IEnumerator FindOptionUiOpbject()        // 옵션 UI 찾는 코루틴
    {
        while(optionUI == null)
        {
            optionUI = GameObject.Find("Option_ui");
            yield return null;
        }
        if(optionUI != null)
        {
            StartCoroutine(GetComponent());
        }
    }

    /*public IEnumerator FindPlayerUi()       // 플레이어 UI 찾는 코루틴
    {
        while(playerUi == null)
        {
            playerUi = GameObject.Find("Player_ui");
            yield return null;
        }
        if(playerUi != null)
        {
            StartCoroutine(FIndPlayerUIScript());
        }

    }
    public IEnumerator FIndPlayerUIScript()     // 인게임 씬에서 플레이어 UI에 속한 스크립트 찾는 코루틴
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            sound = playerUi.GetComponentInChildren<SoundOption>();
            skillUi = playerUi.GetComponentInChildren<SkillUI>();
            changeSkill = playerUi.GetComponentInChildren<ChangeSkill>();
        }
        else
        {
            yield return null;
            StartCoroutine(FIndPlayerUIScript());
        }
    }*/
}
