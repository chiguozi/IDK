using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgMoveTransfer : ConfigTextBase
{
	public int directionType;
	public float angleOffset;
	public float range;
	public float speed;
	public int hitStop;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = ParseInt(value);
				break;
			case 1:
				directionType = ParseInt(value);
				break;
			case 2:
				angleOffset = ParseFloat(value);
				break;
			case 3:
				range = ParseFloat(value);
				break;
			case 4:
				speed = ParseFloat(value);
				break;
			case 5:
				hitStop = ParseInt(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
