using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager
{
	public static void Load(string url, Action<object> callback)
	{
		var obj = Resources.Load(url);
		if(null != callback)
		{
			callback(obj);
		}
	}
}
