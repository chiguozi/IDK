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
}

public class SkillAnimationBehaviour : SkillBehaviourBase
{
    public string clipName;
    public float speed = 1;
    //duration 暂时不配置
    //public float duration = 0.1f;
    public bool force = true;
    public float normalizeTime = 0;

    public override void Trigger()
    {
        base.Trigger();

        _comEventCtrl.Send(ComponentEvents.CrossFade, clipName, speed, force, normalizeTime);
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



