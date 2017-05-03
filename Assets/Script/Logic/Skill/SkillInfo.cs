using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo
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

    public List<SubSkill> subSkillInfoList = new List<SubSkill>();

}
