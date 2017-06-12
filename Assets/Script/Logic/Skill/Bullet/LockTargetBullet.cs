using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTargetBullet : Bullet
{
    EntitySprite _target;

    protected override void OnStart()
    {
        _target = World.GetEntity(_runtimeData.attackedId) as EntitySprite;
    }

    protected override void OnMove(float interval)
    {
        if (_target == null || _target.IsDead())
            MoveForward(interval);
        else
            MoveByTarget(_target, interval);
    }

    protected override void OnHit(List<EntityBase> hitList)
    {
        //默认击中后消失,如果有特殊操作 放到子弹击中的效果中处理
        DisposeSelf();
    }
}
