using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DamageChecker
{
    CfgDamageCheck _cfg;

    //已经存在的时间
    float _esapsedTime;
    //下次伤害检测时间
    float _nextDamageTime;
    //击中次数
    int _hitTotalCount;
    //entity受击次数
    Dictionary<uint, int> _entityIdToHitCountMap = new Dictionary<uint, int>();

    EntityBase _owner;
    Vector3 _checkPos;

    SkillRuntimeData _skillRuntimeData;
    int _campId;

    public Action<List<EntityBase>> OnHitCall;

    DamageRangeType _rangleType
    {
        get
        {
            if (_cfg == null)
                return DamageRangeType.Circle;
            return (DamageRangeType)_cfg.rangeType;
        }
    }

    public DamageChecker(int cfgId, SkillRuntimeData runtimeData)
    {
        _cfg = ConfigTextManager.Instance.GetConfig<CfgDamageCheck>(cfgId);
        if(_cfg == null)
        {
            Debug.LogError("找不到CfgDamageCheck id:" + cfgId);
        }
        _skillRuntimeData = runtimeData;
        _owner = World.GetEntity(runtimeData.ownerId);
        _campId = _owner.campId;
        //@todo将必要信息 单独拷贝出来  放置检测时 entity已经被移除

        _esapsedTime = 0;
        //延时过后立刻检测一次伤害，不需要添加遍量标识是否立刻检测
        _nextDamageTime = _cfg.delay;
    }

    public void UpdatePos(Vector3 position)
    {
        _checkPos = position;
    }


    void Check()
    {
        var list = SelectTarget();
        if (list.Count == 0)
            return;
        //抽象出服务器？
        HitTargets(list);
        if(null != OnHitCall)
        {
            //将整个列表返回
            OnHitCall(list);
        }
    }

    private void HitTargets(List<EntityBase> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var sp = ( list[i] as EntitySprite );
            if (sp != null)
                sp.Hited();
            if (_cfg.maxHitCount > 0)
                _hitTotalCount++;
            if (_cfg.entityHitMaxCount > 0)
            {
                if (_entityIdToHitCountMap.ContainsKey(sp.uid))
                    _entityIdToHitCountMap[sp.uid]++;
                else
                    _entityIdToHitCountMap.Add(sp.uid, 1);
            }
        }
    }

    private List<EntityBase> SelectTarget()
    {
        List<EntityBase> entityList = new List<EntityBase>();
        var entites = World.entites;
        var iter = entites.GetEnumerator();
        while (iter.MoveNext())
        {
            var entity = iter.Current.Value;

            if (!Util.CheckHP(entity as EntitySprite))
                continue;
            if (!Util.CheckTargetType(_cfg.targetType, entity))
                continue;
            if (!Util.CheckCampType(_campId, _cfg.campType, entity))
                continue;
            //单个entity伤害次数
            if (!CheckEntityHitCount(entity))
                continue;
            if (!Util.CheckRange(_rangleType, _checkPos, entity.position, entity.radius, _cfg.rangeParams.ToArray()))
                continue;
            entityList.Add(entity);
        }
        return entityList;
    }

    bool CheckEntityHitCount(EntityBase entity)
    {
        if (_cfg.entityHitMaxCount == 0)
            return true;
        int count = 0;
        _entityIdToHitCountMap.TryGetValue(entity.uid, out count);
        return count < _cfg.entityHitMaxCount;
    }

    public void Update(float delTime)
    {
        if (_cfg == null || (_cfg.totalTime > 0 && _esapsedTime > _cfg.totalTime))
            return;
        if (_cfg.maxHitCount > 0 && _hitTotalCount >= _cfg.maxHitCount)
            return;
        _esapsedTime += delTime;
        
        if(_esapsedTime >= _nextDamageTime)
        {
            _nextDamageTime += _cfg.checkInterval;
            Check();
        }
    }

}
