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
        bullet._initPos = pos;
        bullet._initEulers = eulers;
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

    uint _ownerId;
    uint _defaultTargetId;

    //动态
    const float MOVE_CHECK_INTERNAL = 0.05f;
    public uint uid;
    Vector3 _forward;
    float _movedInterval = 0;
    float _timeSinceFire = 0;
    //float _sqrMoveDistance = 0;

    //直接使用Effect的位置作为bullet的位置
    public Vector3 position
    {
        get
        {
            if (_effect != null)
                return _effect.position;
            return Vector3.zero;
        }
    }

    public Vector3 eulers
    {
        get
        {
            if (_effect != null)
                return _effect.eulers;
            return Vector3.zero;
        }
    }

    public void Fire()
    {
        if(_cfg == null)
        {
            Debug.LogError("找到不CfgBullet :" + _id);
            return;
        }
        _forward = ( _initEulers);
        _effect = Effect.CreateEffect(_cfg.url, _initPos + _forward, _forward,_ownerId, -1);
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
        _effect.position = _effect.position + _forward * _cfg.speed * interval;
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
