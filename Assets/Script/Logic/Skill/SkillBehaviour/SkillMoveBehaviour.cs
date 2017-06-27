using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveBehaviour : SkillBehaviourBase
{
    int _moveCfgId;
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
        _moveCfgId = StringUtil.ParseIntFromList(valueList, 1);

    }


    public override void Update(float delTime)
    {

    }
}
