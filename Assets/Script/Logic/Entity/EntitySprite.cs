using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@todo 添加战斗属性
public class EntitySprite : EntityBase
{
    public EntityFightAttributeData attributeData;
    protected UnitSMManager _smMgr;
    protected EntitySortHelper _sortHelper;
    protected SkillRuntimeData _skillRuntimeData;

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
    }

    public override void InitDatas()
    {
        base.InitDatas();
        attributeData = new EntityFightAttributeData();
    }

    public Transform GetBone(string bone)
    {
        return _transform.Find(bone);
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


    public virtual void UseSkill(int skillId)
    {
        if (!CheckCanUseSkill(skillId))
            return;
        SendSMEvent(UnitStateEvent.UseSkill);
        uint targetId = SelectTarget(skillId);
        if(_skillRuntimeData == null)
        {
            _skillRuntimeData = new SkillRuntimeData();
            _skillRuntimeData.ownerId = uid;
        }

        _skillRuntimeData.attackedId = targetId;
        _skillRuntimeData.startPos = position;
        //targetPos需要特殊处理
        //_skillRuntimeData.targetPos
        _skillRuntimeData.euler = eulers;

        GetComponent<SkillControlComponent>().UseSkill(skillId, _skillRuntimeData);
    }

    uint SelectTarget(int skillId)
    {
        var cfg = ConfigTextManager.Instance.GetConfig<CfgSkill>(skillId);
        if (cfg == null)
            return 0;

        var args = cfg.selectTargetParam;
        if (args.Count == 0)
            return 0;

        SelectTargetType selectType = (SelectTargetType)StringUtil.ParseIntFromList(args, 0, 1);
        if (selectType == SelectTargetType.None)
            return 0;

        var targetList = SelectTargetByParams(args);
        if (targetList.Count == 0)
            return 0;

        //统一排序，放置多次排序
        if (_sortHelper == null)
            _sortHelper = new EntitySortHelper();
        _sortHelper.Init(this, cfg.sortParam);
        _sortHelper.SortList(targetList);

        return targetList[0].uid;
    }

    List<EntityBase> SelectTargetByParams(List<string> args)
    {
        List<EntityBase> entityList = new List<EntityBase>();
        float range = StringUtil.ParseFloatFromList(args, 1, 0);
        //player + monster
        int targetType = StringUtil.ParseIntFromList(args, 2, 6);
        //敌方
        int campType = StringUtil.ParseIntFromList(args, 3, 2);

        var entites = World.entites;
        var iter = entites.GetEnumerator();
        while(iter.MoveNext())
        {
            var entity = iter.Current.Value;
            if (!CheckTargetType(targetType, entity))
                continue;
            if (!CheckCampType(campType, entity))
                continue;
            if (!CheckEntityDistance(range, entity))
                continue;
        }
        return entityList;
    }

    bool CheckEntityDistance(float range, EntityBase entity)
    {
        if (range == 0)
            return false;
        var sqrDis = ( entity.position - position ).XZSqrMagnitude();
        return sqrDis < range * range;
    }

    bool CheckTargetType(int targetType, EntityBase entity)
    {
        return ((int)entity.entityType & targetType) > 0;
    }


    bool CheckCampType(int campType, EntityBase entity)
    {
        var type = Util.GetTargetCampType(this, entity);
        return ( (int)type & campType ) > 0;
    }

    bool CheckCanUseSkill(int skillId)
    {
        if (!HasComponent<SkillControlComponent>())
            return false;
        return GetComponent<SkillControlComponent>().CheckCanUseSkill(skillId);
    }



}
