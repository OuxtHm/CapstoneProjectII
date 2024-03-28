using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public GameObject shopUiPrefabs;
    public GameObject keyX;
    public bool uiOpen;     // ���� UI�� ���ȴ��� Ȯ��
    public bool inShop;     // ���� ������ �����ߴ��� Ȯ��
    private void Awake()
    {
        instance = this;
        shopUiPrefabs = Resources.Load<GameObject>("Prefabs/Shop_UI");
        keyX = transform.GetChild(0).GetChild(0).gameObject;
        uiOpen = false;
    }
    private void Update()
    {
        if (inShop && Input.GetKeyDown(KeyCode.X) && !uiOpen)
        {
            GameObject shopUi = Instantiate(shopUiPrefabs);
            uiOpen = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            keyX.SetActive(true);
            inShop = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        keyX.SetActive(false);
        inShop = false;
    }


}
