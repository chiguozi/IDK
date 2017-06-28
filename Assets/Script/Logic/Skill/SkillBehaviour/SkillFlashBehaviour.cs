using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFlashBehaviour : SkillBehaviourBase
{
    int _flashCfgId;
    CfgMoveFlash _cfg;
    SkillDirectionType _directionType;
    float _duration;
    bool _run = false;
    public override bool needUpdate
    {
        get
        {
            return true;
        }
    }
    public override void Setup(List<string> valueList)
    {
        base.Setup(valueList);
        _flashCfgId = StringUtil.ParseIntFromList(valueList, 1);
        _cfg = ConfigTextManager.Instance.GetConfig<CfgMoveFlash>(_flashCfgId);
        if (_cfg == null)
        {
            Debug.LogError("找不到 CfgMoveFlash  Id = " + _flashCfgId);
            return;
        }
        _directionType = (SkillDirectionType)_cfg.directionType;
       
    }


    public override void Trigger()
    {
        base.Trigger();
        _run = true;
        _duration = _cfg.duration;
    }

    public override void Update(float delTime)
    {
        if (!_run)
            return;
        _duration -= delTime;
        if(_duration <= 0)
        {
            var pos = GetDstPos();
            _comEventCtrl.Send(ComponentEvents.FlashToPos, pos.x, pos.z);
            _run = false;
        }
    }


    Vector3 GetDstPos()
    {
        var owner = World.GetEntity(runtimeData.ownerId);
        var dstDir = owner.forward;
        if (_cfg.angleOffset != 0)
            dstDir = Quaternion.Euler(new Vector3(0, _cfg.angleOffset, 0)) * dstDir;
        Vector3 dstPos = owner.position;
        if (_directionType == SkillDirectionType.dir)
        {
            dstPos = owner.position + dstDir * _cfg.range;
        }
        else if (_directionType == SkillDirectionType.pos)
        {
            dstPos = runtimeData.targetPos;
        }
        else if (_directionType == SkillDirectionType.followTarget)
        {
            var target = World.GetEntity(runtimeData.attackedId);
            if (target != null)
                dstPos = target.position;
            else
                dstPos = owner.position + dstDir * _cfg.range;
        }
        dstPos = dstPos + dstDir * _cfg.distanceOffset;
        //判断不可行走区域  //formatdstPos
        return dstPos;
    }
}
