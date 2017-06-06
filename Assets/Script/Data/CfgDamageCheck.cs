using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgDamageCheck : ConfigTextBase
{
	public int rangeType;
	public List<float> rangeParams;
	public float angleOffset;
	public float delay;
	public float checkInterval;
	public float totalTime;
	public int maxHitCount;
	public int entityHitMaxCount;
	public int targetType;
	public int campType;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = ParseInt(value);
				break;
			case 1:
				rangeType = ParseInt(value);
				break;
			case 2:
				rangeParams = ParseListFloat(value);
				break;
			case 3:
				angleOffset = ParseFloat(value);
				break;
			case 4:
				delay = ParseFloat(value);
				break;
			case 5:
				checkInterval = ParseFloat(value);
				break;
			case 6:
				totalTime = ParseFloat(value);
				break;
			case 7:
				maxHitCount = ParseInt(value);
				break;
			case 8:
				entityHitMaxCount = ParseInt(value);
				break;
			case 9:
				targetType = ParseInt(value);
				break;
			case 10:
				campType = ParseInt(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
