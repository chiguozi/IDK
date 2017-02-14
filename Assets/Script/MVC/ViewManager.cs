using System;
using System.Collections.Generic;
using UnityEngine;


public class ViewManager
{
	static Dictionary<Type, ViewBase> _controlMap = new Dictionary<Type, ViewBase>();

	public static void Regist(ViewBase view)
	{
		var type = view.GetType();
		if(_controlMap.ContainsKey(type))
		{
			Debug.LogError("重复加载view" + type.Name);
		}
		else
		{
			_controlMap.Add(type, view);
		}
	}

	public static T Get<T>() where T : ViewBase
	{
		var type = typeof(T);
		ViewBase view;
		if(_controlMap.TryGetValue(type, out view))
		{
			return (T)view;
		}
		Debug.LogError("找不到view" + type.Name);
		return null;
	}

	public static void Init()
	{
		ViewDeclarer.Regist();
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
