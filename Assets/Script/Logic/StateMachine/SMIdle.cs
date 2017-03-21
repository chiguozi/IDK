using UnityEngine;
using System;

public class SMIdle : UnitSMBase
{
	public override void Enter(UnitStateEvent evt, params object[] param)
	{
		base.Enter(evt, param);
		target.CrossFade(AnimStateName.IDLE, 1);
	}
	
	public override void ProcessEvent(UnitStateEvent evt, params object[] param)
	{
		base.ProcessEvent(evt, param);
		if(IsMoveEvt(evt))
		{
			change = UnitStateChangeEvent.Enter;
			nextState = UnitState.Run;
		}
	}
}