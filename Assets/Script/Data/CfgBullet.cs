using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgBullet : ConfigTextBase
{
	public string url;
	public float speed;
	public int moveType;
	public float lifeTime;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = ParseInt(value);
				break;
			case 1:
				url = ParseString(value);
				break;
			case 2:
				speed = ParseFloat(value);
				break;
			case 3:
				moveType = ParseInt(value);
				break;
			case 4:
				lifeTime = ParseFloat(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
