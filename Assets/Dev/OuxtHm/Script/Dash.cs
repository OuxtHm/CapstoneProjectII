using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public Image firstEnergy;
    public Image secondEnergy;

    public bool isFillingFirst = false;        // ���� ������ 
    public bool isFillingSecond = false;       // ���� ������ 

    private void Awake()
    {
        firstEnergy = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        secondEnergy = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isFillingSecond)
            {
                StartCoroutine(SecondFill());
                
            }
            else if((!isFillingFirst))
            {
                StartCoroutine(FirstFill());
            }
        }
    }

    public IEnumerator FirstFill()  // ù��° ������ ����
    {
        isFillingSecond = false;
        isFillingFirst = true;
        float duration = 3f;
        float elapsedTime = 0f;
        float startValue = secondEnergy.fillAmount;
        firstEnergy.fillAmount = startValue;
        secondEnergy.fillAmount = 0;
        float t = duration - (duration * firstEnergy.fillAmount);

        while (elapsedTime < t)
        {
            elapsedTime += Time.deltaTime;
            float fillRatio = elapsedTime / t;
            firstEnergy.fillAmount = Mathf.Lerp(startValue, 1f, fillRatio); // ����� �κ�
            yield return null;
        }
        firstEnergy.fillAmount = 1f;
        isFillingFirst = false;
        StartCoroutine(SecondFill());
    }

    public IEnumerator SecondFill() // �ι�° ������ ����
    {
        isFillingSecond = true;

        float duration = 3f;
        float elapsedTime = 0f;
        secondEnergy.fillAmount = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            secondEnergy.fillAmount = Mathf.Lerp(0f, 1f, t);
            if (isFillingFirst)
            {
                secondEnergy.fillAmount = 0f;
                yield break;
            }

            yield return null;
        }
        secondEnergy.fillAmount = 1;

        isFillingSecond = false;
    }
}
