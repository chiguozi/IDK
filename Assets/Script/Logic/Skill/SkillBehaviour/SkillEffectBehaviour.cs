using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectBehaviour : SkillBehaviourBase
{
    public int effectId;
    public float lifeTime;
    public EffectType effectType = EffectType.Normal;

    public override void Setup(List<string> valueList)
    {
        effectId = ParseInt(valueList, 1, 0);
        lifeTime = ParseFloat(valueList, 2, 3);
        effectType = (EffectType)ParseInt(valueList, 3, (int)effectType);
    }


    public override void Trigger()
    {
        base.Trigger();
        var owner = GetOwner();

        var cfg = ConfigTextManager.Instance.GetConfig<CfgEffect>(effectId);
        if (cfg == null)
        {
            Debug.LogError("找不到effectid :" + effectId);
            return;
        }
        Vector3 pos = cfg.posOffset;
        Vector3 euler = cfg.eulerOffset;
        Transform bone = null;
        if (string.IsNullOrEmpty(cfg.bonePath) || owner.GetBone(cfg.bonePath) == null)
        {
            pos += owner.position;
            euler += owner.eulers;
        }
        else
        {
            bone = owner.GetBone(cfg.bonePath);
        }
        var effect = Effect.CreateEffect(cfg.url, pos, euler, owner.uid, lifeTime, bone);
        effect.OnEffectEnd = OnEffectRelease;
        _comEventCtrl.Send(ComponentEvents.OnAddRoleEffect, effect.uid);
    }

    void OnEffectRelease(Effect effect)
    {
        _comEventCtrl.Send(ComponentEvents.OnRemoveRoleEffect, effect.uid);
    }
}