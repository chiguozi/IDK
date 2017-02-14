using System;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager
{
	static Dictionary<Type, ControlBase> _controlMap = new Dictionary<Type, ControlBase>();

	public static void Regist(ControlBase control)
	{
		var type = control.GetType();
		if(_controlMap.ContainsKey(type))
		{
			Debug.LogError("重复加载control" + type.Name);
		}
		else
		{
			_controlMap.Add(type, control);
		}
	}

	public static T Get<T>() where T : ControlBase
	{
		var type = typeof(T);
		ControlBase control;
		if(_controlMap.TryGetValue(type, out control))
		{
			return (T)control;
		}
		Debug.LogError("找不到control" + type.Name);
		return null;
	}

	public static void Init()
	{
		ControlDeclarer.Regist();
		var iter = _controlMap.GetEnumerator();
		while(iter.MoveNext())
		{
			iter.Current.Value.Init();
		}
	}

	public static void Update()
	{
		var iter = _controlMap.GetEnumerator();
		while(iter.MoveNext())
		{
			iter.Current.Value.Update();
		}
	}
}

