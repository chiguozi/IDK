using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletMoveType
{
    Static = 1,  //指定位置
    Forward,
    LockTarget, //(可以设置角速度)
    //特殊移动轨迹
}

public class Bullet
{
    public static Bullet CreateBullet(int id, SubSkill subSkill, EventController ctrl, Vector3 pos, Vector3 eulers)
    {
        var bullet = new Bullet();
        bullet._id = id;
        bullet._subSkill = subSkill;
        bullet._eventControl = ctrl;
        bullet._position = pos;
        bullet._eulers = eulers.normalized;
        bullet.uid = Util.GetClientUid();
        bullet._cfg = ConfigTextManager.Instance.GetConfig<CfgBullet>(id);
        bullet._moveType = (BulletMoveType)bullet._cfg.moveType;
        EventManager.Send(Events.FightEvent.AddBullet, bullet);
        return bullet;
    }

    //配置
    //@todo _speed 做成公式
    //float _maxDistance;
    int _id;
    CfgBullet _cfg; 
    BulletMoveType _moveType = BulletMoveType.Forward;


    EventController _eventControl;
    Effect _effect;

    //存放数据
    SubSkill _subSkill;

    //动态
    const float MOVE_CHECK_INTERNAL = 0.05f;
    public uint uid;
    float _movedInterval = 0;
    float _timeSinceFire = 0;

    EntityBase _defalutTarget;

    DamageChecker _damageChecker;
    //float _sqrMoveDistance = 0;

    Vector3 _position;
    Vector3 _eulers;
    Vector3 _forward;


    //直接使用Effect的位置作为bullet的位置
    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            if (_effect != null)
                _effect.position = position;
        }
    }

    public Vector3 eulers
    {
        get
        {
            return _eulers;
        }
        set
        {
            _eulers = value;
            if (_effect != null)
                _effect.eulers = _eulers;
        }
    }

    public void Fire()
    {
        if(_cfg == null)
        {
            Debug.LogError("找到不CfgBullet :" + _id);
            return;
        }

        //放着感觉怪怪的 做成配置？
        if (_moveType == BulletMoveType.Static)
            _position = _subSkill.runtimeData.targetPos;
        _defalutTarget = World.GetEntity(_subSkill.runtimeData.attackedId);
        if (_moveType == BulletMoveType.LockTarget && _defalutTarget == null)
            _moveType = BulletMoveType.Forward;

        _damageChecker = new DamageChecker(_cfg.damageCheckId, _subSkill.runtimeData);
        _damageChecker.OnHitCall = OnHit;

        _forward = Quaternion.Euler(_eulers) * Vector3.forward;
        _effect = Effect.CreateEffect(_cfg.url, _position, _eulers, _subSkill.runtimeData.ownerId, -1);
    }

    public void Update(float delTime)
    {
        if (_effect == null || _cfg == null)
            return;
        _timeSinceFire += delTime;
        if(_timeSinceFire >= _cfg.lifeTime)
        {
            DisposeSelf();
            return;
        }
        Move(delTime);
        _damageChecker.Update(delTime);
    }

    protected virtual void Move(float delTime)
    {
        _movedInterval -= delTime;
        if (_movedInterval > 0)
            return;
        _movedInterval += MOVE_CHECK_INTERNAL;

        if(_moveType == BulletMoveType.Forward)
        {
            MoveForward(MOVE_CHECK_INTERNAL);
        }
        else if(_moveType == BulletMoveType.LockTarget)
        {
            MoveByTarget(MOVE_CHECK_INTERNAL);
        }
        _damageChecker.UpdatePos(_position);
    }


    protected void MoveForward(float interval)
    {
       position = position + _forward * _cfg.speed * interval;
    }

    protected void MoveByTarget(float interval)
    {
        if(_defalutTarget == null)
        {
            _moveType = BulletMoveType.Forward;
            MoveForward(interval);
            return;
        }
        var distance = ( _defalutTarget.position - position ).XZMagnitude();
        if(distance < _cfg.speed * interval)
        {
            position = _defalutTarget.position;
            return;
        }

        //@todo 角速度
        //有角速度不能使用from * (1f - factor) + to * factor方式  需要先计算角度
        eulers = Quaternion.LookRotation(_defalutTarget.position - position).eulerAngles;
        var factor = ( _cfg.speed * interval ) / distance;
        position = position * ( 1 - factor ) + _defalutTarget.position *  factor ;
     
    }

    void DisposeSelf()
    {
        EventManager.Send(Events.FightEvent.RemoveBullet, uid);
    }

    void OnHit(List<EntityBase> hitList)
    {
        if(_cfg.hitNotDispose == 0)
        {
            DisposeSelf();
        }
        _subSkill.runtimeData.hitPos = _position;
        _subSkill.runtimeData.hitEuler = _eulers;
        _eventControl.Send(ComponentEvents.OnSkillHit, _cfg.damageCheckId, hitList, _subSkill.runtimeData);
    }


    public virtual void Release()
    {
        if(_effect != null)
        {
            _effect.Release();
            _effect = null;
        }
    }

}
