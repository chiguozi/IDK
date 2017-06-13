using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Static = 1,  //定点
    LockTarget, //锁定目标
    ForwardLine,    //直线
    RoundTrip,  //往返
}

public class Bullet
{
    public static Bullet CreateBullet(int id, SkillRuntimeData runtimeData, EventController ctrl, Vector3 pos, Vector3 eulers)
    {
        var cfg = ConfigTextManager.Instance.GetConfig<CfgBullet>(id);
        if (cfg == null)
            return null;
        var type = (BulletType)cfg.bulletType;
        Bullet bullet;
        switch (type)
        {
            case BulletType.Static:
                bullet = new StaticBullet();
                break;
            case BulletType.ForwardLine:
                bullet = new ForwardLineBullet();
                break;
            case BulletType.LockTarget:
                bullet = new LockTargetBullet();
                break;
            case BulletType.RoundTrip:
                bullet = new RoundTripBullet();
                break;
            default:
                bullet = new StaticBullet();
                break;
        }
        bullet._id = id;
        //subSkill可能被重用 导致引用丢失
        bullet._runtimeData = runtimeData;
        bullet._eventControl = ctrl;
        bullet._position = pos;
        bullet._eulers = eulers;
        bullet.uid = Util.GetClientUid();
        bullet._cfg = cfg;
        bullet.ParseArgs(cfg.argList);
        EventManager.Send(Events.FightEvent.AddBullet, bullet);
        return bullet;
    }

    //配置
    //@todo _speed 做成公式
    protected int _id;
    protected CfgBullet _cfg; 


    protected EventController _eventControl;
    public EventController eventControl { get { return _eventControl; } }
    protected Effect _effect;

    //存放数据
    //protected SubSkill _subSkill;

    protected SkillRuntimeData _runtimeData;
    public SkillRuntimeData runTimeData { get { return _runtimeData; } }


    //动态
    //const float MOVE_CHECK_INTERNAL = 0.05f;
    public uint uid;
    //float _movedInterval = 0;
    float _timeSinceFire = 0;

    DamageChecker _damageChecker;

    protected Vector3 _position;
    protected Vector3 _eulers;
    protected Vector3 _forward;

    //子子弹深度
    public int childDepth = 0;

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

        OnStart();

        _damageChecker = new DamageChecker(_cfg.damageCheckId, _runtimeData);
        _damageChecker.OnHitCall = HitTarget;

        _forward = Quaternion.Euler(_eulers) * Vector3.forward;
        _effect = Effect.CreateEffect(_cfg.url, _position, _eulers, _runtimeData.ownerId, -1);
    }
    public void Update(float delTime)
    {
        if (_effect == null || _cfg == null)
            return;
        _timeSinceFire += delTime;

        OnPreUpdate();

        if (_cfg.lifeTime > 0 && _timeSinceFire >= _cfg.lifeTime)
        {
            DisposeSelf();
            return;
        }

        //_movedInterval -= delTime;
        //if (_movedInterval <= 0)
        //{
        //    _movedInterval = MOVE_CHECK_INTERNAL;
        //    OnMove(_movedInterval);
        //    _damageChecker.UpdatePos(_position);
        //}
        OnMove(delTime);
        _damageChecker.UpdatePos(_position);

        _damageChecker.Update(delTime);
    }

    //替换
    protected void ReplaceChecker(int checkerId)
    {
        if (checkerId == _cfg.damageCheckId)
            return;
        _damageChecker = new DamageChecker(checkerId, _runtimeData);
        _damageChecker.OnHitCall = HitTarget;
    }


    protected void MoveForward(float interval)
    {
       position = position + _forward * _cfg.speed * interval;
    }

    protected void MoveByTarget(EntityBase target, float interval, bool disposeWhenReach = false)
    {
        var distance = ( target.position - position ).XZMagnitude();
        if(distance < _cfg.speed * interval)
        {
            position = target.position;
            if (disposeWhenReach)
                DisposeSelf();
            return;
        }

        //@todo 角速度
        //有角速度不能使用from * (1f - factor) + to * factor方式  需要先计算角度
        eulers = Quaternion.LookRotation(target.position - position).eulerAngles;
        var factor = ( _cfg.speed * interval ) / distance;
        position = position * ( 1 - factor ) + target.position *  factor ;
    }

    protected void DisposeSelf()
    {
        EventManager.Send(Events.FightEvent.RemoveBullet, uid);
    }

    public void Dispose()
    {
        DisposeSelf();
    }

    void HitTarget(List<EntityBase> hitList)
    {
        OnHit(hitList);

        BulletActionCenter.ExecuteBulletAction(this, _cfg.onHitActions);
        //受击效果会用到？ 2017-6-8 14:34:07
        _runtimeData.hitPos = _position;
        _runtimeData.hitEuler = _eulers;
        _eventControl.Send(ComponentEvents.OnSkillHit, _cfg.damageCheckId, hitList, _runtimeData);
    }


    public void Release()
    {
        OnRelease();
        if (_effect != null)
        {
            _effect.Release();
            _effect = null;
        }
        childDepth = 0;
    }


    protected virtual void OnRelease()
    {

    }

    //默认参数为float  如果需要string 再放到stringarg中  减少string转换为数字的开销 2017-6-8 15:04:14
    protected virtual void ParseArgs(List<float> args)
    { }
    //受击处理
    protected virtual void OnHit(List<EntityBase> hitList)
    { }
    //开始移动的初始化
    protected virtual void OnStart() { }
    //需要吗？ 2017-6-8 14:36:44
    protected virtual void OnPreUpdate() { }

    protected virtual void OnMove(float interval)
    {
    }


}
