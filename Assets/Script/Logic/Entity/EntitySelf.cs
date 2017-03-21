using UnityEngine;
using System.Collections.Generic;


public class EntitySelf : EntityPlayer
{
	public void MoveByDirAndSpeedImmediately(float dirX, float dirZ, float speed)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().MoveByDirAndSpeedImmediately(dirX, dirZ, speed);
		}
	}
	
	protected override void InitModel(GameObject model)
	{
		base.InitModel(model);
		EventManager.Send(Events.GameEvent.OnSelfModelLoaded, model);
	}

}