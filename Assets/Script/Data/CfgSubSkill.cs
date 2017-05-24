using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgSubSkill : ConfigTextBase
{
	public List<List<string>> skillActionList;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = PraseInt(value);
				break;
			case 1:
				skillActionList = PraseListListString(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
