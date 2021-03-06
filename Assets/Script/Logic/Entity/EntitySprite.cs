﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@todo 添加战斗属性
public class EntitySprite : EntityBase
{
    public EntityFightAttributeData attributeData;
    protected UnitSMManager _smMgr;
    //protected EntitySortHelper _sortHelper;
    //protected SkillRuntimeData _skillRuntimeData;

    protected override void Init()
    {
        base.Init();
        InitStateMachine();
    }

    public EntitySprite(EntityBaseData data) : base(data) { }

    public int HP { get { return attributeData.hp; } }

    protected override void AddComponent()
    {
        AddComponent<MoveComponent>();
        AddComponent<ActionComponent>();
        AddComponent<SkillControlComponent>();
        AddComponent<EffectComponent>();
    }

    protected override void RegistEvent()
    {
        base.RegistEvent();
        _eventCtrl.Regist(ComponentEvents.OnMoveEnd, OnMoveEnd);
        _eventCtrl.Regist(ComponentEvents.OnSkillEnd, OnSkillEnd);
        _eventCtrl.Regist<int, List<EntityBase>, SkillRuntimeData>(ComponentEvents.OnSkillHit, OnSkillHit);
        _eventCtrl.Regist<string>(ComponentEvents.OnActionEnd, OnActionEnd);
        _eventCtrl.Regist<float, float, float>(ComponentEvents.MoveToPos, MoveToPos);
        _eventCtrl.Regist<float, float>(ComponentEvents.FlashToPos, FlashToPos);
    }

    public override void InitDatas()
    {
        base.InitDatas();
        attributeData = new EntityFightAttributeData();
    }

    public virtual bool IsDead()
    {
        return false;
    }

    //特殊效果移动
    public void MoveToPos(float x, float z, float speed)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().MoveToPos(x, z, speed);
        }
    }

    public void FlashToPos(float x, float z)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().FlashToPos(x, z);
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
            GetComponent<MoveComponent>().MoveToTarget(target);
        }
    }

    public void SetAngleByDir(float dirX, float dirZ)
    {
        if (HasComponent<MoveComponent>())
        {
            GetComponent<MoveComponent>().SetDstAngle(dirX, dirZ);
        }
    }

    protected virtual void OnMoveEnd()
    {
        //暂时处理
        SendSMEvent(UnitStateEvent.ClearState);
    }

    protected virtual void OnSkillEnd()
    {
        //暂时处理
        SendSMEvent(UnitStateEvent.ClearState);
        //技能结束全局事件？
    }

    protected virtual void OnSkillHit(int damageId, List<EntityBase> hitList, SkillRuntimeData runtimeData)
    {
        //打中目标后效果回调
        //效果放在cfgDamage中
    }

    protected virtual void OnActionEnd(string clipName)
    {
        CrossFade(AnimStateName.IDLE, 1, false);
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

    public void Hited(SkillRuntimeData runtimeData)
    {
        CrossFade(AnimStateName.HIT, 1, false);
    }


    public virtual void UseSkill(int skillId)
    {
        if (!CheckCanUseSkill(skillId))
            return;
        CfgSkill cfg = ConfigTextManager.Instance.GetConfig<CfgSkill>(skillId);
        if (cfg == null)
            return;
        //选择目标
        uint targetId = Util.SkillSelectTarget(this, cfg);

        if(targetId == 0 && cfg.canUseIfNoTarget != 1)
        {
            Debug.LogError("没有目标");
            return;
        }

        //转向
        FaceTarget(cfg, targetId);
        //收集数据
        var skillRuntimeData = CreateRuntimeData(cfg, targetId);

        GetComponent<SkillControlComponent>().UseSkill(skillId, skillRuntimeData);

        SendSMEvent(UnitStateEvent.UseSkill);
    }

    //检测面向目标
    void FaceTarget(CfgSkill cfg, uint targetId)
    {
        if (cfg.faceTarget != 1)
            return;
        var entity = World.GetEntity(targetId);
        if (entity == null)
            return;
        var dir = (entity.position - position).normalized;
        SetAngleByDir(dir.x, dir.z);
    }


    //子弹会延长runtimeData的生命周期，下次使用技能时可能修改数据，所以每次使用技能创建一个单独的runtimedata
    SkillRuntimeData CreateRuntimeData(CfgSkill cfg, uint targetId)
    {
        var skillRuntimeData = new SkillRuntimeData();
        skillRuntimeData.ownerId = uid;
        skillRuntimeData.attackedId = targetId;
        skillRuntimeData.startPos = position;
        //targetPos需要特殊处理
        skillRuntimeData.targetPos = GetTargetPos(cfg, targetId);
        skillRuntimeData.euler = eulers;
        return skillRuntimeData;
    }

    /// <summary>
    /// 获取技能目标点，只对定点子弹有效
    /// 如果targetId 为0  默认位置为前方range距离的点
    /// </summary>
    /// <param name="cfg">技能配置</param>
    /// <param name="targetId">目标对象id</param>
    /// <param name="inputPos">todo  支持摇杆指定位置</param>
    /// <returns></returns>
    Vector3 GetTargetPos(CfgSkill cfg, uint targetId, Vector3 inputPos = default(Vector3))
    {
        var entity = World.GetEntity(targetId);
        if (entity != null)
            return entity.position;
        Vector3 pos = position + forward * cfg.range;
        return pos;
    }

    ///// <summary>
    ///// 选择目标  支持距离，类型，阵营过滤
    ///// </summary>
    ///// <param name="skillId"></param>
    ///// <returns></returns>
    //uint SelectTarget(CfgSkill cfg)
    //{
    //    var args = cfg.selectTargetParam;
    //    if (args.Count == 0)
    //        return 0;

    //    SelectTargetType selectType = (SelectTargetType)StringUtil.ParseIntFromList(args, 0, 1);
    //    if (selectType == SelectTargetType.None)
    //        return 0;

    //    var targetList = SelectTargetByParams(cfg.range, args);
    //    if (targetList.Count == 0)
    //        return 0;

    //    ////统一排序，放置多次排序
    //    //if (_sortHelper == null)
    //    //    _sortHelper = new EntitySortHelper();
    //    //_sortHelper.Init(this, cfg.sortParam);
    //    //_sortHelper.SortList(targetList);
    //    EntitySortHelper.SortEntities(this, cfg.sortParam, targetList);

    //    return targetList[0].uid;
    //}

    //List<EntityBase> SelectTargetByParams(float range, List<string> args)
    //{
    //    //player + monster
    //    int targetType = StringUtil.ParseIntFromList(args, 1, 6);
    //    //敌方
    //    int campType = StringUtil.ParseIntFromList(args, 2, 2);

    //    //选择目标默认使用圆形
    //    return Util.DefalutSelectTarget(this, targetType, campType, DamageRangeType.Circle, null, range);
    //}

    bool CheckCanUseSkill(int skillId)
    {
        if (!HasComponent<SkillControlComponent>())
            return false;
        if (_smMgr.GetCurrentState() == UnitState.Skill || _smMgr.GetCurrentState() == UnitState.Die)
            return false;
        return GetComponent<SkillControlComponent>().CheckCanUseSkill(skillId);
    }



}
