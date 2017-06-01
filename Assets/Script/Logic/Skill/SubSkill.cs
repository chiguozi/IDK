using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill
{
    public int id;
    public float delay;
    public CfgSubSkill cfg;
    public List<SkillBehaviourBase> skillActionList = new List<SkillBehaviourBase>();

    public bool hasTriggered = false;

    public List<SkillBehaviourBase> updateActionList = new List<SkillBehaviourBase>();

    public uint ownerId;
    public uint attackedId;
    public Vector3 selectedPos;
    public Vector3 selectedEuler;

    public void Init(EventController eventMgr, uint playerId, List<string> args)
    {
        ownerId = playerId;
        id = StringUtil.ParseInt(args[0]);
        delay = StringUtil.ParseFloat(args[1], 0);
        cfg = ConfigTextManager.Instance.GetConfig<CfgSubSkill>(id);
        if(cfg == null)
        {
            Debug.LogError("找不到子技能 id :" + id);
            return;
        }
        for(int i = 0; i < cfg.skillActionList.Count; i++)
        {
            var behaviour = SkillBehaviourFactory.Create(cfg.skillActionList[i], this, eventMgr);
            skillActionList.Add(behaviour);
        }
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
