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

    }

    protected override void RegistEvent()
    {
        base.RegistEvent();
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
        if(currentSkill != null && currentSubSkillList.Count > 0)
        {
            for(int i = 0; i < currentSubSkillList.Count; i++)
            {
                currentSubSkillList[i].Update(delTime);
            }
        }
    }
}
