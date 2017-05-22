using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControlComponent : ComponentBase
{
    public List<int> skillIdList = new List<int>();
    public List<int> runtimeSkillIdList = new List<int>();
    public Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();

    public Skill currentSkill = null;
    public List<SubSkill> currentSubSkillList;

    public void SetSkillList(List<int> skillList)
    {
        var skill = new Skill();
        skill.lifeTime = 0.5f;
        var subSkill = new SubSkill();
        subSkill.Init(_eventCtrl, _ownerId);
        skill.subSkillInfoList.Add(subSkill);
        skillMap.Add(1, skill);
    }

    protected override void RegistEvent()
    {
        base.RegistEvent();
        SetSkillList(null);
    }


    public void UseSkill(int skillId)
    {
        //技能检测放在外面
        Skill skill;
        if(!skillMap.TryGetValue(skillId, out skill))
        {
            Debug.LogErrorFormat("找不到技能id: {0}", skillId);
            return;
        }
        if (currentSkill != null)
            StopSkill();
        //选择目标 朝向

        currentSkill = skill;
        currentSubSkillList = skill.subSkillInfoList;  
    }

    public void StopSkill()
    { }

    public bool CheckCanUseSkill(int skillId)
    {
        return true;
    }


    bool CheckState()
    {
        return true;
    }

    bool CheckCD()
    {
        return true;
    }

    public override void Update(float delTime)
    {
        if (currentSkill == null)
            return;
        currentSkill.Update(delTime);  
        currentSkill.lifeTime -= delTime;
        if(currentSkill.lifeTime <= 0)
        {
            currentSkill.Release();
            currentSkill = null;
            Send(ComponentEvents.OnSkillEnd);
        }
    }
}
