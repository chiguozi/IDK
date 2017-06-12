using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBullet : Bullet
{

    protected override void OnStart()
    {
        base.OnStart();
        _position = _runtimeData.targetPos;
    }

    protected override void OnMove(float interval)
    {
        return;
    }
}
