using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase 
{
	public ViewBase()
	{
		ViewManager.Regist(this);
	}
	public virtual void Init()
	{}

	public virtual void Update()
	{}
}
