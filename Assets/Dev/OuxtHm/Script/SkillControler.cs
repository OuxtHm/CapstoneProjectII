using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControler : MonoBehaviour
{
    public SkillScriptable skill;
    public int num;
    public string _name;
    public string description;
    public int level;
    public float coolTime;
    public int price;
    public float coefficient;

    private void Awake()
    {
        num = skill.number;
        _name = skill.name;
        description = skill.description;
        level = skill.level[0];
        coolTime = skill.coolTime;
        price = skill.price;
        coefficient = skill.coefficient;
    }
}
