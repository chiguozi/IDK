using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager
{
	static Dictionary<Type, ModelBase> _modelMap = new Dictionary<Type, ModelBase>();

	public static T Get<T>() where T : ModelBase
	{
		var type = typeof(T);
		ModelBase model;
		if(_modelMap.TryGetValue(type, out model))
		{
			return (T)model;
		}
		else
		{
			Debug.LogError("找不到model ：" + type.Name);
			return null;
		}
	}

	public static void Regist(ModelBase model)
	{
		var type = model.GetType();
		if(_modelMap.ContainsKey(type))
		{
			Debug.LogError("重复注册 ： " + type.Name);
		}
		else
		{
			_modelMap[type] = model;
		}
	}

	public static void Init()
	{
		ModelDeclarer.Regist();
		var iter = _modelMap.GetEnumerator();
		while(iter.MoveNext())
		{
			iter.Current.Value.Init();
		}
	}
}
