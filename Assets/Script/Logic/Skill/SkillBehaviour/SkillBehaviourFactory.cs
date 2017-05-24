using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillBehaviourType
{
    None,
    Action = 1,
    Effect = 2,
}


public class SkillBehaviourFactory
{
    public static SkillBehaviourBase Create(List<string> args, SubSkill skill, EventController con)
    {
        if (args == null || args.Count == 0)
        {
            Debug.LogError("subskill" + skill.id + "配置错误");
            return null;
        }
        SkillBehaviourType type = (SkillBehaviourType)StringUtil.ParseInt(args[0], 0);
        SkillBehaviourBase behaviour = null;
        switch (type)
        {
            case SkillBehaviourType.Action:
                behaviour = new SkillActionBehaviour();
                break;
            case SkillBehaviourType.Effect:
                behaviour = new SkillEffectBehaviour();
                break;
        }
        if(behaviour != null)
        {
            behaviour.subSkill = skill;
            behaviour.SetEventController(con);
            behaviour.Setup(args);
        }
        return behaviour;
    }
}
