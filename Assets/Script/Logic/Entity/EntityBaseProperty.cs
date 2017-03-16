using UnityEngine;
using System.collections.Generic;


public partial class EntityBase
{
	//protected EntityBaseData _entityBaseData;
	protected ComponentEventManager  _eventMgr;
	protected Vector3 _position = Vector3.zero;
	protected Vector3 _eulers = Vector3.zero;
	protected Vector3 _scale = Vector3.one;
	protected Vector3 _right = Vector3.right;
	protected Vector3 _forward = Vector3.forward;
	
	protected GameObject _gameObject;
	protected Transform _transform;
	
	protected bool _hasLeaveWorld;
	
	public GameObject gameObject {get {return _gameObject;}}
	public GameObject transform {get {return _transform;}}
	
	public Vector3 scale {get {return _scale;}}
	
	public Vector3 position 
	{
		get 
		{
			if(HasComponent<MoveComponent>())
			{
				return GetComponent<MoveComponent>().curPos;
			}
			return _position;
		}
	}
	
	public Vector3 eulers 
	{
		if(HasComponent<MoveComponent>())
			{
				return GetComponent<MoveComponent>().curEulers;
			}
			return _eulers;
		}
	}
	
	
	protected void SetPositionInternal()
	{
		if(_transform != null)
		{
			_transform.position = position;
		}
	}
	
	protected void SetEulerInternal()
	{
		if(_transform != null)
		{
			_transform.eulerAngles = eulers;
		}
	}
	
	protected void SetScaleInternal()
	{
		if(_transform != null)
		{
			_transform.localScale = _scale;
		}
	}
}