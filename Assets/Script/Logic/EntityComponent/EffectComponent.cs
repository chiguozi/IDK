using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//可以删掉，EffectManager中查找Owner来单独处理特效
public class EffectComponent : ComponentBase
{
    HashSet<uint> _usingEffectIdSet = new HashSet<uint>();

    protected override void RegistEvent()
    {
        base.RegistEvent();
        Regist<uint>(ComponentEvents.OnAddRoleEffect, OnAddEffect);
        Regist<uint>(ComponentEvents.OnRemoveRoleEffect, OnRemoveEffect);
    }


    void OnAddEffect(uint effId)
    {
        if (!_usingEffectIdSet.Contains(effId))
            _usingEffectIdSet.Add(effId);
    }


    void OnRemoveEffect(uint effId)
    {
        if (_usingEffectIdSet.Contains(effId))
            _usingEffectIdSet.Remove(effId);
    }



}
