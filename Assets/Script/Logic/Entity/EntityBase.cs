using UnityEngine;
using System.Collections.Generic;


public partial class EntityBase
{
	public EntityBase()
	{
		Init();
		AddComponent();
		RegistEvent();
	}

	
	protected virtual void RegistEvent()
	{
		//@todo  下移到entityplayer中
		_eventMgr.Regist(ComponentEvents.UpdatePos, OnUpdatePos);
        _eventMgr.Regist(ComponentEvents.UpdateAngle, OnUpdateAngle);
    }

    protected virtual void AddComponent()
	{
	}
	
	protected virtual void Init()
	{
		_eventMgr = new ComponentEventManager();
	}
	
	public virtual void OnEnterWorld(){}
	public virtual void OnLeaveWorld(){}
	
	public void CrossFade(string clipName, float speed = 1, float duration = 0.1f, bool force = true, float normalizeTime = 0)
	{
		if(HasComponent<ActionComponent>())
			GetComponent<ActionComponent>().CrossFade(clipName, speed, duration, force, normalizeTime);
	}
	
	
	public virtual void Update(float delTime)
	{
		UpdateComponents(delTime);
	}
	
	protected virtual void OnUpdatePos(object obj)
	{
		SetPositionInternal();
	}
	
	
	protected virtual void OnUpdateAngle(object obj)
	{
		SetEulerInternal();
	}
}