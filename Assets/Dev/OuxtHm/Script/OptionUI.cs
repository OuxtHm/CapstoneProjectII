using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    GameManager gm;
    SoundManager sm;

    public AudioClip clickSounds;      // ��ư Ŭ�� ����

    private Button[] btn = new Button[4];
    private GameObject btnArray;     
    private GameObject soundsAray;   
    private Button soundsBackBtn;
    public GameObject questionObj;      // Ȯ���ϴ� ���� ������Ʈ
    private Button yesBtn;       // ���� Ȯ�� ��ư
    private Button noBtn;
    private TextMeshProUGUI questionTxt;
    private string mainSceneTxt;    // ����ȭ������ ���� ��ư�� ������ �� ��� �ؽ�Ʈ
    private string gameOverTxt;     // ���� ���� ��ư�� ������ �� ��� �ؽ�Ʈ
    private void Awake()
    {
        mainSceneTxt = "����ȭ������ �̵��Ͻðڽ��ϱ�?";
        gameOverTxt = "���� �����Ͻðڽ��ϱ�?\r\n(������� ���� ������ �������ϴ�.)";
        btnArray = transform.GetChild(0).GetChild(1).gameObject;
        soundsAray = transform.GetChild(0).GetChild(2).gameObject;
        questionObj = transform.GetChild(1).gameObject;
        yesBtn = questionObj.transform.GetChild(1).GetComponent<Button>();
        noBtn = questionObj.transform.GetChild(2).GetComponent<Button>();
        questionTxt = questionObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        for(int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Button>();
        }
        SoundsButton();
        MainSceneBtn();
        GameOverBtn();
        YesBtn();
        NoBtn();
    }

    private void Start()
    {
        gm = GameManager.instance;
        sm = SoundManager.instance;

        btn[0].onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            gm.show = false;
            transform.gameObject.SetActive(false);
        });

    }

    void SoundsButton()     // �Ҹ� ���� ��ư�Լ�
    {
        btn[1].onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            btnArray.SetActive(false);
            soundsAray.SetActive(true);
        });

        soundsBackBtn = transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>();
        soundsBackBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            btnArray.SetActive(true);
            soundsAray.SetActive(false);
        });
    }
    void MainSceneBtn()
    {
        btn[2].onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            questionTxt.text = mainSceneTxt;
            questionObj.SetActive(true);
        });
    }
    void GameOverBtn()      // ���� ���� ��ư �Լ�
    {
        btn[3].onClick.AddListener(() => 
        {
            sm.SFXPlay(clickSounds);
            questionTxt.text = gameOverTxt;
            questionObj.SetActive(true);
        });
    }
    void YesBtn()       // Ȯ�� ��ư �Լ�
    {
        yesBtn.onClick.AddListener(() => {
            sm.SFXPlay(clickSounds);
            QuitGame();
        });
    }
    void NoBtn()        // �ƴϿ� ��ư �Լ�
    {
        noBtn.onClick.AddListener(() => {
            sm.SFXPlay(clickSounds);
            questionObj.SetActive(false);
        }); 
    }
    public void QuitGame()  // ���� ���� �Լ�
    {
        if (questionTxt.text == gameOverTxt)
        {
            // ����Ƽ �����Ϳ��� ���� ���̶��
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
            // ����� ���ø����̼ǿ��� ���� ���̶��
                Application.Quit();
            #endif
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }    
}
