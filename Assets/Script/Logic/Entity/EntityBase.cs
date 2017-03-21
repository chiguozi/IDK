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
		_eventMgr.Regist(ComponentEvents.OnMoveEnd, OnMoveEnd);
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
	
	public void RotateByDirAndSpeed(float x, float z, float speed)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().RotateByDirAndSpeed(x,z,speed);
		}
	}
	
	//特殊效果移动
	public void MoveByPos(float x, float z, float speed)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().MoveByPosAndSpeed(x,z,speed);
		}
	}
	
	public void MoveByWayPoints(List<Vector3> wayPoints, float speed)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().MoveByWayPoints(wayPoints, speed);
		}
	}
	
	public void MoveByTargetAndSpeed(Transform target, float speed, float reachDistance = 0.1f, float checkInterval = 0.1f)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().MoveByTargetAndSpeed(target, speed, reachDistance, checkInterval);
		}
	}
	
	public void SetAngleByDir(float dirX, float dirZ)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().SetAngleByDir(dirX, dirZ);
		}
	}
	
	public virtual void Update()
	{
		UpdateComponents();
	}
	
	protected virtual void OnUpdatePos(object obj)
	{
		SetPositionInternal();
	}
	
	protected virtual void OnMoveEnd(object obj)
	{}
	
	protected virtual void OnUpdateAngle(object obj)
	{
		SetEulerInternal();
	}
}