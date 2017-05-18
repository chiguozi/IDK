using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    //configSkill
    public int skillId;
    public int skillUid;
    public string skillName;
    public string iconUrl;
    public bool lockTarget;
    public bool faceTarget;
    public float skillCd;
    public float currentCd;

    public float lifeTime;  //技能时长  暂时还没想到好的方法

    public List<SubSkill> subSkillInfoList = new List<SubSkill>();

    public void Init()
    {

    }

    public void RefreshCd(float deltime)
    {

    }
}
