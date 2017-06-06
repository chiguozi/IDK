using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageChecker
{
    CfgDamageCheck _cfg;

    float _esapsedTime;
    float _nextDamageTime;

    int _hitTotalCount;
    //entity受击次数
    Dictionary<int, int> _entityIdToHitCountMap = new Dictionary<int, int>();

    DamageRangeType _rangleType
    {
        get
        {
            if (_cfg == null)
                return DamageRangeType.Circle;
            return (DamageRangeType)_cfg.rangeType;
        }
    }

    public DamageChecker(int cfgId)
    {
        _cfg = ConfigTextManager.Instance.GetConfig<CfgDamageCheck>(cfgId);
        if(_cfg == null)
        {
            Debug.LogError("找不到CfgDamageCheck id:" + cfgId);
        }
        _esapsedTime = 0;
        //延时过后立刻检测一次伤害，不需要添加遍量标识是否立刻检测
        _nextDamageTime = _cfg.delay;
    }


    void Check()
    {

    }

    public void Update(float delTime)
    {
        if (_cfg == null || _esapsedTime > _cfg.totalTime)
            return;
        _esapsedTime += delTime;
        
        if(_esapsedTime >= _nextDamageTime)
        {
            _nextDamageTime += _cfg.checkInterval;
            Check();
        }
    }

}
