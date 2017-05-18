using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill
{
    public int id;
    public float delay;
    public List<SkillActionBase> skillActionList = new List<SkillActionBase>();

    public bool hasTriggered = false;

    public List<SkillActionBase> updateActionList = new List<SkillActionBase>();

    public void Init()
    {
        //初始化SkillAction
    }

    public void Update(float delTime)
    {
        if(!hasTriggered && delay > 0)
        {
            delay -= delTime;
            if(delay <= 0)
            {
                hasTriggered = true;
                Trigger();
            }
        }
        if (!hasTriggered)
            return;
        UpdateSkillActions();
    }

    //重用使用
    public void Release()
    {
        updateActionList.Clear();
        hasTriggered = false;
        skillActionList.Clear();
        delay = 0;
        id = 0;
    }

    void Trigger()
    {
        for(int i = 0; i < skillActionList.Count; i++)
        {
            skillActionList[i].Trigger();
            if (skillActionList[i].needUpdate)
                updateActionList.Add(skillActionList[i]);
        }
    }
    
    void UpdateSkillActions()
    {
        for(int i = 0; i  < updateActionList.Count; i++)
        {
            updateActionList[i].Update();
        }
    }

}
