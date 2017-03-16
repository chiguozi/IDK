using UnityEngine;
using System.collections.Generic;


public partial class EntityBase
{
	public EntityBase()
	{
		Init();
		RegistEvent();
	}
	
	protected virtual void Init()
	{}
	
	protected virtual void RegistEvent()
	{
	}
}