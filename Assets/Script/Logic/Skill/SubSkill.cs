using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill
{
    public int id;
    public float delay;
    public uint ownerId;
    public List<SkillBehaviourBase> skillActionList = new List<SkillBehaviourBase>();

    public bool hasTriggered = false;

    public List<SkillBehaviourBase> updateActionList = new List<SkillBehaviourBase>();




    //存储skillaction的公共属性

    public void Init(ComponentEventManager eventMgr)
    {
        delay = 0;
        var skillAction = new SkillAnimationBehaviour();
        skillAction.SetEventManager(eventMgr);
        skillAction.clipName = "atk_1";
        skillAction.duration = 0.1f;
        skillActionList.Add(skillAction);
        //初始化SkillAction
    }

    public void Update(float delTime)
    {
        if(!hasTriggered && delay >= 0)
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
        //skillActionList.Clear();
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
