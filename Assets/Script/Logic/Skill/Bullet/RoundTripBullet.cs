using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTripBullet : Bullet
{
    enum ReturnPosType
    {
        //返回自己
        BackToSelf = 1,
        //返回
        ReverseDirection,
    }

    enum ReturnType
    {
        //到达距离返回
        Distance = 1,
        HitTarget,
    }

    enum MoveState
    { 
        Forward,   //向前移动
        Backward,  //直线转向
        BackToSelf, //返回玩家
    }

    ReturnPosType _returnPosType;
    ReturnType _returnType;
    float _maxDistance;
    //额外偏移量
    float _hitDistanceOffset;
    //返回时 替换DamageChecker
    int _backDamageCheckerId;



    float _movedDistance;
    MoveState _moveState = MoveState.Forward;
    bool _hasHit = false;
    EntitySprite _selfEntity;

    protected override void ParseArgs(List<float> args)
    {
        _returnPosType = (ReturnPosType)Util.GetFloatFromList(args, 0, 1);
        _returnType = (ReturnType)Util.GetFloatFromList(args, 1, 1);
        _maxDistance = Util.GetFloatFromList(args, 2, 2);
        _hitDistanceOffset = Util.GetFloatFromList(args, 3, 0);
        _backDamageCheckerId = (int)Util.GetFloatFromList(args, 4, _cfg.damageCheckId);
    }


    protected override void OnStart()
    {
        _selfEntity = World.GetEntity(_subSkill.runtimeData.ownerId) as EntitySprite;
    }


    protected override void OnMove(float interval)
    {
        if(_moveState == MoveState.BackToSelf && _selfEntity != null && !_selfEntity.IsDead())
        {
            MoveByTarget( _selfEntity,interval, true);
        }
        else
        {
            if (_movedDistance >= _maxDistance)
            {
                GoBack();
                return;
            }
            MoveForward(interval);
        }
        _movedDistance += _cfg.speed * interval;
    }

    void GoBack()
    {
        if(_returnPosType == ReturnPosType.BackToSelf)
        {
            _moveState = MoveState.BackToSelf;
        }
        else
        {
            _moveState = MoveState.Backward;
            _forward = -_forward;
            eulers = -eulers;
        }
    }

    protected override void OnHit(List<EntityBase> hitList)
    {
        if(!_hasHit && _returnType == ReturnType.HitTarget)
        {
            _hasHit = true;
            _maxDistance = _movedDistance + _hitDistanceOffset;
        }
    }
}
