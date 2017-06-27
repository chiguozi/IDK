using UnityEngine;
using System.Collections.Generic;


public class EntitySelf : EntityPlayer
{
    public EntitySelf(EntityBaseData data) : base(data) { }
	public void MoveByDirAndSpeedImmediately(float dirX, float dirZ, float speed)
	{
		if(HasComponent<MoveComponent>())
		{
			GetComponent<MoveComponent>().JoystickMove(dirX, dirZ);
		}
	}

    protected override void InitModel(GameObject model)
	{
		base.InitModel(model);
		EventManager.Send(Events.GameEvent.OnSelfModelLoaded, model);
	}
}