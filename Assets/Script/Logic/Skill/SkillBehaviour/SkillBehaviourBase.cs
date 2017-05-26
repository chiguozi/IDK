using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillBehaviourBase
{
    public SubSkill subSkill;
    public bool needUpdate = false;
    protected EventController _comEventCtrl;

    public void SetEventController(EventController eventCtrl)
    {
        _comEventCtrl = eventCtrl;
    }

    public virtual void Trigger()
    {

    }

    //伤害检测会使用  @todo  移走
    public virtual void Update()
    {
    }

    protected EntitySprite GetOwner()
    {
        return World.entites[subSkill.ownerId] as EntitySprite;
    }

    //valueList[0]为behaviortype  需要跳过
    public virtual void Setup(List<string> valueList)
    { }

    protected float ParseFloat(List<string> valueList, int index, float def = 0)
    {
        if (index >= valueList.Count)
            return def;
        return StringUtil.ParseFloat(valueList[index], def);
    }

    protected int ParseInt(List<string> valueList, int index, int def = 0)
    {
        if (index >= valueList.Count)
            return def;
        return StringUtil.ParseInt(valueList[index], def);
    }

    protected string ParseString(List<string> valueList, int index, string def = "")
    {
        if (index >= valueList.Count)
            return def;
        return valueList[index];
    }


}







