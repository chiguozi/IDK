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

    public float freezeTime;  //冻结时间

    public List<SubSkill> subSkillInfoList = new List<SubSkill>();

    public void Init()
    {

    }

    public void RefreshCd(float deltime)
    {

    }
}
