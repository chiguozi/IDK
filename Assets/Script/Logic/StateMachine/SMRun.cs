using UnityEngine;
using System.Collections.Generic;
using System;

public class SMRun : UnitSMBase
{
	public override void Enter(UnitStateEvent evt, params object[] param)
	{
		base.Enter(evt, param);
		DoMove(evt, param);
		target.CrossFade(AnimStateName.RUN, 1);
	}
	
	void DoMove(UnitStateEvent evt, params object[] param)
	{
		if(evt == UnitStateEvent.MoveByDir)
		{
			var entity = target as EntitySelf;
			if(entity == null)
				return;
            float x = Convert.ToSingle(param[0]);
            float z = Convert.ToSingle(param[1]);
            float speed = Convert.ToSingle(param[2]);
            if (x == 0 && z == 0)
            {
                change = UnitStateChangeEvent.Exit;
                return;
            }
			entity.MoveByDirAndSpeedImmediately(x, z, speed);
            entity.SetAngleByDir(x, z);
		}
		else if(evt == UnitStateEvent.MoveToPos)
		{
			var entity = target as EntityPlayer;
			if(entity == null)
				return;
			entity.MoveToPos(Convert.ToSingle(param[0]), Convert.ToSingle(param[1]),Convert.ToSingle(param[2]));
		}
		else if(evt == UnitStateEvent.MoveByPosList)
		{
			var entity = target as EntityPlayer;
			if(entity == null)
				return;
			entity.MoveByWayPoints((List<Vector3>)param[0], Convert.ToSingle(param[1]));
		}
		else if(evt == UnitStateEvent.MoveByTarget)
		{
			var entity = target as EntityPlayer;
			if(entity == null)
				return;
			entity.MoveByTargetAndSpeed((Transform)param[0], Convert.ToSingle(param[1]));
		}
	}
	
	public override void ProcessEvent(UnitStateEvent evt, params object[] param)
	{
		base.ProcessEvent(evt, param);
		if(IsMoveEvt(evt))
		{
			DoMove(evt, param);
			return;
		}
		//‘› ±¥¶¿Ì
		if(evt == UnitStateEvent.ClearState)
		{
			change = UnitStateChangeEvent.Clear;
		}
	}


    public override UnitState state
    {
        get
        {
            return UnitState.Run;
        }
    }
}