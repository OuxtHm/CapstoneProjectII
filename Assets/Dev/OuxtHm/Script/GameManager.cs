using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject optionUI;     // ¿É¼Ç Ã¢
    public bool show;
    private void Awake()
    {
        instance = this;
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
        }
    }
}
