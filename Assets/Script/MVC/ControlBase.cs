using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBase 
{
	public ControlBase()
	{
		ControlManager.Regist(this);
	}
	public virtual void Init()
	{
		
	}

	public virtual void Update()
	{
		
	}

}
