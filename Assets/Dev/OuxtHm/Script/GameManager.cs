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
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded += FindOptionUiOpbject;        // ���� �ҷ� �� ������ ������ �ǵ��� �Լ� �߰�
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

    private void GetComponent()      // �ΰ��Ӿ����� �ɼ� UI�� ���� ������Ʈ ��� �ڷ�ƾ
    {
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
        optionUI.SetActive(false);
    }
}
