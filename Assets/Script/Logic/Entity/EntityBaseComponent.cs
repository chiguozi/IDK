using System;
using System.Collections.Generic;


public partial class EntityBase
{
	protected Dictionary<Type, ComponentBase> _componentMap = new Dictionary<Type, ComponentBase>();
	
	protected T GetComponent<T>() where T : ComponentBase
	{
		var type = typeof(T);
		if(_componentMap.ContainsKey(type))
			return _componentMap[type] as T;
		return null;
	}
	
	protected bool HasComponent<T>() where T : ComponentBase
	{
		var type = typeof(T);
		return _componentMap.ContainsKey(type);
	}
	
	protected void AddComponent<T>() where T : ComponentBase
	{
		var type = typeof(T);
		var com = Activator.CreateInstance<T>();
		com.Init(_eventCtrl);
		_componentMap.Add(type, com);
	}
	
	protected void UpdateComponents(float delTime)
	{
		var iter = _componentMap.GetEnumerator();
		while(iter.MoveNext())
		{
			iter.Current.Value.Update(delTime);
		}
	}
	
}