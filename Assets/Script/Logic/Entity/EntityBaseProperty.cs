using UnityEngine;
using System.Collections.Generic;


public partial class EntityBase
{
	protected EntityBaseData _entityBaseData;
	protected EventController  _eventCtrl;
	protected Vector3 _position = Vector3.zero;
	protected Vector3 _eulers = Vector3.zero;
	protected Vector3 _scale = Vector3.one;
	protected Vector3 _right = Vector3.right;
	protected Vector3 _forward = Vector3.forward;
	
	protected GameObject _gameObject;
	protected Transform _transform;
	
	protected bool _hasLeaveWorld;
	protected bool isDispose;
	
	public GameObject gameObject {get {return _gameObject;}}
	public Transform transform {get {return _transform;}}
	
	public Vector3 scale {get {return _scale;}}

    public uint uid
    {
        get
        {
            if (_entityBaseData == null)
                return 0;
            return _entityBaseData.uid;
        }
    }

    public int campId
    {
        get
        {
            if (_entityBaseData == null)
                return 1;
            return _entityBaseData.campId;
        }
    }
	


    public virtual void InitDatas()
    {

    }
	
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
		get 
		{
			if(HasComponent<MoveComponent>())
			{
				return GetComponent<MoveComponent>().curEulers;
			}
			return _eulers;
		}
	}
	
	public Vector3 forward
	{
		get
		{
			if(HasComponent<MoveComponent>())
				return GetComponent<MoveComponent>().forward;
			return _forward;
		}
	}
	
	//@todo  对于没有moveComponent的Vector3s 赋值
	
	
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