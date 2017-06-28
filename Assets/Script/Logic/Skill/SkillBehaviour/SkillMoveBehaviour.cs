using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveBehaviour : SkillBehaviourBase
{
    int _moveCfgId;
    CfgMoveTransfer _cfg;
    SkillDirectionType _directionType;
    public override void Setup(List<string> valueList)
    {
        base.Setup(valueList);
        _moveCfgId = StringUtil.ParseIntFromList(valueList, 1);
        _cfg = ConfigTextManager.Instance.GetConfig<CfgMoveTransfer>(_moveCfgId);
        if(_cfg == null)
        {
            Debug.LogError("找不到 CfgMoveTransfer  Id = " + _moveCfgId);
            return;
        }
        _directionType = (SkillDirectionType)_cfg.directionType;
    }
    public override void Trigger()
    {
        base.Trigger();
        if (_cfg == null)
            return;
        var dstPos = GetDstPos();
        _comEventCtrl.Send(ComponentEvents.MoveToPos, dstPos.x, dstPos.z, _cfg.speed);
    }

    Vector3 GetDstPos()
    {
        var owner = World.GetEntity(runtimeData.ownerId);
        var dstDir = owner.forward ;
        if (_cfg.angleOffset != 0)
            dstDir = Quaternion.Euler(new Vector3(0, _cfg.angleOffset, 0)) * dstDir;
        Vector3 dstPos = owner.position;
        if(_directionType == SkillDirectionType.dir)
        {
            dstPos = owner.position + dstDir * _cfg.range;
        }
        else if(_directionType == SkillDirectionType.pos)
        {
            dstPos = runtimeData.targetPos;
        }
        else if(_directionType == SkillDirectionType.followTarget)
        {
            var target = World.GetEntity(runtimeData.attackedId);
            if (target != null)
                dstPos = target.position;
            else
                dstPos = owner.position + dstDir * _cfg.range;
        }

        //判断不可行走区域  //formatdstPos
        return dstPos;
    }


}
