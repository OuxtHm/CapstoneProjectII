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

        StartCoroutine(GetComponent());
        
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
        GameObject deadUi = Instantiate(deadUiPrefab);
    }

    private IEnumerator GetComponent()
    {
        if(SceneManager.GetActiveScene().name != "MainScene")
        {
            btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
            soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
        }
        else
        {
            yield return null;
            StartCoroutine(GetComponent());
        }
    }

}
