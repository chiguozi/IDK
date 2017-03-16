using System;
using System.Collections.Generic;

public class EventManager
{
	static Dictionary<string, Action<Object>> _eventMap = new Dictionary<string, Action<Object>>();
	public static void Regist(string eventType, Action<Object> callback)
	{
		if(_eventMap.ContainsKey(eventType))
		{
			_eventMap[eventType] += callback;
		}
		else
		{
			_eventMap.Add(eventType, callback);
		}
	}
	
	public static void UnRegist(string eventType, Action<Object> callback)
	{
		if(!_eventMap.ContainsKey(eventType))
			return;
		_eventMap[eventType] -= callback;
		if(_eventMap[eventType] == null)
			_eventMap.Remove(eventType);
	}
	
	public static void Send(string eventType, Object obj)
	{
		if(_eventMap.ContainsKey(eventType))
		{
			//不直接调用，防止异常时，中断所有回调。
			var callbacks = _eventMap[eventType].GetInvocationList();
			for(int i = 0; i < callbacks.Length; i++)
			{
				var callback = callbacks[i] as Action<Object>;
				if(callback != null)
				{
					callback(obj);
				}
			}
		}
	}
}