using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMSkill : UnitSMBase
{
    public override void Enter(UnitStateEvent evt, object[] param)
    {
        base.Enter(evt, param);
    }

    public override void ProcessEvent(UnitStateEvent evt, object[] param)
    {
        base.ProcessEvent(evt, param);
        if(evt == UnitStateEvent.ClearState)
        {
            change = UnitStateChangeEvent.Exit;
        }
    }
}
