using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBulletBehaviour : SkillBehaviourBase
{
    int _bulletId;
    Vector3 _eulerOffset;
    Vector3 _posOffset;

    public override void Setup(List<string> valueList)
    {
        base.Setup(valueList);
        _bulletId = ParseInt(valueList, 1);
        _eulerOffset = ParseVector3(valueList, 2, Vector3.zero);
        _posOffset = ParseVector3(valueList, 3, Vector3.zero);
    }


    public override void Trigger()
    {
        base.Trigger();
        var owner = GetOwner();
        var bullet = Bullet.CreateBullet(_bulletId, subSkill, _comEventCtrl, owner.position + _posOffset, Quaternion.Euler( owner.eulers + _eulerOffset) * Vector3.forward);
        bullet.Fire();
    }
}
