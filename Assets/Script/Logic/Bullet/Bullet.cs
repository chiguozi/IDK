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
        bullet._euler = eulers.normalized;
        bullet.uid = Util.GetClientUid();
        bullet._cfg = ConfigTextManager.Instance.GetConfig<CfgBullet>(id);
        EventManager.Send(Events.FightEvent.AddBullet, bullet);
        return bullet;
    }

    //配置
    //@todo _speed 做成公式
    //float _maxDistance;
    int _id;
    CfgBullet _cfg; 
    BulletMoveType _moveType = BulletMoveType.Forward;


    //传入  offset由上层计算
    Vector3 _initPos;
    Vector3 _initEulers;
    EventController _eventControl;
    Effect _effect;

    //存放数据
    SubSkill _subSkill;

    //动态
    const float MOVE_CHECK_INTERNAL = 0.05f;
    public uint uid;
    float _movedInterval = 0;
    float _timeSinceFire = 0;
    //float _sqrMoveDistance = 0;

    Vector3 _position;
    Vector3 _euler;
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
            return _euler;
        }
        set
        {
            _euler = value;
            if (_effect != null)
                _effect.eulers = _euler;
        }
    }

    public void Fire()
    {
        if(_cfg == null)
        {
            Debug.LogError("找到不CfgBullet :" + _id);
            return;
        }
        _forward = _euler;
        _effect = Effect.CreateEffect(_cfg.url, _position + _forward, _forward, _subSkill.ownerId, -1);
    }

    public void Update(float delTime)
    {
        if (_effect == null || _cfg == null)
            return;
        _timeSinceFire += delTime;
        if(_timeSinceFire >= _cfg.lifeTime)
        {
            EventManager.Send(Events.FightEvent.RemoveBullet, uid);
            return;
        }
        Move(delTime);
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
    }


    protected void MoveForward(float interval)
    {
       position = position + _forward * _cfg.speed * interval;
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
