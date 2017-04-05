using UnityEngine;

public class CameraManager : MonoBehaviour
{
	Camera _camera;
	GameObject _gameObject;
	Transform _transform;
	
	Vector3 _offsetPos;
	float _distance;
	float _duration = 0f;
	
	Vector3 _from;
	Vector3 _to;
	Transform _target;
	Vector3 _destPoint;
	bool _isMoving = false;
	float _time;
	float _timeTotal;
	Vector3 _velocity = Vector3.zero;
	
	bool _needTweenToDistance = false;
	float _fromDistance = 0;
	float _toDistance = 0;
	float _tweenDistanceDuration = 0f;
	float _tweenDistanceLeftTime = 0;
	
	ShakeAction _shakeAction;
	
	void Awake()
	{
		_gameObject = gameObject;
		_transform = _gameObject.transform;
		_camera = GetComponent<Camera>();
		_camera.nearClipPlane = 5.5f;
		_camera.farClipPlane = 220;
	}
	
	public void SetCameraParams(float fieldOfView, CameraClearFlags clearFlags, Color bgColor, bool orthographic = false, float orthographicSize = 10f)
	{
		if(_camera == null)
			return;
		_camera.fieldOfView = fieldOfView;
		_camera.clearFlags = clearFlags;
		_camera.backgroundColor = bgColor;
		_camera.orthographic = orthographic;
		_camera.orthographicSize = orthographicSize;
	}
	
	public void SetParams(Vector3 rotation, Vector3 offsetPos, float distance)
	{
		_transform.rotation = Quaternion.Euler(rotation);
		offsetPos.Normalize();
		_offsetPos = Quaternion.Euler(0, rotation.y, 0) * offsetPos;
		
		_distance = distance;
		_toDistance = _distance;
		_needTweenToDistance = false;
		_destPoint = _to + _offsetPos * _distance;
		
		if(!_isMoving)
		{
			_transform.position = _destPoint;
		}
	}
	
	public void SetDistance(float distance, float time)
	{
		_needTweenToDistance = true;
		_fromDistance = distance;
		_toDistance = distance;
		_tweenDistanceDuration = time;
		_tweenDistanceLeftTime = _tweenDistanceDuration;
	}
	
	public void SetTarget(Transform target, float duration)
	{
		_target = target;
		_duration = duration;
		if(_target != null)
			MoveTo(_target.position);
	}
	
	public void SetPosition(Vector3 pos, float duration)
	{
		_duration = duration;
		MoveTo(_target.position);
	}
	
	void MoveTo(Vector3 pos)
	{
		_to = pos;
		_from = _transform.position;
		_destPoint = _to + _offsetPos * _distance;
		if(_isMoving)
		{
			_isMoving = true;
			_timeTotal = _time = _duration;
		}
	}
	
	public void StartShake(int count, float interval, float rotationY, Vector3 offset, Vector3 randomOffset, float pow = 1f, float zoom = 0f, float intervalDecay = 1f)
	{
		if(_shakeAction == null)
			_shakeAction = new ShakeAction();
		_shakeAction.Start(count, interval, rotationY, offset, randomOffset, pow, zoom, intervalDecay);
	}
	
	public void StopShake()
	{
		if(_shakeAction != null)
			_shakeAction.Stop();
	}
	
	public bool isShaking {get {return null != _shakeAction && _shakeAction.isShaking;}}
	
	bool _isDirty;
	
	void LateUpdate()
	{
		_isDirty = false;
		var dt = Time.unscaledDeltaTime;
		if(_needTweenToDistance)
		{
			_isDirty = true;
			_tweenDistanceLeftTime -= dt;
			if(_tweenDistanceLeftTime <= 0)
			{
				_distance = _toDistance;
				_needTweenToDistance = false;
			}
			else
			{
				var v = Mathf.Clamp01(_tweenDistanceLeftTime / _tweenDistanceDuration);
				_distance = v * _fromDistance + (1 - v) * _toDistance;
			}
			_destPoint = _to + _offsetPos * _distance;
		}
		if( null != _target && _target.position != _to)
		{
			SetTarget(_target, _duration);
		}
		Vector3 pos = _destPoint;
		if(_isMoving)
		{
			_isDirty = true;
			_time -= dt;
			if(_time <= 0)
				_isMoving = false;
			else
			{
				pos = Vector3.SmoothDamp(_transform.position, _destPoint, ref _velocity, _duration);
			}
			if(isShaking)
			{
				_isDirty = true;
				Vector3 shakeOffset = _shakeAction.Update();
				pos += shakeOffset;
			}
			if(_isDirty)
				_transform.position = pos;
		}
	}	
}