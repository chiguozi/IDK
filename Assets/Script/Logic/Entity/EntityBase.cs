using UnityEngine;
using System.Collections.Generic;


public partial class EntityBase
{
	public EntityBase(EntityBaseData data)
	{
        _entityBaseData = data;
        Init();
        AddComponent();
		RegistEvent();
        InitEntityBaseData(data);
    }

	
	protected virtual void RegistEvent()
	{
		_eventCtrl.Regist<Vector3>(ComponentEvents.UpdatePos, OnUpdatePos);
        _eventCtrl.Regist(ComponentEvents.UpdateAngle, OnUpdateAngle);
    }

    protected virtual void AddComponent()
	{
	}
	
	protected virtual void Init()
	{
		_eventCtrl = new EventController();
	}

    public Transform GetBone(string bone)
    {
        return _transform.Find(bone);
    }

    protected virtual void InitEntityBaseData(EntityBaseData data)
    {
        _entityBaseData = data;
        _position = data.initPos;
        InitDatas();
        if (HasComponent<MoveComponent>())
            GetComponent<MoveComponent>().InitData(data);
    }

    public virtual void OnEnterWorld(){}
	public virtual void OnLeaveWorld(){}
	
	public void CrossFade(string clipName, float speed = 1,  bool force = true, float normalizeTime = 0, float duration = 0.1f)
	{
		if(HasComponent<ActionComponent>())
			GetComponent<ActionComponent>().CrossFade(clipName, speed,  force, normalizeTime, duration);
	}
	
	public virtual void Update(float delTime)
	{
		UpdateComponents(delTime);
	}
	
	protected virtual void OnUpdatePos(Vector3 pos)
	{
		SetPositionInternal();
	}
	
	
	protected virtual void OnUpdateAngle()
	{
		SetEulerInternal();
	}
}