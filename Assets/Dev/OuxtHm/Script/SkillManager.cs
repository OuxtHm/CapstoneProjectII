using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public int[] commonSkillNum = new int[2];
    public int ultSkillNum;
    public int[] passiveNum = new int[2];   
    public GameObject[] skill = new GameObject[13];
    private void Awake()
    {
        instance = this;
    }

    public void CreateSkill(int number, Transform createObjectParent)
    {
        GameObject createIcon = Instantiate(skill[number], createObjectParent);
        createIcon.transform.SetSiblingIndex(0);
    }
}
