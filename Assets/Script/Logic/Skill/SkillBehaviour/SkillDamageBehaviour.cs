using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageBehaviour : SkillBehaviourBase
{
    int _damageId;
    DamageChecker _checker;
    EntityBase _owner;

    public override bool needUpdate
    {
        get
        {
            return true;
        }
    }

    public override void Setup(List<string> valueList)
    {
        base.Setup(valueList);
        _damageId = StringUtil.ParseIntFromList(valueList, 1);
    }

    public override void Trigger()
    {
        _owner = GetOwner();
        _checker = new DamageChecker(_damageId, runtimeData);
        _checker.OnHitCall = HitTarget;
    }


    public override void Update(float delTime)
    {
      
        _checker.UpdatePos(_owner.position);
        _checker.Update(delTime);
    }

    void HitTarget(List<EntityBase> targetList)
    {
        Debug.LogError("hit");
        _comEventCtrl.Send(ComponentEvents.OnSkillHit, _damageId, targetList, runtimeData);
    }
}
