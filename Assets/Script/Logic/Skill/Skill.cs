using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    //configSkill
    public CfgSkill cfg;
    public int skillId;
    public int skillUid;
    public float currentCd;

    public float lifeTime;  //技能时长  暂时还没想到好的方法

    public List<SubSkill> subSkillInfoList = new List<SubSkill>();

    public void Init(int skillId, uint ownerId, EventController com)
    {
        this.skillId = skillId;
        cfg = ConfigTextManager.Instance.GetConfig<CfgSkill>(skillId);
        if(cfg == null)
        {
            Debug.LogError("找不到技能Id : " + skillId);
            return;
        }
        for(int i = 0; i < cfg.subSkillList.Count; i++)
        {
            SubSkill sub = new SubSkill();
            sub.Init(com, ownerId, cfg.subSkillList[i]);
            subSkillInfoList.Add(sub);
        }
    }
    public void RefreshCd(float deltime)
    {

    }

    public void Release()
    {
        for(int i = 0; i < subSkillInfoList.Count; i++)
        {
            subSkillInfoList[i].Release();
        }
        lifeTime = 1.3f;
    }
    public void Update(float delTime)
    {
        if (subSkillInfoList.Count > 0)
        {
            for (int i = 0; i < subSkillInfoList.Count; i++)
            {
                subSkillInfoList[i].Update(delTime);
            }
        }
    }
}
