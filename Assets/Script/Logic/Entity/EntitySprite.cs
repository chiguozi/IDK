using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@todo 添加战斗属性
public class EntitySprite : EntityBase
{
    public EntityFightAttributeData attributeData;
    protected UnitSMManager _smMgr;

    protected override void Init()
    {
        base.Init();
        InitStateMachine();
    }

    protected override void AddComponent()
    {
        AddComponent<MoveComponent>();
        AddComponent<ActionComponent>();
        AddComponent<SkillControlComponent>();
    }

    protected override void RegistEvent()
    {
        base.RegistEvent();
        _eventMgr.Regist(ComponentEvents.OnMoveEnd, OnMoveEnd);
        _eventMgr.Regist(ComponentEvents.OnSkillEnd, OnSkillEnd);
    }

    public override void InitDatas()
    {
        base.InitDatas();
        attributeData = new EntityFightAttributeData();
    }


    public void RotateByDirAndSpeed(float x, float z, float speed)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().RotateByDirAndSpeed(x, z, speed);
        }
    }

    //特殊效果移动
    public void MoveByPos(float x, float z, float speed)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().MoveByPosAndSpeed(x, z, speed);
        }
    }

    public void MoveByWayPoints(List<Vector3> wayPoints, float speed)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().MoveByWayPoints(wayPoints, speed);
        }
    }

    public void MoveByTargetAndSpeed(Transform target, float speed, float reachDistance = 0.1f, float checkInterval = 0.1f)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().MoveByTargetAndSpeed(target, speed, reachDistance, checkInterval);
        }
    }

    public void SetAngleByDir(float dirX, float dirZ)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().SetAngleByDir(dirX, dirZ);
        }
    }


    protected virtual void OnMoveEnd(object obj)
    {
        //暂时处理
        SendSMEvent(UnitStateEvent.ClearState);
    }

    protected virtual void OnSkillEnd(object obj)
    {
        //暂时处理
        SendSMEvent(UnitStateEvent.ClearState);
        //技能结束全局事件？
    }

    protected virtual void InitStateMachine()
    {
        _smMgr = new UnitSMManager(this);
        _smMgr.InitState(UnitState.Idle, new SMIdle());
        _smMgr.RegistState(UnitState.Run, new SMRun());
        _smMgr.RegistState(UnitState.Skill, new SMSkill());
    }

    public void SendSMEvent(UnitStateEvent evt, params object[] param)
    {
        if (_smMgr != null)
        {
            _smMgr.ProcessEvent(evt, param);
        }
    }


    public virtual void UseSkill()
    {
        SendSMEvent(UnitStateEvent.UseSkill);
        GetComponent<SkillControlComponent>().UseSkill(1);
    }

    bool CheckCanUseSkill(int skillId)
    {
        if (!HasComponent<SkillControlComponent>())
            return false;
        return GetComponent<SkillControlComponent>().CheckCanUseSkill(skillId);
    }

}
