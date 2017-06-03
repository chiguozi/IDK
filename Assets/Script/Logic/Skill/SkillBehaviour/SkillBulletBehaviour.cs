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
        _bulletId = StringUtil.ParseIntFromList(valueList, 1);
        _eulerOffset = StringUtil.ParseVector3FromList(valueList, 2, Vector3.zero);
        _posOffset = StringUtil.ParseVector3FromList(valueList, 3, Vector3.zero);
    }


    public override void Trigger()
    {
        base.Trigger();
        var owner = GetOwner();
        var bullet = Bullet.CreateBullet(_bulletId, subSkill, _comEventCtrl, subSkill.runtimeData.startPos + _posOffset, Quaternion.Euler( subSkill.runtimeData.euler + _eulerOffset) * Vector3.forward);
        bullet.Fire();
    }
}
