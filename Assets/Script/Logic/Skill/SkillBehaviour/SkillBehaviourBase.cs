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
        return World.entites[subSkill.runtimeData.ownerId] as EntitySprite;
    }

    //valueList[0]为behaviortype  需要跳过
    public virtual void Setup(List<string> valueList)
    { }



}







