using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControler : MonoBehaviour
{
    public SkillScriptable skill;
    public string _name;
    public string description;
    public int level;
    public float coolTime;
    public int price;

    private void Awake()
    {
        _name = skill.name;
        description = skill.description;
        level = skill.level[0];
        coolTime = skill.coolTime;
        price = skill.price;
    }
}
