using UnityEngine;

public class ShakeAction
{
	const float PI = Mathf.PI;
	bool _isShaking = false;
	
	Vector3 _forward = Vector3.forward;
	Vector3 _right = Vector3.right;
	Vector3 _up = Vector3.up;
	Vector3 _offset; //固定偏移
	Vector3 _randomOffset;  //随机偏移
	
	int _count;
	float _interval;
	float _intervalDecay = 1f; //衰减系数
	float _pow = 1f;
	float _zoom;
	
	float _totalTime; //整体shake的完整时间
	float _totalLeftTime; //shake剩余时间
	
	float _duration;  //单词shake的完整时间
	float _time; //单词shake的时间
	
	public bool isShaking {get {return _isShaking;}}
	
	public void Reset()
	{
		_totalTime = _count * _interval;
		_totalLeftTime = _totalTime;
		_duration = _interval;
		_time = 0;
	}
	
	public void Start(int count, float interval, float rotationY, float Vector3 offset, Vector3 ramdowOffset, float pow = 1f, float zoom = 0f, float intervalDecay = 1f)
	{
		_count = count;
		_interval = interval;
		_intervalDecay = intervalDecay;
		Quaternion qua = Quaternion.Euler(0, rotationY, 0);
		_forward = qua * Vector3.forward;
		_right = qua * Vector3.right;
		_offset = offset;
		_randomOffset = _randomOffset;
		_pow = pow;
		_zoom = zoom;
		Reset();
		_isShaking = true;
	}
	
	public void Stop()
	{
		_isShaking = false;
	}
	
	public Vector3 Update()
	{
		if(!_isShaking) return Vector3.zero;
		
		var dt = Time.unscaledDeltaTime;
		_totalLeftTime -= dt;
		if(_totalLeftTime <= 0)
		{
			_isShaking = false;
		}
		else
		{
			_time += dt;
			if(_time >= _duration)
			{
				_time -= _duration;
				_duration = _duration * _intervalDecay;
			}
			if(_duration < 0.02f)
				_isShaking = false;
		}
		if( !_isShaking)
			return Vector3.zero;
		
		float v = Mathf.Cos(_time / _duration * PI);
		
		float pow = Mathf.Pow(_totalLeftTime / _totalTime, pow);
		
		float decay = v * pow;
		
		float x = UnityEngine.Random.Range(-_randomOffset.x, _randomOffset.x) + _offset.x * decay;
		float x = UnityEngine.Random.Range(-_randomOffset.y, _randomOffset.y) + _offset.y * decay;
		float x = UnityEngine.Random.Range(-_randomOffset.z, _randomOffset.z) + (_offset.z + _zoom) * decay;
		return x * _right + y * _up + z * _forward;
	}
}