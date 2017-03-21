using UnityEngine;
using System.Collections.Generic;


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