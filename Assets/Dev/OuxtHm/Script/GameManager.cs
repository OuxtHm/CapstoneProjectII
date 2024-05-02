using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SkillUI skillUi;
    public ChangeSkill changeSkill;
    public GameObject optionUI;     // ¿É¼Ç Ã¢
    public GameObject btnArray;
    public GameObject soundOption;
    public GameObject deadUiPrefab;
    public bool show;
    private void Awake()
    {
        instance = this;
        deadUiPrefab = Resources.Load<GameObject>("Prefabs/PlayerDead_canvas");
        btnArray = optionUI.transform.GetChild(0).GetChild(1).gameObject;
        soundOption = optionUI.transform.GetChild(0).GetChild(2).gameObject;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
}
