using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBulletBehaviour : SkillBehaviourBase
{
    int _bulletId;
    int _angleOffset;
    Vector3 _posOffset;

    public override void Setup(List<string> valueList)
    {
        base.Setup(valueList);
        _bulletId = StringUtil.ParseIntFromList(valueList, 1);
        _angleOffset = StringUtil.ParseIntFromList(valueList, 2, 0);
        _posOffset = StringUtil.ParseVector3FromList(valueList, 3, Vector3.zero);
    }


    public override void Trigger()
    {
        base.Trigger();
        var eulers = runtimeData.euler;
        eulers.y += _angleOffset;
        var bullet = Bullet.CreateBullet(_bulletId, runtimeData, _comEventCtrl, runtimeData.startPos + _posOffset, eulers, subSkillId);
        bullet.Fire();
    }
}
