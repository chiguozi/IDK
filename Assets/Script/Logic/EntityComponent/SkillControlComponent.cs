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

    }

    bool CheckState()
    {
        return true;
    }

    bool CheckCD()
    {
        return true;
    }

    public override void Update()
    {
        base.Update();

    }
}
