using UnityEngine;
using System.Collections.Generic;
using Object = System.Object;

public class AnimStateName
{
	public const string IDLE = "idle";
	public const string RUN = "run";
}

public class ActionComponent : ComponentBase
{
	//url -> <name, len>  每个url对应的动画时间长度
	static Dictionary<string, Dictionary<string, float>> _clipLengthMap = new Dictionary<string, Dictionary<string, float>>();
	//name -> hash   动作名称对应的哈希值
	static Dictionary<string, int> _clipNameToHashMap = new Dictionary<string, int>();
	//url -> <name, hasState> 记录url对应的资源下有哪些动作   
	//动作的名称和AnimationClip的名称可能不一样，所以分两个表分别存储
	static Dictionary<string, Dictionary<string, bool>> _modelClipMap = new Dictionary<string, Dictionary<string, bool>>();
	
	Animator _animator;
	string _curClipName = AnimStateName.IDLE;
	float _curSpeed = 1;
	string _url;
	
	public string curClipName {get {return _curClipName;}}
	public float curSpeed {get {return _curSpeed;}}

	
	public void CrossFade(string clipName, float speed = 1, bool force = false, float normalizeTime = 0, float duration = 0.1f)
	{
		if(!force && _curClipName == clipName)
		{
			ChangeAnimatorSpeed(speed);
			return;
		}
		_curClipName = clipName;
		if(_animator != null)
		{
			ChangeAnimatorSpeed(speed);
			if(HasState(clipName))
			{
                _animator.CrossFade(_clipNameToHashMap[clipName], duration, 0, normalizeTime);
            }
        }
		//如果需要可以添加事件
	}
	
	
	protected override void RegistEvent()
	{
		Regist<GameObject>(ComponentEvents.OnModelLoaded, OnModelLoaded);
		Regist<string>(ComponentEvents.BeginLoadModel, OnBeginLoad);
        Regist<string, float, bool, float>(ComponentEvents.CrossFade, OnCrossFade);
	}
	
    void OnCrossFade(string clipName, float speed, bool force, float normailizeTime)
    {
        CrossFade(clipName, speed, force, normailizeTime);
    }
	
	void InitClipsLength()
	{
		if(_clipLengthMap.ContainsKey(_url))
			return;
		_clipLengthMap.Add(_url, new Dictionary<string, float>());
		var map = _clipLengthMap[_url];
		var runtimeAnimatorControler = _animator.runtimeAnimatorController;
		int len = runtimeAnimatorControler.animationClips.Length;
		for(int i = 0; i < len; i++)
		{
			var clip = runtimeAnimatorControler.animationClips[i];
			//不需要时用baseLayer.name  全名 半名都支持
			var clipName = clip.name;
			var clipLength = clip.length;
			map.Add(clipName, clipLength);
		}
	}
	
	void ChangeAnimatorSpeed(float speed)
	{
		if(speed < 0)
		{
			speed = 0;
		}
		if(_curSpeed == speed)
			return;
		_curSpeed = speed;
		if(_animator != null)
		{
			_animator.speed = speed;
		}
	}
	
	//判断是否有动作，动态添加到缓存表中
	bool HasState(string clipName)
	{
		if(!_modelClipMap.ContainsKey(_url))
		{
			_modelClipMap.Add(_url, new Dictionary<string, bool>());
		}
		var clipMap = _modelClipMap[_url];
		if(!clipMap.ContainsKey(clipName))
		{
			if(_animator == null)
				return false;
			if(!_clipNameToHashMap.ContainsKey(clipName))
				_clipNameToHashMap.Add(clipName, Animator.StringToHash(clipName));
			clipMap.Add(clipName, _animator.HasState(0, _clipNameToHashMap[clipName]));
		}
		return clipMap[clipName];
	}
	
	void OnModelLoaded(GameObject go)
	{
		if(go == null)
			return;
		_animator = go.GetComponent<Animator>();
		if(_animator != null)
		{
			InitClipsLength();
			CrossFade(_curClipName, _curSpeed, force:true);
		}
	}
	
	void OnBeginLoad(string url)
	{
		_url = url;
	}
}