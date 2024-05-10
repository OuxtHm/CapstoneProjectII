using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("����� ��ũ��Ʈ")]
    public SoundOption sound;
    public SkillUI skillUi;
    public ChangeSkill changeSkill;

    [Header("������Ʈ")]
    public GameObject optionUI;     // �ɼ� â
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;

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
            Destroy(instance.gameObject);
        }
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");
       
        StartCoroutine(FindOpbject());
        
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

    private IEnumerator GetComponent()      // �ΰ��Ӿ����� �ɼ� UI�� ���� ������Ʈ ��� �ڷ�ƾ
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

    public IEnumerator FindOpbject()        // �ɼ� UI ã�� �ڷ�ƾ
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
}
