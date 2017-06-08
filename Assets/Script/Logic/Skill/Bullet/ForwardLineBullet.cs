using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardLineBullet : Bullet
{
    bool _hitDispose;
    //移动一段距离后停止  李元芳大招
    float _maxDistance;


    float _movedDistance;

    protected override void ParseArgs(List<float> args)
    {
        _hitDispose = Util.GetFloatFromList(args, 0) == 1;
        _maxDistance = Util.GetFloatFromList(args, 1);
    }
    protected override void OnMove(float interval)
    {
        if (_maxDistance > 0 && _movedDistance >= _maxDistance)
            return;
        MoveForward(interval);
        _movedDistance += _cfg.speed;
    }

    protected override void OnHit(List<EntityBase> hitList)
    {
        if (_hitDispose)
            DisposeSelf();
    }

}
