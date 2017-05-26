using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillActionBehaviour : SkillBehaviourBase
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

    public override void Setup(List<string> valueList)
    {
        clipName = ParseString(valueList, 1);
        speed = ParseFloat(valueList, 2, speed);
        force = ParseInt(valueList, 3, 0) == 1;
        normalizeTime = ParseFloat(valueList, 4, normalizeTime);
    }

  
}
