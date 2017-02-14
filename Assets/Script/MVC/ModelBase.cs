using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBase 
{
	public virtual void Init()
	{
		
	}

	public ModelBase()
	{
		ModelManager.Regist(this);
	}
}
