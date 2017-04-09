using UnityEngine;
using System.Collections.Generic;


public class EntityPlayer : EntityBase
{
	protected UnitSMManager _smMgr;
	
	protected override void AddComponent()
	{
		AddComponent<MoveComponent>();
		AddComponent<ActionComponent>();
	}
	
	public void SendSMEvent(UnitStateEvent evt, params object[] param)
	{
		if(_smMgr != null)
		{
			_smMgr.ProcessEvent(evt, param);
		}
	}
	
	protected override void OnMoveEnd(object obj)
	{
		base.OnMoveEnd(obj);
		//暂时处理
		SendSMEvent(UnitStateEvent.ClearState);
	}
	
	protected virtual void InitStateMachine()
	{
		_smMgr = new UnitSMManager();
		_smMgr.InitState(UnitState.Idle, new SMIdle());
		_smMgr.RegistState(UnitState.Run, new SMRun());
	}
}