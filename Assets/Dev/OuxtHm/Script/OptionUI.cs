using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    GameManager gm;

    private Button[] btn = new Button[4];
    private GameObject btnArray;     
    private GameObject soundsAray;   
    private Button soundsBackBtn;
    public GameObject questionObj;      // 확인하는 문구 오브젝트
    private Button yesBtn;       // 종료 확인 버튼
    private Button noBtn;
    private TextMeshProUGUI questionTxt;
    private string mainSceneTxt;    // 메인화면으로 가기 버튼을 눌렀을 때 띄울 텍스트
    private string gameOverTxt;     // 게임 종료 버튼을 눌렀을 떄 띄울 텍스트
    private void Awake()
    {
        mainSceneTxt = "메인화면으로 이동하시겠습니까?";
        gameOverTxt = "정말 종료하시겠습니까?\r\n(저장되지 않은 구간은 지워집니다.)";
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
        btn[0].onClick.AddListener(() =>
        {
            gm.show = false;
            transform.gameObject.SetActive(false);
        });

    }

    void SoundsButton()     // 소리 설정 버튼함수
    {
        btn[1].onClick.AddListener(() =>
        {
            btnArray.SetActive(false);
            soundsAray.SetActive(true);
        });

        soundsBackBtn = transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>();
        soundsBackBtn.onClick.AddListener(() =>
        {
            btnArray.SetActive(true);
            soundsAray.SetActive(false);
        });
    }
    void MainSceneBtn()
    {
        btn[2].onClick.AddListener(() =>
        {
            questionTxt.text = mainSceneTxt;
            questionObj.SetActive(true);
        });
    }
    void GameOverBtn()      // 게임 종료 버튼 함수
    {
        btn[3].onClick.AddListener(() => 
        {
            questionTxt.text = gameOverTxt;
            questionObj.SetActive(true);
        });
    }
    void YesBtn()       // 확인 버튼 함수
    {
        yesBtn.onClick.AddListener(() =>  QuitGame());
    }
    void NoBtn()        // 아니오 버튼 함수
    {
        noBtn.onClick.AddListener(() => questionObj.SetActive(false)); 
    }
    public void QuitGame()  // 게임 종료 함수
    {
        // 유니티 에디터에서 실행 중이라면
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 애플리케이션에서 실행 중이라면
            Application.Quit();
        #endif
    }
}
