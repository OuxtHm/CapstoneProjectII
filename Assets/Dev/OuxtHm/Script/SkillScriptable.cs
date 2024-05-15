using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SkillOb")]
public class SkillScriptable : ScriptableObject
{
    public int number;
    public string skillName;
    public string description;
    public int[] level =  {1, 2, 3};
    public float coolTime;
    public int price;
}
