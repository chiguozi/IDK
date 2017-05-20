using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviourBase
{
    public SubSkill subSkill;
    public bool needUpdate = false;
    protected ComponentEventManager _comEventMgr;

    public void SetEventManager(ComponentEventManager eventMgr)
    {
        _comEventMgr = eventMgr;
    }

    public virtual void Trigger()
    {

    }

    //伤害检测会使用  @todo  移走
    public virtual void Update()
    {
    }
}

public class SkillAnimationBehaviour : SkillBehaviourBase
{
    public string clipName;
    public float speed = 1;
    public float duration = 0.1f;
    public bool force = true;
    public float normalizeTime = 0;

    public override void Trigger()
    {
        base.Trigger();
        var param = new EventParams();
        param.sParam1 = clipName;
        param.fParam1 = speed;
        param.fParam2 = duration;
        param.fParam3 = normalizeTime;
        param.bParam1 = force;
        _comEventMgr.Send(ComponentEvents.CrossFade, param);
    }
}

public class SkillEffectBehaviour : SkillBehaviourBase
{
    public string url;
    public Vector3 posOffset;
    public Vector3 eulersOffset;
    public string bonePath;
    public float lifeTime;
    public EffectType effectType = EffectType.Normal;


    public override void Trigger()
    {
        base.Trigger();
    }
}



